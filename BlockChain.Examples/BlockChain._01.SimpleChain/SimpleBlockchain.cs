using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlockChain._01.SimpleChain
{
    public class SimpleBlockchain<T>
    {
        private List<Block<T>> _chain;
        private Clock _clock;

        public SimpleBlockchain() : this (new List<Block<T>>())
        {
        }

        private SimpleBlockchain(List<Block<T>> blocks)
        {
            _clock = new Clock();
            _chain = blocks;
        }

        /// <summary>
        /// Gets the <see cref="Block{T}"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="Block{T}"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public Block<T> this[int index] => _chain[index];

        /// <summary>
        /// Gets the count of blocks.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count => _chain.Count;

        /// <summary>
        /// Gets the blockchain hash.
        /// </summary>
        /// <value>
        /// The blockchain hash.
        /// </value>
        public string BlockchainHash => GetRootHash();

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public SimpleBlockchain<T> Add(T item)
        {
            var count = _chain.Count;
            var previousHash = "root";

            if (_chain.Count > 0)
                previousHash = GetRootHash();

            _chain.Add(new Block<T>()
            {
                //Using the high resolution timestamp because by default 
                TimeStamp = _clock.UtcNow,
                Index = count,
                Data = item,
                PreviousHash = previousHash
            });

            return this;
        }

        /// <summary>
        /// Deletes the index of the after.
        /// </summary>
        /// <param name="index">The index.</param>
        public void DeleteAfterIndex(int index)
        {
            if (index >= 0)
                _chain.RemoveRange(index + 1, _chain.Count - index - 1);
        }

        public SimpleBlockchain<T> Clone()
        {
            var clonedChain = _chain
                .Select(block => block.Clone())
                .ToList();

            return new SimpleBlockchain<T>(clonedChain);
        }

        /// <summary>
        /// Gets the root hash.
        /// </summary>
        /// <returns></returns>
        private string GetRootHash()
        {
            var count = _chain.Count;
            return _chain[count - 1].GetHash();
        }
    }
}
