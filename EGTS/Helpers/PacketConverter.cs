using Serilog;
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

        public PacketConverter()
        {
            Log.Debug("Вызов конструктора PacketConverter()");

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


        /// <summary>
        /// Создает экземпляр пакета из двоичных данных.
        /// </summary>
        /// <param name="bytes">Двоичные данные EGTS.</param>
        /// <returns>Экземпляр пакета.</returns>
        public Packet FromBytes(byte[] bytes)
        {
            Log.Debug("Вызов PacketConverter.FromBytes(byte[] )");
            Log.Verbose("Двоичные данные пакета: {SourceBytes}", bytes);

            // Чтение заголовка пакета.
            var header = ReadHeader(bytes);

            // TODO: валидация пакета
            Log.Warning(new NotImplementedException(), "Валидация заголовка пакета");

            // Чтение данных уровня поддержки услуг, определение начала первой записи.
            var sfrd = ReadSFRD(packetType: header.PT, sfrdBytes: header.SFRD);

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
            ReadSDR(packet: packet, sfrdBytes: header.SFRD, startIndex: sfrd.StartIndex);

            return packet;
        }


        /// <summary>
        /// Преобразует двоичные данные в структуру заголовка транспортного уровня.
        /// </summary>
        /// <param name="bytes">Двоичные данные пакета EGTS.</param>
        /// <returns>Возвращает структуру, соответствующую заголовку транспортного уровня из протокола EGTS.</returns>
        private Types.Header ReadHeader(byte[] bytes)
        {
            Log.Debug("Чтение двоичных данных пакета. Вызов PacketConverter.ReadHeader(byte[] )");

            var header = new Types.Header();

            // Разбор заголовка пакета
            header.PRV = bytes[0];
            header.SKID = bytes[1];
            header.Bitfield = bytes[2];
            header.HL = bytes[3];
            header.HE = bytes[4];
            header.FDL = BitConverter.ToUInt16(bytes, 5); // bytes 5 to 6
            header.PID = BitConverter.ToUInt16(bytes, 7); // bytes 7 to 8
            header.PT = bytes[9];

            Log.Verbose("Protocol Version     PRV: {PRV}", header.PRV);
            Log.Verbose("Security Key ID     SKID: {SKID}", header.SKID);
            Log.Verbose("Header flags            : {HeaderFlags}", header.Bitfield);
            Log.Verbose(" * Prefix                   PRF: {PRF}", header.PRF);
            Log.Verbose(" * Route                    RTE: {RTE}", header.RTE);
            Log.Verbose(" * Encryption Algorithm     ENA: {ENA}", header.ENA);
            Log.Verbose(" * Compressed               CMP: {CMP}", header.CMP);
            Log.Verbose(" * Priority                  PR: {PR}", header.PR);
            Log.Verbose("Header Length         HL: {HL}", header.HL);
            Log.Verbose("Header Encoding       HE: {HE}", header.HE);
            Log.Verbose("Frame Data Length    FDL: {FDL}", header.FDL);
            Log.Verbose("Packet Identifier    PID: {PID}", header.PID);
            Log.Verbose("Packet Type           PT: {PT} [ {PacketType} ]", header.PT, (Types.PacketType)header.PT);

            if (header.RTE)
            {
                header.PRA = BitConverter.ToUInt16(bytes, 10); // bytes 10 to 11
                header.RCA = BitConverter.ToUInt16(bytes, 12); // bytes 12 to 13
                header.TTL = bytes[14];
                header.HCS = bytes[15];
            }
            else
            {
                header.HCS = bytes[10];
            }

            Log.Verbose("Peer Address         PRA: {PRA}", header.RTE ? header.PRA : -1);
            Log.Verbose("Recipient Address    RCA: {RCA}", header.RTE ? header.RCA : -1);
            Log.Verbose("Time To Live         TTL: {TTL}", header.RTE ? header.TTL : -1);
            Log.Verbose("Header Check Sum     HCS: {HCS}", header.HCS);


            header.HeaderBytes = new byte[header.HL];
            header.SFRD = new byte[header.FDL];

            try
            {
                // Копирование байтов заголовка пакета в структуру.
                Array.Copy(sourceArray: bytes, destinationArray: header.HeaderBytes, length: header.HL);
            }
            catch (Exception e)
            {
                Log.Error(e, "Вызов Array.Copy(sourceArray: bytes, destinationArray: header.HeaderBytes, length: header.HL);" +
                    "\nДлина источника sourceArray: {sourceArrayLength};" +
                    "\nДлина приемника destinationArray {destinationArrayLength};" +
                    "\nКопируемых элементов length: {lengthToCopy}",
                    bytes.Length,
                    header.HeaderBytes.Length,
                    header.HL);

                // Выкидываем исключение дальше.
                throw e;
            }

            if (header.FDL != 0)
            {
                try
                {
                    // Копирование байтов тела пакета в структуру
                    Array.Copy(sourceArray: bytes, destinationArray: header.SFRD, sourceIndex: header.HL, destinationIndex: 0, length: header.FDL);
                }
                catch (Exception e)
                {
                    Log.Error(e, "Вызов Array.Copy(sourceArray: bytes, destinationArray: header.SFRD, sourceIndex: header.HL, destinationIndex: 0, length: header.FDL); " +
                    "\nДдлина источника sourceArray: {sourceArrayLength};" +
                    "\nДлина приемника destinationArray {destinationArrayLength};" +
                    "\nНачальный индекс источника {sourceIndex};" +
                    "\nНачальный индекс приемника {destinationIndex};" +
                    "\nКопируемых элементов length: {lengthToCopy}",
                    bytes.Length,
                    header.HeaderBytes.Length,
                    header.HL,
                    0,
                    header.FDL);

                    // Выкидываем исключение дальше.
                    throw new SystemException();
                }

                header.SFRCS = BitConverter.ToUInt16(bytes, header.HL + header.FDL);
                Log.Verbose("SFRD Check Sum     SFRCS: {SFRCS}", header.SFRCS);

            }

            return header;
        }

        /// <summary>
        /// Читает двоичные данные из поля SFRD пакета EGTS.
        /// </summary>
        /// <param name="packetType">Значение поля PT заголовка транспортного уровня.</param>
        /// <param name="sfrdBytes">Значение поля SFRD</param>
        /// <returns>Возвращает значения RPID, PR, SIGL, SIGD и индекс начала первой структуры SDR в массиве.</returns>
        private Types.SFRD ReadSFRD(byte packetType, byte[] sfrdBytes)
        {
            var currMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Log.Debug("Чтение данных уровня услуг. Вызов PacketConverter.ReadSFRD(byte, byte[])");

            var sfrd = new Types.SFRD();

            switch (packetType)
            {
                case 0:
                    // EGTS_PT_RESPONSE
                    sfrd.RPID = BitConverter.ToUInt16(sfrdBytes, 0);
                    Log.Verbose("\tОтвет на пакет RPID = {ResponsePID}", sfrd.RPID);

                    sfrd.PR = sfrdBytes[2];
                    Log.Verbose("\tРезультат обработки PR = {PR}", sfrd.PR);

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
                    sfrd.SIGL = BitConverter.ToInt16(sfrdBytes, 0);
                    Log.Verbose("\tДлина подписи SIGL = {SignatureLength}", sfrd.SIGL);

                    sfrd.SIGD = new byte[sfrd.SIGL];
                    Array.Copy(sourceArray: sfrdBytes, destinationArray: sfrd.SIGD, sourceIndex: 2, destinationIndex: 0, length: sfrd.SIGL);
                    Log.Verbose("\tДанные подписи SIGD = {SignatureData}", sfrd.SIGD);

                    sfrd.StartIndex = 2 + sfrd.SIGL;

                    break;
            }

            return sfrd;
        }

        /// <summary>
        /// Преобразует двоичные данные поля SFRD в структуры SDR и добавляет их в пакет.
        /// </summary>
        /// <param name="packet">Экземпляр создаваемого пакета, в которые будут добавляться прочитанные записи.</param>
        /// <param name="sfrdBytes">Двоичные данные поля SFRD.</param>
        /// <param name="startIndex">Индекс первго байта структуры SDR.</param>
        private void ReadSDR(Packet packet, byte[] sfrdBytes, int startIndex)
        {
            Log.Debug("Чтение SDR. Вызов PacketConverter.ReadSDR(). Позиция первой записи: {SFRDOffset})", packet, sfrdBytes, startIndex);
            int bytesRead = 0;
            int firstRecordIndex = startIndex;

            while (bytesRead < (sfrdBytes.Length - startIndex))
            {
                // Смещение индекса относительно firstRecordIndex. 
                // firstRecordIndex будет сдвигаться в конце цикла.
                int currOffset = 0;

                // Длина поля RD
                ushort rl = BitConverter.ToUInt16(sfrdBytes, firstRecordIndex + currOffset);  // +2 bytes
                Log.Verbose("\tДлина данных записи RL={RecordLength}; текущее смещение {CurrentOffset}, смещение от начала {TotalOffset}", rl, currOffset, firstRecordIndex + currOffset);
                currOffset += 2;

                // Номер записи
                ushort rn = BitConverter.ToUInt16(sfrdBytes, firstRecordIndex + currOffset); // +2 bytes
                Log.Verbose("\tНомер записи RN={RecordNumber}; текущее смещение {CurrentOffset}, смещение от начала {TotalOffset}", rn, currOffset, firstRecordIndex + currOffset);
                currOffset += 2;

                // Флаги наличия полей
                byte rfl = sfrdBytes[firstRecordIndex + currOffset];    // +1 byte
                Log.Verbose("\tФлаги записи RFL={RecordFlags}; текущее смещение {CurrentOffset}, смещение от начала {TotalOffset}", rfl, currOffset, firstRecordIndex + currOffset);
                currOffset += 1;

                // Отправитель на устройстве.
                bool ssod = ((rfl & (1 << 7)) == (1 << 7)); // 7 байт в RFL
                Log.Verbose("\t\tОтправитель на устройстве SSOD={SSOD}", ssod);

                // Получатель на устройстве.
                bool rsod = ((rfl & (1 << 6)) == (1 << 6)); // 6 байт в RFL
                Log.Verbose("\t\tПолучатель на устройстве RSOD={RSOD}", rsod);

                // Принадлежность к группе.
                bool grp = ((rfl & (1 << 5)) == (1 << 5)); // 5 байт в RFL
                Log.Verbose("\t\tПринадлежность группе GRP={GRP}", grp);

                // Приоритет обработки.
                byte rpp = (byte)((rfl & (3 << 3)) >> 3); // 4 и 3 байты в RFL
                Log.Verbose("\t\tПриоритет обработки RPP={RPP}", rpp);

                // Наличие поля времени.
                bool tmfe = ((rfl & (1 << 2)) == (1 << 2)); // 2 байт в RFL
                Log.Verbose("\t\tНаличие поля времени TMFE={TMFE}", tmfe);

                // Наличие поля с идентификатором события.
                bool evfe = ((rfl & (1 << 1)) == (1 << 1)); // 1 байт в RFL
                Log.Verbose("\t\tНаличие поля события EVFE={EVFE}", evfe);

                // Наличие поля с идентификатором объекта.
                bool obfe = ((rfl & (1 << 0)) == (1 << 0)); // 0 байт в RFL
                Log.Verbose("\t\tНаличие поля объекта OBFE={OBFE}", obfe);

                // Далее идет чтение опциональных полей.
                uint oid = 0, evid = 0, tm = 0;
                if (obfe)
                {
                    oid = BitConverter.ToUInt32(sfrdBytes, firstRecordIndex + currOffset);   // +4 bytes
                    Log.Verbose("\tИдентификатор объекта OID={ObjectId}; текущее смещение {CurrentOffset}, смещение от начала {TotalOffset}", oid, currOffset, firstRecordIndex + currOffset);
                    currOffset += 4;
                }

                if (evfe)
                {
                    evid = BitConverter.ToUInt32(sfrdBytes, firstRecordIndex + currOffset); // +4 bytes
                    Log.Verbose("\tИдентификатор события EVID={EventId}; текущее смещение {CurrentOffset}, смещение от начала {TotalOffset}", evid, currOffset, firstRecordIndex + currOffset);
                    currOffset += 4;
                }

                if (tmfe)
                {
                    tm = BitConverter.ToUInt32(sfrdBytes, firstRecordIndex + currOffset); // +4 byte
                    Log.Verbose("\tВремя формирования записи TM={RecordTime}; текущее смещение {CurrentOffset}, смещение от начала {TotalOffset}", tm, currOffset, firstRecordIndex + currOffset);
                    currOffset += 4;
                }

                // Тип сервиса-отправителя
                byte sst = sfrdBytes[firstRecordIndex + currOffset];  // +1 byte
                Log.Verbose("\tСервис-отправитель SST={SourceService}; текущее смещение {CurrentOffset}, смещение от начала {TotalOffset}", sst, currOffset, firstRecordIndex + currOffset);
                currOffset += 1;

                // Тип сервиса-получателя.
                byte rst = sfrdBytes[firstRecordIndex + currOffset];  // +1 byte
                Log.Verbose("\tСервис-получатель SST={RecipientService}; текущее смещение {CurrentOffset}, смещение от начала {TotalOffset}", rst, currOffset, firstRecordIndex + currOffset);
                currOffset += 1;

                var rec = packet.PacketData.CreateRecord();
                rec.Number = rn;
                rec.ObjectID = oid;
                rec.EventID = evid;
                rec.Group = grp;
                rec.Time = new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(tm);
                rec.SourceService = (Types.Service)sst;
                rec.SourceServiceOnDevice = ssod;
                rec.RecipientService = (Types.Service)rst;
                rec.RecipientServiceOnDevice = rsod;
                rec.ProcessingPriority = (Types.Priority)rpp;

                //TODO: ReadRD
                currOffset += rl;

                firstRecordIndex += currOffset;
                bytesRead += currOffset;

                Log.Debug("Прочитано байт {BytesRead} из {TotalBytes}", bytesRead, sfrdBytes.Length - startIndex);
                Log.Information("Прочитана запись {Record}", rec);
            }
        }





        public byte[] ToBytes(Packet packet)
        {
            throw new NotImplementedException();
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
