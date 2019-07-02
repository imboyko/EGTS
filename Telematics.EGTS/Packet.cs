using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Telematics.EGTS
{
    interface IPacket
    {
        Types.PacketType Type { get; set; }
        ushort Identifier { get; set; }
    }

    public class Packet
    {
        #region Construstors
        /// <summary>
        /// Конструктор пакета указанного типа.
        /// </summary>
        /// <param name="type">Тип пакета.</param>
        public Packet(Types.PacketType type)
        {
            _PT = (byte)type;

            // Инициализируем данные прикладного уровня.
            switch (type)
            {
                case Types.PacketType.EGTS_PT_APPDATA:
                    _Data = new Types.EGTS_PT_APPDATA();
                    break;
                case Types.PacketType.EGTS_PT_RESPONSE:
                    _Data = new Types.EGTS_PT_RESPONSE();
                    break;
                case Types.PacketType.EGTS_PT_SIGNED_APPDATA:
                    _Data = new Types.EGTS_PT_SIGNED_APPDATA();
                    break;
            };
        }
        /// <summary>
        /// Конструктор пакета из двоичных данных.
        /// </summary>
        /// <param name="stream">Двоичные данные пакета</param>
        public Packet(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                _PRV = reader.ReadByte();
                _SKID = reader.ReadByte();
                _Flags = reader.ReadByte();
                _HL = reader.ReadByte();
                _HE = reader.ReadByte();
                _FDL = reader.ReadByte();
                _PID = reader.ReadByte();
                _PT = reader.ReadByte();
                // Если Route, то считываем опциональные поля.
                if (Route)
                {
                    _PRA = reader.ReadUInt16();
                    _RCA = reader.ReadUInt16();
                    _TTL = reader.ReadByte();
                }
                // TODO: Считывание данных прикладного уровня.
                //if (_FDL > 0)
                //{
                //    _Data = new PacketData(stream);
                //    _SFRCS = reader.ReadUInt16();
                //}
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Параметр определяет версию используемой структуры заголовка и должен содержать значение 0x01.
        /// </summary>
        public byte ProtocolVersion
        {
            get => _PRV;
            set => _PRV = value;
        }
        /// <summary>
        /// Параметр определяет идентификатор ключа, используемый при шифровании.
        /// </summary>
        public byte SecurityKeyId
        {
            get => _SKID;
            set => _SKID = value;
        }
        /// <summary>
        /// Данный параметр определяет префикс заголовка Транспортного Уровня и для данной версии протокола должен содержать значение 0.
        /// </summary>
        public byte Prefix
        {
            get => (byte)(_Flags & 0b11000000 >> 6);
            set
            {
                if (value != 0)
                    throw new ArgumentOutOfRangeException("value", value, "Для данной версии протокола префикс должен иметь значение 0");
                else
                    _Flags = (byte)((value << 6) | (_Flags & 0b00111111));
            }
        }
        /// <summary>
        /// Определяет необходимость дальнейшей маршрутизации данного пакета на удалённую телематическую платформу, а также наличие опциональных параметров <see cref="PeerAddress"/>, <see cref="RecipientAddress"/>, <see cref="TTL"/>.
        /// </summary>
        public bool Route
        {
            get => (_Flags & 0b00100000) == 0b00100000;
            set => _Flags = (byte)((value ? 0b001000000 : 0) | (_Flags & 0b11011111));
        }
        /// <summary>
        /// Битовое поле определяет код алгоритма, используемый для шифрования данных из поля SFRD. 
        /// Если поле имеет значение 0  то данные в поле SFRD не шифруются.
        /// Состав и коды алгоритмов не определены в данной версии Протокола
        /// </summary>
        public byte EncryptionAlgorithm
        {
            get => (byte)(_Flags & 0b00011000 >> 3);
            private set
            {
                if (value > 3)
                    throw new ArgumentOutOfRangeException("value", value, "Допустимы значения 0 - 3");
                else
                    _Flags = (byte)((value << 3) | (_Flags & 0b00011000));
            }
        }
        /// <summary>
        /// Определяет, используется ли сжатие данных из поля SFRD. 
        /// Если поле имеет значение 1, то данные в поле SFRD считаются сжатыми.  
        /// Алгоритм сжатия не определен в данной версии Протокола.
        /// </summary>
        public bool Compressed
        {
            get => (_Flags & 0b00000100) == 0b00000100;
            private set => _Flags = (byte)((value ? 0b000000100 : 0) | (_Flags & 0b11111011));
        }
        /// <summary>
        /// Определяет приоритет маршрутизации данного пакета. 
        /// <seealso cref="Types.Priority"/>
        /// </summary>
        public Types.Priority Priority
        {
            get => (Types.Priority)(_Flags & 0b00000011);
            set => _Flags = (byte)((byte)value | (_Flags & 0b00000011));
        }
        /// <summary>
        /// Содержит номер пакета Транспортного Уровня.
        /// </summary>
        public ushort PacketIdentifier
        {
            get => _PID;
            set => _PID = value;
        }
        /// <summary>
        /// Тип пакета Транспортного Уровня.
        /// <seealso cref="Types.PacketType"/>
        /// </summary>
        public Types.PacketType PacketType
        {
            get => (Types.PacketType)_PT;
        }
        /// <summary>
        /// Адрес ТП, на которой данный пакет сгенерирован.
        /// Данный адрес является уникальным в рамках связной сети и используется для создания пакета-подтверждения на принимающей стороне.
        /// </summary>
        public ushort PeerAddress
        {
            get => _PRA;
            set => _PRA = value;
        }
        /// <summary>
        /// Адрес ТП, для которой данный пакет предназначен.
        /// По данному адресу производится идентификация принадлежности пакета определённой ТП и его маршрутизация при использовании промежуточных ТП.
        /// </summary>
        public ushort RecipientAddress
        {
            get => _RCA;
            set => _RCA = value;
        }
        /// <summary>
        /// Время жизни пакета при его маршрутизации между ТП.
        /// Значение TTL уменьшается на единицу при трансляции пакета через каждую ТП, при этом пересчитывается контрольная сумма заголовка Транспортного Уровня.
        /// При достижении данным параметром значения 0 и при обнаружении необходимости дальнейшей маршрутизации пакета, происходит уничтожение пакета и выдача подтверждения с соответствующим кодом <see cref="Types.ProcessingCode.EGTS_PC_TTLEXPIRED"/>
        /// </summary>
        public byte TTL
        {
            get => _TTL;
            set => _TTL = value;
        }
        #endregion

        #region Fields
        private byte _PRV;
        private byte _SKID;
        private byte _Flags;
        private byte _HL;
        private byte _HE;
        private ushort _FDL;
        private ushort _PID;
        private readonly byte _PT;
        private ushort _PRA;
        private ushort _RCA;
        private byte _TTL;
        private byte _HCS;
        private ushort _SFRCS;

        private readonly Types.IServiceData _Data;
        #endregion


    }
}
