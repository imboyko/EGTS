using Egts.Data;
using Egts.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Egts
{
    public class EgtsProcessor
    {
        private IEgtsProcessor PacketProcessor;
        private EgtsPacketBuilder Builder = new EgtsPacketBuilder();

        public EgtsProcessor(IEgtsProcessor packetProcessor)
        {
            PacketProcessor = packetProcessor;
        }

        public byte[] ProcessData(byte[] data)
        {
            Builder.BuildFromBytes(data);
            EgtsPacket inPacket = Builder.GetPacket();

            Log.Debug("Получен пакет EGTS {@packet}", inPacket);
            ProcessingResult procResult = new ProcessingResult();

            ProcessingCode code = CheckPacket(inPacket, data);

            if (code == ProcessingCode.EGTS_PC_OK)
            {
                Log.Debug("Результат проверки полученного пакета #{PacketId} - {ProcessingCode}", inPacket.Header.PID, code);
                inPacket.SetProcessor(PacketProcessor);
                inPacket.Process(ref procResult);

            }
            else
            {
                Log.Warning("Результат проверки полученного пакета #{PacketId} - {ProcessingCode}", inPacket.Header.PID, code);
                procResult.PacketId = inPacket.Header.PID;
                procResult.Result = code;
            }

            Builder.BuildFromProcessingResult(procResult);
            EgtsPacket outPacket = Builder.GetPacket();
            return outPacket.GetBytes();
        }

        private ProcessingCode CheckPacket(EgtsPacket packet, byte[] source)
        {
            // Check protocol version support
            if (packet.Header.ProtocolVersion != 1 || packet.Header.Prefix != 0)
                return ProcessingCode.EGTS_PC_UNS_PROTOCOL;

            // Check header length
            if (packet.Header.HeaderLength != 11 && packet.Header.HeaderLength != 16)
                return ProcessingCode.EGTS_PC_INC_HEADERFORM;

            // Check header CRC
            if (Validator.GetCrc8(source, (ushort)(packet.Header.HeaderLength - 1)) != packet.Header.CRC)
                return ProcessingCode.EGTS_PC_HEADERCRC_ERROR;

            // TODO: add routing check

            // Check if FDL is zero
            if (packet.Header.FrameDataLength == 0)
                return ProcessingCode.EGTS_PC_OK;

            // Check header CRC
            if (packet.Header.FrameDataLength > 0 && Validator.GetCrc16(source, packet.Header.HeaderLength, packet.Header.FrameDataLength) != packet.CRC)
                return ProcessingCode.EGTS_PC_DATACRC_ERROR;

            // Any encryption algorythm isn't supported
            if (packet.Header.EncryptionAlgorithm != 0)
                return ProcessingCode.EGTS_PC_DECRYPT_ERROR;

            // Compression isn't supported
            if (packet.Header.Compressed)
                return ProcessingCode.EGTS_PC_INC_DATAFORM;

            // No errors
            return ProcessingCode.EGTS_PC_OK;
        }
    }
}
