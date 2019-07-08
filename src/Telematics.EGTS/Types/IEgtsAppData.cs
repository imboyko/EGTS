
namespace Telematics.EGTS.Types
{
    interface IEgtsAppData
    {
        int Count { get; }

        void Add(DataRecord item);
        void Clear();
        void Remove(DataRecord item);
        void RemoveAt(int index);
    }
}
