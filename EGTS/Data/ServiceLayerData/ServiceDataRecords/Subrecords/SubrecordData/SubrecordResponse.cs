using System;

namespace Egts.Data.ServiceLayer
{
    class SubrecordResponse : SubrecordData
    {
        /// <summary>CRN (Confirmed Record Number)</summary>
        public ushort ConfirmedRecord { get; set; }

        /// <summary>RST (Record Status)</summary>
        public byte Result { get; set; }


        public override byte[] GetBytes()
        {
            byte[] CRN = BitConverter.GetBytes(ConfirmedRecord);    // 2 bytes

            return new byte[] { CRN[0], CRN[1], Result };   // Total 3 bytes
        }
    }
}
