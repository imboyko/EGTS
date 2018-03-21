using Egts;
using Egts.Data;
using Egts.Data.ServiceLayer;
using Egts.Data.ServiceLayer.TeledataService;
using Egts.Processing;
using System;
using System.Collections.Generic;

namespace EgtsTest
{
    internal class ToConsoleProcessor : IEgtsProcessor
    {
        private Dictionary<SubrecordType, Action<ServiceDataRecord, SubrecordData>> SubrecordDataProc = new Dictionary<SubrecordType, Action<ServiceDataRecord, SubrecordData>>();

        public ToConsoleProcessor()
        {
            SubrecordDataProc.Add(SubrecordType.EGTS_SR_POS_DATA, ProcessPosDataSubrecord);
        }

        public ProcessingCode ProcessPacket(EgtsPacket packet)
        {
            Console.WriteLine("EGTS Transport Layer:");
            Console.WriteLine("---------------------");

            Console.WriteLine("    Protocol version       - {0}", packet.Header.ProtocolVersion);
            Console.WriteLine("    Security Key ID        - {0}", packet.Header.SecurityKeyId);

            Console.WriteLine("    Flags");
            Console.WriteLine("        Prefix             - {0}", packet.Header.Prefix);
            Console.WriteLine("        Route              - {0}", packet.Header.Route);
            Console.WriteLine("        Encryption Alg     - {0}", packet.Header.EncryptionAlgorithm);
            Console.WriteLine("        Compression        - {0}", packet.Header.Compressed);
            Console.WriteLine("        Priority           - {0}", packet.Header.Priority);

            Console.WriteLine("    Header Length          - {0}", packet.Header.HeaderLength);
            Console.WriteLine("    Header Encoding        - {0}", packet.Header.HeaderEncoding);
            Console.WriteLine("    Frame Data Length      - {0}", packet.Header.FrameDataLength);
            Console.WriteLine("    Packet ID              - {0}", packet.Header.PID);
            Console.WriteLine("    Header Check Sum       - {0}", packet.Header.CRC);
            Console.WriteLine();

            Console.WriteLine("EGTS Service Layer:");
            Console.WriteLine("-------------------");

            Console.WriteLine("    Packet Type            - {0}", packet.Header.Type);
            Console.WriteLine("    Service Layer CS       - {0}", packet.CRC);
            Console.WriteLine();

            return ProcessingCode.EGTS_PC_OK;
        }

        public ProcessingCode ProcessServiceFrameData(ServiceFrameData data)
        {
            if (data is ResponsePacket)
            {

                Console.WriteLine("    Response To            - {0}", ((ResponsePacket)data)?.ResponseTo);
                Console.WriteLine("    Result Code            - {0}", ((ResponsePacket)data)?.ResultCode);
            }
            else if (data is SignedAppdataPacket)
            {
                Console.WriteLine("    Signature Length       - {0}", ((SignedAppdataPacket)data)?.SignatureLength);
                Console.WriteLine("    Signature Data         - {0}", ((SignedAppdataPacket)data)?.SignatureData);
            }
            else if (data is AppdataPacket)
            {
                // Nothing to do
            }

            return ProcessingCode.EGTS_PC_OK;
        }

        public ProcessingCode ProcessServiceDataRecord(ServiceDataRecord record)
        {
            Console.WriteLine("        Service Layer Record:");
            Console.WriteLine("        ---------------------");
            Console.WriteLine("            Record Length             - {0}", record.RecordLength);
            Console.WriteLine("            Record Number             - {0}", record.RecordNumber);
            Console.WriteLine("            Record flags");
            Console.WriteLine("                Sourse Service On Device    - {0}", record.SourceServiceOnDevice);
            Console.WriteLine("                Recipient Service On Device - {0}", record.RecipientServiceOnDevice);
            Console.WriteLine("                Group Flag                  - {0}", record.Group);
            Console.WriteLine("                Record Processing Priority  - {0}", record.ProcessingPriority);
            Console.WriteLine("                Time Field Exists           - {0}", record.TimeFieldExists);
            Console.WriteLine("                Event ID Field Exists       - {0}", record.EventFieldExists);
            Console.WriteLine("                Object ID Field Exists      - {0}", record.ObjectFieldExists);
            Console.WriteLine("            Object Identifier         - {0}", record.ObjectID);
            Console.WriteLine("            Time                      - {0} ({1})", record.TM, record.Time);
            Console.WriteLine("            Source Service Type       - {0}", record.SourceService);
            Console.WriteLine("            Recipient Service Type    - {0}", record.RecipientService);
            Console.WriteLine();

            if(record.RecipientService != Service.EGTS_TELEDATA_SERVICE)
            {
                return ProcessingCode.EGTS_PC_SRVC_NFOUND;
            }

            foreach (ServiceDataSubrecord subrecord in record.RecordData)
            {
                Console.WriteLine("            Subrecord Data:");
                Console.WriteLine("            ---------------");
                Console.WriteLine("                Subrecord Type             - {0}", subrecord.Type);
                Console.WriteLine("                Subrecord Length           - {0}", subrecord.Length);

                SubrecordDataProc.TryGetValue(subrecord.Type, out Action<ServiceDataRecord, SubrecordData> proc);
                proc?.Invoke(record, subrecord.Data);
            }

            return ProcessingCode.EGTS_PC_OK;
        }

        private void ProcessPosDataSubrecord(ServiceDataRecord recod, SubrecordData subrecordData)
        {
            PosDataSubrecord pos = subrecordData as PosDataSubrecord;
            if (pos == null)
            {
                return;
            }

            Console.WriteLine("                Navigation Time            - {0} ({1})", pos.NTM, pos.NavigationTime);
            Console.WriteLine("                Latitude                   - {0}", pos.Latitude);
            Console.WriteLine("                Longitude                  - {0}", pos.Longitude);
            Console.WriteLine("                Flags");
            Console.WriteLine("                   Moving                  - {0}", pos.Moving);
            Console.WriteLine("                   Actual                  - {0}", pos.Actual);
            Console.WriteLine("                   Valid                   - {0}", pos.Valid);
            Console.WriteLine("                Speed                      - {0}", pos.Speed);
            Console.WriteLine("                Direction                  - {0}", pos.Direction);
            Console.WriteLine("                Odometer                   - {0}", pos.Odometer);
            Console.WriteLine("                Altitude                   - {0}", pos.Altitude);
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
