namespace EGTS.Types
{
    public partial class ServiceDataRecord
    {
        private enum Flags : byte
        {
            OBFE = (1 << 0),
            EVFE = (1 << 1),
            TMFE = (1 << 2),
            RPP = (1 << 3) | (1 << 4),
            GRP = (1 << 5),
            RSOD = (1 << 6),
            SSOD = (1 << 7)
        }
    }

}
