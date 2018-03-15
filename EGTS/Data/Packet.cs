﻿using EGTS.Data.ServiceLayer;
using EGTS.Data.TransportLayer;

namespace EGTS.Data
{
    public class Packet : IGetByteArray
    {
        /// <summary>EGTS Transport Layer Header</summary>
        public TransportHeader Header { get; set; }

        /// <summary>SFRD (Services Frame Data)</summary>
        public ServiceFrameData ServiceFrameData { get; set; }

        /// <summary>SFRCS (Services Frame Data Check Sum)</summary>
        public ushort CRC { get; set; }

        public byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }
    }
}