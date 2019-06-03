using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Telematics.EGTS.Types
{
    interface IServiceData
    {
        int Count { get; }

        void Add(DataRecord item);
        void Clear();
        void Remove(DataRecord item);
        void RemoveAt(int index);
    }

    /// <summary>
    /// Предназначен для передачи одной или нескольких структур, содержащих информацию Протокола Уровня Поддержки Услуг
    /// </summary>
    class EGTS_PT_APPDATA : IServiceData
    {
        protected readonly List<DataRecord> _Records;

        public EGTS_PT_APPDATA()
        {
            _Records = new List<DataRecord>();
        }

        public int Count => _Records.Count;

        public void Add(DataRecord item)
        {
            _Records.Add(item);
        }
        public void Clear()
        {
            _Records.Clear();
        }
        public void Remove(DataRecord item)
        {
            _Records.Remove(item);
        }
        public void RemoveAt(int index)
        {
            _Records.RemoveAt(index);
        }
    }

    /// <summary>
    /// С помощью данного типа пакета осуществляется подтверждения пакета Транспортного Уровня. 
    /// Он содержит помимо структур Уровня Поддержки Услуг, информацию о результате обработки данных Протокола Транспортного Уровня, полученного ранее. 
    /// </summary>
    class EGTS_PT_RESPONSE : EGTS_PT_APPDATA
    {
        private ushort _RPID;
        private byte _PR;

        public EGTS_PT_RESPONSE() : base() { }

        public ushort ResponseTo
        {
            get => _RPID;
            set => _RPID = value;
        }
        public Types.ProcessingCode ProcessingCode
        {
            get => (Types.ProcessingCode)_PR;
            set => _PR = (byte)value;
        }
    }

    /// <summary>
    /// Пакет данного типа применяется для передачи помимо структур, содержащих информацию Уровня Поддержки Услуг, также информации о так называемой  «цифровой подписи», идентифицирующей отправителя данного пакета. 
    /// </summary>
    class EGTS_PT_SIGNED_APPDATA : EGTS_PT_APPDATA
    {
        private short _SIGL;
        private byte[] _SIGD;

        public EGTS_PT_SIGNED_APPDATA() : base() { }
    }
}
