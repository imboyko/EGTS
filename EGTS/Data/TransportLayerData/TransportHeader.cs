using System;

namespace Egts.Data.TransportLayer
{
    /// <summary>EGTS Transport Layer Header</summary>
    public class TransportHeader:IGetByteArray
    {
        /// <summary>PRV (Protocol Version)</summary>
        public byte ProtocolVersion { get; set; }

        /// <summary>SKID (Security Key ID)</summary>
        public byte SecurityKeyId { get; set; }

        // bitfield values
        /// <summary>PRF (Prefix)</summary>
        public byte Prefix { get; set; }

        /// <summary>RTE (Route)</summary>
        public bool Route { get; set; }

        /// <summary>ENA (Encryption Algorithm)</summary>
        public byte EncryptionAlgorithm { get; set; } = 0;       // TODO: get value from bitfiled, in current version of EGTS protocol not defined, must be 0

        /// <summary>CMP (Compressed)</summary>
        public bool Compressed { get; set; }

        /// <summary>PR (Priority)</summary>
        public Priority Priority { get; set; }

        /// <summary>HL (Header Length)</summary>
        public byte HeaderLength { get; set; }

        /// <summary>HE (Header Encoding)</summary>
        public byte HeaderEncoding { get; set; }

        /// <summary>FDL (Frame Data Length)</summary>
        public ushort FrameDataLength { get; set; }

        /// <summary>PID (Packet Identifier)</summary>
        public ushort PID { get; set; }

        /// <summary>PT (Packet Type)</summary>
        public PacketType Type { get; set; }

        /// <summary>Object that contains roting information (PRA, RCA, TTL)</summary>
        public RoutingInfo RoutingInfo { get; set; }

        /// <summary>HCS (Header Check Sum)</summary>
        public byte CRC { get; set; }

        public byte[] GetBytes()
        {
            byte[] result = new byte[11];

            byte flags = 0;
            if (Route)
            {
                flags = (byte)(flags | (byte)HeaderFlags.RTE);
            }
            if (Compressed)
            {
                flags = (byte)(flags | (byte)HeaderFlags.CMP);
            };
            // TODO: Prefix, ENA
            flags = (byte)(flags | (byte)Priority);

            result[0] = ProtocolVersion;    // 0
            result[1] = SecurityKeyId;      // 1
            result[2] = flags;              // 2
            result[3] = HeaderLength;       // 3
            result[4] = HeaderEncoding;     // 4
            BitConverter.GetBytes(FrameDataLength).CopyTo(result, 5);   // 5-6
            BitConverter.GetBytes(PID).CopyTo(result, 7);   // 7-8
            result[9] = (byte)Type;         // 9
            // TODO: PRA, RCA, TTL
            result[10] = CRC;               // 10

            return result;
        }
    }
}
