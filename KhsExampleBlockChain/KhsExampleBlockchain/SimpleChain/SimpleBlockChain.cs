using KhsExampleBlockchain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhsExampleBlockchain.SimpleChain
{
    public class SimpleBlockchain<T> where T : Tx
    {
        public const int BLOCK_SIZE = 10;
        public List<Block<T>> chain = new List<Block<T>>();

        public SimpleBlockchain()
        {
            // create genesis block
            chain.Add(newBlock());
        }

        public SimpleBlockchain(List<Block<T>> blocks)
            : base()
        {

            chain = blocks;
        }

        public Block<T> getHead()
        {

            Block<T> result = null;
            if (this.chain.Count() > 0)
            {
                result = this.chain.ElementAt(this.chain.Count() - 1);
            }
            else
            {

                throw new RuntimeException("No Block's have been added to chain...");
            }

            return result;
        }

        public void addAndValidateBlock(Block<T> block)
        {

            // compare previous block hash back to genesis hash
            Block<T> current = block;
            for (int i = chain.Count() - 1; i >= 0; i--)
            {
                Block<T> b = chain.ElementAt(i);
                if (b.getHash().Equals(current.getPreviousHash()))
                {
                    current = b;
                }
                else
                {

                    throw new RuntimeException("Block Invalid");
                }

            }

            this.chain.Add(block);

        }

        public Block<T> newBlock()
        {
            int count = chain.Count();
            string previousHash = "root";

            if (count > 0)
                previousHash = blockChainHash();

            Block<T> block = new Block<T>();

            block.setTimeStamp(TimeUtils.CurrentTimeMillis());
            block.setIndex(count);
            block.setPreviousHash(previousHash);
            // chain.add(block);
            return block;
        }

        public SimpleBlockchain<T> add(T item)
        {

            if (chain.Count() == 0)
            {
                // genesis block
                newBlock();
            }

            // See if head block is full
            if (getHead().getTransactions().Count() >= BLOCK_SIZE)
            {
                newBlock();
            }

            getHead().add(item);

            return this;
        }

        /* Deletes the index of the after. */
        public void DeleteAfterIndex(int index)
        {
            if (index >= 0)
            {
                Predicate<Block<T>> predicate = b => chain.IndexOf(b) >= index;
                chain.RemoveAll(predicate);
            }
        }

        public SimpleBlockchain<T> Clone()
        {
            List<Block<T>> clonedChain = new List<Block<T>>();
            Action<Block<T>> consumer = (b) => clonedChain.Add(b.Clone());
            chain.ForEach(consumer);
            return new SimpleBlockchain<T>(clonedChain);
        }

        public List<Block<T>> getChain()
        {
            return chain;
        }

        public void setChain(List<Block<T>> chain)
        {
            this.chain = chain;
        }

        /* Gets the root hash. */
        public string blockChainHash()
        {
            return getHead().getHash();
        }

    }
}
