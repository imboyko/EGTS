
using EGTS.ServiceLayer;

namespace EGTS.TransportLayer
{
    /// <summary>EGTS Transport Layer Header</summary>
    public class TransportHeader
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

    }
}
