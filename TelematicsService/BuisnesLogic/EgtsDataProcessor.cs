using Egts;
using Egts.Data;
using Egts.Data.ServiceLayer;
using Egts.Data.ServiceLayer.TeledataService;
using Egts.Processing;
using System;
using Serilog;

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
                Log.Debug("В записи {RecordNumber} указан неподдерживаемый сервис {RecipientService}. Результат процессинга {ProcessingCode}"
                    , record.RecordNumber, record.RecipientService, ProcessingCode.EGTS_PC_SRVC_NFOUND);

                return ProcessingCode.EGTS_PC_SRVC_NFOUND;
            }

            foreach (ServiceDataSubrecord subrecord in record.RecordData)
            {
                if (subrecord.Type != SubrecordType.EGTS_SR_POS_DATA)
                {
                    Log.Debug("Не задан обработчик для подзаписи типа {SubrecordType}", subrecord.Type);
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

                    Log.Information("Получены координаты по трекеру {ObjectId} от {NavigationTime}. Номер записи {RecordNumber}", data.Id, data.Time, record.RecordNumber);
                    Log.Verbose("{@PosData}", data);

                    try
                    {
                        //Storage.WritePosData(record.ObjectID, data);
                        geoService.PutPosData(data.Id, data.Time, data.Lat, data.Lon, data.Direction, data.Speed, data.Odometer, data.Valid, data.Actual, data.Moving);
                        Log.Debug("Данные отправлены в сервис.");
                    }
                    catch (System.Exception e)
                    {
                        Log.Error(e, "Не удалось отправить данные в сервис.");
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
