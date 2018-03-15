namespace EGTS.Data.TransportLayer
{
    public struct RoutingInfo
    {
        /// <summary>PRA (Peer Address)</summary>
        public ushort PeerAddress { get; set; }

        /// <summary>RCA (Recipient Address)</summary>
        public ushort RecipientAddress { get; set; }

        /// <summary>TTL (Time To Live)</summary>
        public byte TTL { get; internal set; }
    }
}
