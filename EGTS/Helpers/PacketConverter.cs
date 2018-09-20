using System;
using System.Collections.Generic;


namespace EGTS.Helpers
{
    public class PacketConverter : IPacketConverter
    {
        // Коллекция парсеров данных Протокола Уровня Поддержки Услуг.
        private readonly Dictionary<Types.PacketType, Func<object, byte[]>> PacketDataToBytes = new Dictionary<Types.PacketType, Func<object, byte[]>>();

        // Коллекция парсеров подзаписей Протокола Уровня Поддержки Услуг.
        private readonly Dictionary<Types.SubrecordType, Func<byte[], object>> SubrecordFromBytes = new Dictionary<Types.SubrecordType, Func<byte[], object>>();
        private readonly Dictionary<Types.SubrecordType, Func<object, byte[]>> SubrecordToBytes = new Dictionary<Types.SubrecordType, Func<object, byte[]>>();

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public PacketConverter()
        {
            logger.Debug("Инициализация конвертера пакетов EGTS.");

            // Регистрация парсеров
            SubrecordFromBytes.Add(Types.SubrecordType.EGTS_SR_POS_DATA, (d) => throw new NotImplementedException());
            SubrecordFromBytes.Add(Types.SubrecordType.EGTS_SR_EXT_POS_DATA, (d) => throw new NotImplementedException());
            SubrecordFromBytes.Add(Types.SubrecordType.EGTS_SR_RECORD_RESPONSE, (d) => throw new NotImplementedException());


            PacketDataToBytes.Add(Types.PacketType.EGTS_PT_APPDATA, (d) => throw new NotImplementedException());
            PacketDataToBytes.Add(Types.PacketType.EGTS_PT_SIGNED_APPDATA, (d) => throw new NotImplementedException());
            PacketDataToBytes.Add(Types.PacketType.EGTS_PT_RESPONSE, (d) => throw new NotImplementedException());

            SubrecordToBytes.Add(Types.SubrecordType.EGTS_SR_POS_DATA, (d) => throw new NotImplementedException());
            SubrecordToBytes.Add(Types.SubrecordType.EGTS_SR_EXT_POS_DATA, (d) => throw new NotImplementedException());
            SubrecordToBytes.Add(Types.SubrecordType.EGTS_SR_RECORD_RESPONSE, (d) => throw new NotImplementedException());

        }


        public Packet FromBytes(byte[] bytes)
        {
            logger.Debug("Reading Packet from byte[]");

            // Чтение заголовка пакета.
            var header = ReadHeader(bytes);

            // TODO: валидация пакета
            //logger.Debug($"Validation result {result}");

            // Чтение данных уровня поддержки услуг, определение начала первой записи.
            var sfrd = ReadSFRD(packetType:header.PT, bytes:header.SFRD);
            
            // TODO: дополнительная валидация пакета


            // Непосредственное создание пакета.
            var packet = new Packet((Types.PacketType)header.PT, sfrd.RPID)
            {
                PID = header.PID,
                RouteInfo = new Types.RouteInfo()
                {
                    Route = header.RTE,
                    PeerAddress = header.PRA,
                    RecipientAddress = header.RCA,
                    TTL = header.TTL,
                    Priority = (Types.Priority)header.PR
                }
            };

            // Чтение и добавление записей пакета.
            ReadSDR(packet: packet, bytes: header.SFRD, startIndex: sfrd.StartIndex);

            return packet;
        }

        

