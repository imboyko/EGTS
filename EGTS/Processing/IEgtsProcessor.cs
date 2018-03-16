using Egts.Data;
using Egts.Data.ServiceLayer;
using System;
using System.Collections.Generic;

namespace Egts.Processing
{
    public interface IEgtsProcessor
    {
        ProcessingCode ProcessPacket(EgtsPacket packet);
        ProcessingCode ProcessServiceFrameData(ServiceFrameData data);
        ProcessingCode ProcessServiceDataRecord(ServiceDataRecord record);
        ProcessingCode ProcessServiceDataSubrecord(ServiceDataSubrecord subrecord);
    }
}