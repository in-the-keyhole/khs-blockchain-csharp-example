using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChain._01.SimpleChain
{
    [Serializable]
    public class Block<T>
    {
        public DateTime TimeStamp { get; set; }

        public int Index { get; set; }

        public T Data { get; set; }

        public string PreviousHash { get; set; }

        public string GetHash()
        {
            var serializedData = JsonConvert.SerializeObject(Data);

            var hash = $"{TimeStamp}{Index}{serializedData}{PreviousHash}".ToSHA512Hash();

            return hash;
        }

        public Block<T> Clone()
        {
            //Object serialized then rehydrated into a new instance of an object so memory conflicts don't happen
            //There are more efficent ways but this is the most reaadable
            var serializedData = JsonConvert.SerializeObject(this);
            var rehydratedObject = JsonConvert.DeserializeObject<Block<T>>(serializedData);

            return rehydratedObject;
        }
    }
}