        /// <summary>
        /// Преобразует двоичные данные в структуру заголовка транспортного уровня.
        /// </summary>
        /// <param name="bytes">Двоичные данные пакета EGTS.</param>
        /// <returns>Возвращает структуру, соответствующую заголовку транспортного уровня из протокола EGTS.</returns>
        private Header ReadHeader(byte[] bytes)
        {
            var header = new Header();

            // Разбор заголовка пакета
            header.PRV = bytes[0];
            header.SKID = bytes[1];
            header.Bitfield = bytes[2];
            header.HL = bytes[3];
            header.HE = bytes[4];
            header.FDL = BitConverter.ToUInt16(bytes, 5); // bytes 5 to 6
            header.PID = BitConverter.ToUInt16(bytes, 7); // bytes 7 to 8
            header.PT = bytes[9];

            logger.Trace($"PRV = {header.PRV}");
            logger.Trace($"SKID = {header.SKID}");
            logger.Trace($"Bitfield = {header.Bitfield} ({Convert.ToString(header.Bitfield, 2)})");
            logger.Trace($"HL = {header.HL}");
            logger.Trace($"HE = {header.HE}");
            logger.Trace($"FDL = {header.FDL}");
            logger.Trace($"PID = {header.PID}");
            logger.Trace($"PT = {header.PT}");

            if (header.RTE)
            {
                header.PRA = BitConverter.ToUInt16(bytes, 10); // bytes 10 to 11
                header.RCA = BitConverter.ToUInt16(bytes, 12); // bytes 12 to 13
                header.TTL = bytes[14];
                header.HCS = bytes[15];

                logger.Trace($"PRA = {header.PRA}");
                logger.Trace($"RCA = {header.RCA}");
                logger.Trace($"TTL = {header.TTL}");
            }
            else
            {
                header.HCS = bytes[10];

                logger.Trace($"PRA not present");
                logger.Trace($"RCA not present");
                logger.Trace($"TTL not present");
            }

            logger.Trace($"HCS = {header.HCS}");

            header.HeaderBytes = new byte[header.HL];
            header.SFRD = new byte[header.FDL];

            // Копирование байтов заголовка пакета в структуру
            logger.Trace($"Copy {header.HL} bytes to HeaderBytes");
            Array.Copy(sourceArray: bytes, destinationArray: header.HeaderBytes, length: header.HL);

            if (header.FDL != 0)
            {
                // Копирование байтов тела пакета в структуру
                logger.Trace($"Copy {header.FDL} bytes starting from index {header.HL} to SFRD");
                Array.Copy(sourceArray: bytes, destinationArray: header.SFRD, sourceIndex: header.HL, destinationIndex: 0, length: header.FDL);

                header.SFRCS = BitConverter.ToUInt16(bytes, header.HL + header.FDL);
                logger.Trace($"SFRCS = {header.SFRCS}");
            }
            else
            {
                logger.Trace($"SFRD and SFRCS not present");
            }

            return header;
        }

        /// <summary>
        /// Читает двоичные данные из поля SFRD пакета EGTS.
        /// </summary>
        /// <param name="packetType">Значение поля PT заголовка транспортного уровня.</param>
        /// <param name="bytes">Значение поля SFRD</param>
        /// <returns>Возвращает значения RPID, PR, SIGL, SIGD и индекс начала первой структуры SDR в массиве.</returns>
        private SFRD ReadSFRD(byte packetType, byte[] bytes)
        {

            var sfrd = new SFRD();
            
            switch (packetType)
            {
                case 0:
                    // EGTS_PT_RESPONSE
                    sfrd.RPID = BitConverter.ToUInt16(bytes, 0);
                    sfrd.PR = bytes[2];

                    logger.Trace($"RPID = {sfrd.RPID}");
                    logger.Trace($"PR = {sfrd.PR}");

                    sfrd.SIGL = 0;
                    sfrd.SIGD = new byte[0];

                    sfrd.StartIndex = 3;
                    break;

                case 1:
                    // EGTS_PT_APPDATA 
                    sfrd.RPID = 0;
                    sfrd.PR = 0;
                    sfrd.SIGL = 0;
                    sfrd.SIGD = new byte[0];

                    sfrd.StartIndex = 0;

                    break;

                case 2:
                    // EGTS_PT_SIGNED_APPDATA 
                    sfrd.RPID = 0;
                    sfrd.PR = 0;
                    sfrd.SIGL = BitConverter.ToInt16(bytes, 0);
                    sfrd.SIGD = new byte[sfrd.SIGL];

                    logger.Trace($"SIGL = {sfrd.SIGL}");
                    
                    logger.Trace($"Copy {sfrd.SIGL} bytes starting from index {sfrd.2} to SIGD");
                    Array.Copy(sourceArray: bytes, destinationArray: sfrd.SIGD, sourceIndex: 2, destinationIndex: 0, length: sfrd.SIGL);

                    sfrd.StartIndex = 2 + sfrd.SIGL;

                    break;
            }

            return sfrd;
        }

