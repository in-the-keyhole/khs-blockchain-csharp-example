using KhsExampleBlockchain.SimpleChain;
using KhsExampleBlockchain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KhsExampleBlockchain.SimpleChain
{
    public class Miner<T> where T : Tx
    {

        List<T> transactionPool = new List<T>();
        SimpleBlockchain<T> chain = null;

        public Miner(SimpleBlockchain<T> chain)
        {
            this.chain = chain;
        }

        public void mine(T tx)
        {
            transactionPool.Add(tx);
            if (transactionPool.Count() > SimpleBlockchain<T>.BLOCK_SIZE)
            {
                createBlockAndApplyToChain();
            }
        }

        private void createBlockAndApplyToChain()
        {

            Block<T> block = chain.newBlock();
            // set previous hash with current hash
            block.setPreviousHash(chain.getHead().getHash());
            // set block hashes from POW
            // block
            block.setHash(proofOfWork(block));
            chain.addAndValidateBlock(block);
            // empty pool
            transactionPool = new List<T>();
        }

        private string proofOfWork(Block<T> block)
        {

            string nonceKey = block.getNonce();
            long nonce = 0;
            bool nonceFound = false;
            string nonceHash = "";


            string serializedData = JsonConvert.SerializeObject(transactionPool);
            string message = block.getTimeStamp() + block.getIndex() + block.getMerkleRoot() + serializedData
                    + block.getPreviousHash();

            while (!nonceFound)
            {

                nonceHash = SHA256.generateHash(message + nonce);
                nonceFound = nonceHash.Substring(0, nonceKey.Length).Equals(nonceKey);
                nonce++;

            }

            return nonceHash;

        }


    }
}
