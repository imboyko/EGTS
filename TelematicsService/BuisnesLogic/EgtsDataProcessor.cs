using Egts;
using Egts.Data;
using Egts.Data.ServiceLayer;
using Egts.Data.ServiceLayer.TeledataService;
using Egts.Processing;
using System;

namespace Telematics.BuisnesLogic
{
    class EgtsDataProcessor : IEgtsProcessor, IDisposable
    {
        private DAL.DataStorage Storage = new DAL.DataStorage();
        private DAL.ExternalServices.GeoServicePortTypeClient geoService = new DAL.ExternalServices.GeoServicePortTypeClient("GeoServiceSoap");

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

                if (subrecord.Data is PosDataSubrecord pos)
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
                    System.Console.Write("RECORD #{0}", record.RecordNumber);
                    System.Console.Write("\tTIME={0}; ID={1}; LAT={2}; LON={3}; DIR={4}; SPD={5}; ODM={6}; VLD={7}; ACT={8}; MOV={9}",
                        data.Time, data.Id, data.Lat, data.Lon, data.Direction, data.Speed, data.Odometer, data.Valid, data.Actual, data.Moving);
                    

#endif
                    try
                    {
                        Storage.WritePosData(record.ObjectID, data);
                        geoService.PutPosData(data.Id, data.Time, data.Lat, data.Lon, data.Direction, data.Speed, data.Odometer, data.Valid, data.Actual, data.Moving);
#if DEBUG
                        System.Console.ForegroundColor = System.ConsoleColor.Green;
                        System.Console.WriteLine("\t[ SUCCESS ]");
                        System.Console.ForegroundColor = System.ConsoleColor.White;
#endif                        
                    }
                    catch (System.Exception e)
                    {
                        System.Console.ForegroundColor = System.ConsoleColor.Red;
                        System.Console.WriteLine("\t[ FAIL ]");
                        System.Console.WriteLine(e);
                        System.Console.ForegroundColor = System.ConsoleColor.White;
                    }
                }
            }
            return ProcessingCode.EGTS_PC_OK;
        }

        public void Dispose()
        {
            geoService.Close();
        }
    }
}
