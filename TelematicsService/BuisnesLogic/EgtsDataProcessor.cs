using Egts;
using Egts.Data;
using Egts.Data.ServiceLayer;
using Egts.Data.ServiceLayer.TeledataService;
using Egts.Processing;

namespace Telematics.BuisnesLogic
{
    class EgtsDataProcessor : IEgtsProcessor
    {
        private DAL.DataStorage Storage = new DAL.DataStorage();

        public ProcessingCode ProcessPacket(EgtsPacket packet)
        {
            return ProcessingCode.EGTS_PC_OK;
        }

        public ProcessingCode ProcessServiceFrameData(ServiceFrameData data)
        {
            return ProcessingCode.EGTS_PC_OK;
        }

        public ProcessingCode ProcessServiceDataRecord(ServiceDataRecord record)
        {
            if (record.RecipientService != Service.EGTS_TELEDATA_SERVICE)
            {
                return ProcessingCode.EGTS_PC_SRVC_NFOUND;
            }

            foreach (ServiceDataSubrecord subrecord in record.RecordData)
            {
                if (subrecord.Type != SubrecordType.EGTS_SR_POS_DATA)
                {
                    continue;
                }

                PosDataSubrecord pos = subrecord.Data as PosDataSubrecord;
                if (pos != null)
                {
                    DAL.PosData data = new DAL.PosData
                    {
                        Id = record.ObjectID,
                        Time = pos.NavigationTime,
                        Lat = pos.Latitude,
                        Lon = pos.Longitude,
                        Direction = pos.Direction,
                        Speed = pos.Speed,
                        Odometer = pos.Odometer,
                        Actual = pos.Actual,
                        Valid = pos.Valid,
                        Moving = pos.Moving
                    };
#if DEBUG
                    System.Console.Write("*** RECORD #{0}", record.RecordNumber);
                    System.Console.WriteLine("\tTIME={0}; ID={1}; LAT={2}; LON={3}; DIR={4}; SPD={5}; ODM={6}; VLD={7}; ACT={8}; MOV={9} ***",
                        data.Time, data.Id, data.Lat, data.Lon, data.Direction, data.Speed, data.Odometer, data.Valid, data.Actual, data.Moving);
                    
#endif
                    Storage.WritePosData(record.ObjectID, data);
                }
            }
            return ProcessingCode.EGTS_PC_OK;
        }
    }
}
