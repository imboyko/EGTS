namespace EGTS.Types
{
    public struct RouteInfo
    {
        /// <summary>
        /// Признак необходимости дальнейшей маршрутизации пакета на удалённую телематическую платформу.
        /// </summary>
        public bool Route { get; set; }
        
        /// <summary>
        /// Приоритет маршрутизации пакета.
        /// </summary>
        public Priority Priority { get; set; }
        
        /// <summary>
        /// Адрес телематической платформы, на которой пакет сгенерирован.
        /// </summary>
        public ushort PeerAddress { get; set; }
       
        /// <summary>
        /// Адрес телематической платформы, для которой пакет предназначен.
        /// </summary>
        public ushort RecipientAddress { get; set; }

        /// <summary>
        /// Время жизни пакета при его маршрутизации между телематическими платформами.
        /// </summary>
        public byte TTL { get; set; }
    }
}
