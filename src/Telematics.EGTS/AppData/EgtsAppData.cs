using System.Collections;
using System.Collections.Generic;

namespace Telematics.EGTS
{

    /// <summary>
    /// Предназначен для передачи одной или нескольких структур, содержащих информацию Протокола Уровня Поддержки Услуг
    /// </summary>
    public class EgtsAppData : IEgtsAppData
    {
        internal EgtsAppData(EgtsPacket egtsPacket)
        {
            this._EgtsPacket = egtsPacket;
        }

        protected readonly List<EgtsDataRecord> _Records;
        protected readonly EgtsPacket _EgtsPacket;

        #region Реализация ICollection<DataRecord>
        public int Count => _Records.Count;

        public bool IsReadOnly => false;

        public void Add(EgtsDataRecord item)
        {
            _Records.Add(item);
        }

        public void Clear()
        {
            _Records.Clear();
        }

        public bool Contains(EgtsDataRecord item)
        {
            return _Records.Contains(item);
        }

        public void CopyTo(EgtsDataRecord[] array, int arrayIndex)
        {
            _Records.CopyTo(array, arrayIndex);
        }

        public IEnumerator<EgtsDataRecord> GetEnumerator()
        {
            return _Records.GetEnumerator();
        }

        public bool Remove(EgtsDataRecord item)
        {
            return _Records.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Records.GetEnumerator();
        }
        #endregion

        /// <summary>Создает запись уровня поддержки услуг и добавляет ее в список записей.</summary>
        /// <returns>Запись уровня поддержки услуг.</returns>
        public EgtsDataRecord Add()
        {
            var item = new EgtsDataRecord(_EgtsPacket);
            return item;
        }
        
    }

}
