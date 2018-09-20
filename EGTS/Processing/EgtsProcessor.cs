using EGTS.Types;
using EGTS.Helpers;

namespace EGTS
{
    public class EgtsProcessor
    {
       // private EgtsPacketBuilder Builder = new EgtsPacketBuilder();


        public byte[] ProcessData(byte[] data)
        {
            throw new System.NotImplementedException();
        }

        private ProcessingCode CheckPacket(Packet packet, byte[] source)
        {
            throw new System.NotImplementedException();

            //// Check protocol version support
            //if (packet.ProtocolVersion != 1 || packet.Prefix != 0)
            //    return ProcessingCode.EGTS_PC_UNS_PROTOCOL;

            //// Check header length
            ////if (packet.HeaderLength != 11 && packet.Header.HeaderLength != 16)
            ////    return ProcessingCode.EGTS_PC_INC_HEADERFORM;

            //// Check header CRC
            //if (Validator.GetCrc8(source, (ushort)(packet.Header.HeaderLength - 1)) != packet.Header.CRC)
            //    return ProcessingCode.EGTS_PC_HEADERCRC_ERROR;

            //// TODO: add routing check

            //// Check if FDL is zero
            //if (packet.Header.FrameDataLength == 0)
            //    return ProcessingCode.EGTS_PC_OK;

            //// Check header CRC
            //if (packet.Header.FrameDataLength > 0 && Validator.GetCrc16(source, packet.Header.HeaderLength, packet.Header.FrameDataLength) != packet.CRC)
            //    return ProcessingCode.EGTS_PC_DATACRC_ERROR;

            //// Any encryption algorythm isn't supported
            //if (packet.Header.EncryptionAlgorithm != 0)
            //    return ProcessingCode.EGTS_PC_DECRYPT_ERROR;

            //// Compression isn't supported
            //if (packet.Header.Compressed)
            //    return ProcessingCode.EGTS_PC_INC_DATAFORM;

            //// No errors
            //return ProcessingCode.EGTS_PC_OK;
        }
    }
}
