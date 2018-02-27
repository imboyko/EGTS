using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGTS.ServiceLayer
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
