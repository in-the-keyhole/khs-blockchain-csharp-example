using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlockChain._01.SimpleChain
{
    public class SimpleBlockchain<T>
    {
        private List<Block<T>> _chain;

        public SimpleBlockchain()
        {
            _chain = new List<Block<T>>();
        }

        public Block<T> this[int index] => _chain[index];

        public SimpleBlockchain<T> Add(T item)
        {
            var count = _chain.Count;
            var previousHash = "root";

            if (_chain.Count > 0)
                previousHash = GetRootHash();

            _chain.Add(new Block<T>()
            {
                Index = count,
                Data = item,
                PreviousHash = previousHash
            });

            return this;
        }

        public void DeleteAfterIndex(int index)
        {
            if (index >= 0)
                _chain.RemoveRange(index + 1, _chain.Count - index - 1);
        }

        public int Count => _chain.Count;

        public string BlockchainHash => GetRootHash();

        private string GetRootHash()
        {
            var count = _chain.Count;
            return _chain[count - 1].GetHash();
        }
    }
}
