namespace Telematics.EGTS
{
    public class EgtsDataRecord
    {
        private ushort _RL;
        private ushort _RN;
        private byte _RFL;
        private uint _OID;
        private uint _EVID;
        private uint _TM;
        private byte _SST;
        private byte _RST;

        private object[] RecordData;
    }
}
