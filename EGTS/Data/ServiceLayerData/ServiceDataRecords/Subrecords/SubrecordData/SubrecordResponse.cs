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
            throw new NotImplementedException();
        }
    }
}