        /// <summary>
        /// Преобразует двоичные данные поля SFRD в структуры SDR и добавляет их в пакет.
        /// </summary>
        /// <param name="packet">Экземпляр создаваемого пакета, в которые будут добавляться прочитанные записи.</param>
        /// <param name="bytes">Двоичные данные поля SFRD.</param>
        /// <param name="startIndex">Индекс первго байта структуры SDR.</param>
        private void ReadSDR(Packet packet, byte[] bytes, int startIndex)
        {
            int bytesRead = 0;

            while (bytesRead < bytes.Length)    //UNDONE: не учитывается стартовый индекс
            {
                var rec = packet.PacketData.CreateRecord();

                // Смещение индекса относительно начала текущей SDR. Начало SDR будет сдвигаться в конце цикла.
                int currOffset = 0;

                // Длина поля RD
                ushort rl = BitConverter.ToUInt16(bytes, startIndex);  // +2 bytes
                currOffset += 2;

                // Номер записи
                ushort rn = BitConverter.ToUInt16(bytes, startIndex + currOffset); // +2 bytes
                currOffset += 2;

                // Флаги наличия поле
                byte rfl = bytes[startIndex + 4];    // +1 byte
                currOffset += 1;

                // Отправитель на устройстве.
                bool ssod = ((rfl & (1 << 7)) == (1 << 7)); // 7 байт в RFL

                // Получатель на устройстве.
                bool rsod = ((rfl & (1 << 6)) == (1 << 6)); // 6 байт в RFL

                // Принадлежность к группе.
                bool grp = ((rfl & (1 << 5)) == (1 << 5)); // 5 байт в RFL

                // Приоритет обработки.
                bool rpp = ((rfl & (3 << 3)) == (3 << 3)); // 4 и 3 байты в RFL

                // Наличие поля времени.
                bool tmfe = ((rfl & (1 << 2)) == (1 << 2)); // 2 байт в RFL

                // Наличие поля с идентификатором события.
                bool evfe = ((rfl & (1 << 1)) == (1 << 1)); // 1 байт в RFL

                // Наличие поля с идентификатором объекта.
                bool obfe = ((rfl & (1 << 0)) == (1 << 0)); // 0 байт в RFL

                // Далее идет чтение опциональных полей.
                uint oid, evid, tm;
                if (obfe)
                {
                    oid = BitConverter.ToUInt32(bytes, startIndex + currOffset);   // +4 bytes
                    currOffset += 4;
                }

                if (evfe)
                {
                    evid = BitConverter.ToUInt32(bytes, startIndex + currOffset); // +4 bytes
                    currOffset += 4;
                }

                if (tmfe)
                {
                    tm = BitConverter.ToUInt32(bytes, startIndex + currOffset); // +4 byte
                    currOffset += 4;
                }

                // Тип сервиса-отправителя
                byte sst = bytes[startIndex + currOffset];  // +1 byte
                currOffset += 1;

                // Тип сервиса-получателя.
                byte rst = bytes[startIndex + currOffset];  // +1 byte
                currOffset += 1;

                //TODO: ReadRD
            }
        }

        public byte[] ToBytes(Packet packet)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Структура представляет Заголовок Протокола Транспортного Уровня.
        /// </summary>
        private struct Header
        {
            public byte PRV;
            public byte SKID;
            public byte Bitfield;

            public byte PRF => (byte)((Bitfield & (3 << 6)) >> 6);
            public bool RTE => (Bitfield & (1 << 5)) == (1 << 5);
            public byte ENA => (byte)((Bitfield & (3 << 3)) >> 3);
            public bool CMP => (Bitfield & (1 << 2)) == (1 << 2);
            public byte PR => (byte)(Bitfield & (3 << 0));

            public byte HL;
            public byte HE;
            public ushort FDL;
            public ushort PID;
            public byte PT;
            public ushort PRA;
            public ushort RCA;
            public byte TTL;
            public byte HCS;

