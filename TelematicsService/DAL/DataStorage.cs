using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Telematics.DAL
{
    public class DataStorage
    {
        public static ConcurrentDictionary<uint, PosData> Storage = new ConcurrentDictionary<uint, PosData>();

        public void WritePosData(uint id, PosData data)
        {
            Storage.AddOrUpdate(id, data, (key, value)=> data);
        }
    }
}
