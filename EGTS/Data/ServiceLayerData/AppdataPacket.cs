namespace Egts.Data.ServiceLayer
{
    public class AppdataPacket : ServiceFrameData
    {
        public override void Process(ref ProcessingResult result)
        {
            ProcessingCode code = Processor.ProcessServiceFrameData(this);
            foreach (ServiceDataRecord record in this.ServiceDataRecords)
            {
                record.Process(ref result);
            }
        }

        public override byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }
    }
}