            public byte[] SFRD;
            public ushort SFRCS;

            public byte[] HeaderBytes;
        }

        /// <summary>
        /// Структура представляет Протокол Уровня Поддержки Услуг.
        /// </summary>
        private struct SFRD
        {
            public short SIGL;
            public byte[] SIGD;

            public ushort RPID;
            public byte PR;

            public int StartIndex;
        }


        #region Parsing
        private void ParseServiceDataRecords(ref byte[] data)
        {
            throw new System.NotImplementedException();
            
        }

    //    private void ParseRecord(ref byte[] data, int firstByte, ref ServiceDataRecord record)
      //  {
            //throw new System.NotImplementedException();
            //int bytesRead = 0;

            //while (bytesRead != record.RecordLength)
            //{
            //    ServiceDataSubrecord subrecord = new ServiceDataSubrecord
            //    {
            //        Type = (SubrecordType)data[firstByte + 0],
            //        Length = BitConverter.ToUInt16(data, firstByte + 1)
            //    };

            //    subrecordParsers.TryGetValue(subrecord.Type, out SubrecordParserDel parser);
            //    parser?.Invoke(ref data, (firstByte + 3), ref subrecord);

            //    record.RecordData.Add(subrecord);

            //    bytesRead = (bytesRead + subrecord.Length + 3);
            //    firstByte += (subrecord.Length + 3);
            //}
        //}

        //private void ParsePosDataSubrecord(ref byte[] data, int firstByte, ref ServiceDataSubrecord subrecord)
        //{
          //  throw new System.NotImplementedException();
            //    Data.ServiceLayer.TeledataService.PosDataSubrecord posData = new Data.ServiceLayer.TeledataService.PosDataSubrecord();

            //    byte flags = data[firstByte + 12];  // 12
            //    posData.NTM = BitConverter.ToUInt32(data, firstByte + 0);   // 0-3
            //    posData.Latitude = (float)BitConverter.ToUInt32(data, firstByte + 4) * 90 / 0xFFFFFFFF * ((((PosDataFlags)flags & PosDataFlags.LAHS) == PosDataFlags.LAHS) ? -1 : 1);   // 4-7
            //    posData.Longitude = (float)BitConverter.ToUInt32(data, firstByte + 8) * 180 / 0xFFFFFFFF * ((((PosDataFlags)flags & PosDataFlags.LOHS) == PosDataFlags.LOHS) ? -1 : 1); // 8-11

            //    posData.Valid = ((PosDataFlags)flags & PosDataFlags.VLD) == PosDataFlags.VLD;
            //    posData.Actual = ((PosDataFlags)flags & PosDataFlags.BB) != PosDataFlags.BB;
            //    posData.Moving = ((PosDataFlags)flags & PosDataFlags.MV) == PosDataFlags.MV;

            //    posData.Speed = (ushort)(BitConverter.ToUInt16(new byte[] { data[firstByte + 13], (byte)(data[firstByte + 14] & 0x3F) }, 0) / 10); // 13-14
            //    posData.Direction = BitConverter.ToUInt16(new byte[] { data[firstByte + 15], (byte)((data[firstByte + 14] & 0x80) >> 7) }, 0); // 15

            //    posData.Odometer = (float)BitConverter.ToUInt32(new byte[] { data[firstByte + 16], data[firstByte + 17], data[firstByte + 18], 0 }, 0) / 10;    // 16-18

            //    posData.DigitalInputs = data[firstByte + 19];   // 19
            //    posData.Source = data[firstByte + 20];  // 20

            //    if (((PosDataFlags)flags & PosDataFlags.ALTE) == PosDataFlags.ALTE)
            //    {
            //        posData.Altitude = BitConverter.ToInt32(new byte[] { data[firstByte + 21], data[firstByte + 22], data[firstByte + 23], 0 }, 0) * (((data[firstByte + 14] & 0x40) == 0x40) ? -1 : 1);    //21-23
            //    }

            //    subrecord.Data = posData;
      //  }


        #endregion

    }
}
