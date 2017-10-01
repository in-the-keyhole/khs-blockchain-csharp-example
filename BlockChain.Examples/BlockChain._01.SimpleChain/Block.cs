using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChain._01.SimpleChain
{
    public class Block<T>
    {
        public int Index { get; set; }

        public T Data { get; set; }

        public string PreviousHash { get; set; }

        public string GetHash()
        {
            var serializedData = JsonConvert.SerializeObject(Data);

            var hash = $"{Index}{serializedData}{PreviousHash}".ToSHA512Hash();

            return hash;
        }
    }
}
