using Serilog;
using System;

namespace EGTS
{
    public class Packet
    {
        #region Конструкторы
        /// <summary>
        /// Используется для создания пакета типа EGTS_PT_APPDATA или EGTS_PT_SIGNED_APPDATA.
        /// </summary>
        /// 
        /// <param name="type">
        /// Тип пакета. 
        /// Допустимы значения EGTS_PT_APPDATA и EGTS_PT_SIGNED_APPDATA
        /// </param>
        public Packet(Types.PacketType type)
        {
            Log.Debug("Вызов конструктора Packet(type={PacketType})", type);
            Type = type;

            // Инициализация PacketData
            switch (type)
            {
                case Types.PacketType.EGTS_PT_APPDATA:
                    PacketData = new Types.Appdata(this);
                    break;

                case Types.PacketType.EGTS_PT_SIGNED_APPDATA:
                    PacketData = new Types.SignedAppdata(this);
                    break;

                default:
                    throw new ArgumentException(
                        message: $"Недопустимое значение параметра. " +
                            $"Допустимы только значения {Types.PacketType.EGTS_PT_APPDATA.ToString()} и {Types.PacketType.EGTS_PT_SIGNED_APPDATA.ToString()}.",
                        paramName: "type");
            }
        }

        /// <summary>
        /// Используется для создания пакета типа EGTS_PT_RESPONSE.
        /// </summary>
        /// 
        /// <param name="type">
        /// Тип пакета. 
        /// Допустимо значение EGTS_PT_RESPONSE.
        /// </param>
        /// 
        /// <param name="responseTo">
        /// PID пакета, на который формируется ответ.
        /// </param>
        public Packet(Types.PacketType type, ushort responseTo)
        {
            Log.Debug("Вызов конструктора Packet(type={PacketType}, responseTo={responseTo})", type, responseTo);
            Type = type;

            // Инициализация PacketData
            switch (type)
            {
                case Types.PacketType.EGTS_PT_APPDATA:
                    PacketData = new Types.Appdata(this);
                    break;

                case Types.PacketType.EGTS_PT_SIGNED_APPDATA:
                    PacketData = new Types.SignedAppdata(this);
                    break;

                case Types.PacketType.EGTS_PT_RESPONSE:
                    PacketData = new Types.Response(this, responseTo);
                    break;

                default:
                    throw new ArgumentException(
                        message: $"Недопустимое значение параметра.",
                        paramName: "type");
            }
        }
        #endregion

        #region Свойства
        /// <summary>
        /// Тип пакета.
        /// </summary>
        public Types.PacketType Type { get; }

        /// <summary>
        /// Идентификатор пакета.
        /// </summary>
        public ushort PID { get; set; }

        /// <summary>
        /// Данные уровня поддержки услуг.
        /// </summary>
        public Types.IPacketData PacketData { get; }

        /// <summary>
        /// Информация для маршрутизации данного пакета.
        /// </summary>
        public Types.RouteInfo RouteInfo { get; set; }
        #endregion

        public override string ToString()
        {
            return $"{this.Type} #{this.PID}";
        }

    }
}
