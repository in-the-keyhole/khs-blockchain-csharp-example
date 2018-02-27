using System;
using System.Collections.Generic;
using KhsExampleBlockchain.SimpleChain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestkhsExampleBlockchainCsharp.SimpleChain
{
    [TestClass]
    public class SimpleChainTest
    {
        [TestMethod]
        public void ChainTest()
        {
            SimpleBlockchain<Transaction> chain1 = new SimpleBlockchain<Transaction>();

            chain1.add(new Transaction("A")).add(new Transaction("B")).add(new Transaction("C"));

            SimpleBlockchain<Transaction> chain2 = chain1.Clone();

            chain1.add(new Transaction("D"));

            Console.WriteLine(string.Format("Chain 1 Hash: {0}", chain1.getHead().getHash()));
            Console.WriteLine(string.Format("Chain 2 Hash: {0}", chain2.getHead().getHash()));
            Console.WriteLine(
            string.Format("Chains Are In Sync: {0}", chain1.getHead().getHash().Equals(chain2.getHead().getHash())));

            chain2.add(new Transaction("D"));

            Console.WriteLine(string.Format("Chain 1 Hash: {0}", chain1.getHead().getHash()));
            Console.WriteLine(string.Format("Chain 2 Hash: {0}", chain2.getHead().getHash()));
            Console.WriteLine(
                    string.Format("Chains Are In Sync: {0}", chain1.getHead().getHash().Equals(chain2.getHead().getHash())));

            Assert.IsTrue(chain1.blockChainHash().Equals(chain2.blockChainHash()));

            Console.WriteLine("Current Chain Head Transactions: ");
            foreach (Block<Transaction> block in chain1.chain)
            {
                block.getTransactions().ForEach( i => Console.WriteLine(i.toString()));
            }

            // Block Merkle root should equal root hash in Merkle Tree computed from block transactions
            Block<Transaction> headBlock = chain1.getHead();
            List<string> merkleTree = headBlock.merkleTree();
            Assert.IsTrue(headBlock.getMerkleRoot().Equals(merkleTree[merkleTree.Count - 1]));
        }

        [TestMethod]
        public void MerkleTreeTest()
        {
            // create chain, add transaction

            SimpleBlockchain<Transaction> chain1 = new SimpleBlockchain<Transaction>();

            chain1.add(new Transaction("A")).add(new Transaction("B")).add(new Transaction("C")).add(new Transaction("D"));

            // get a block in chain
            Block<Transaction> block = chain1.getHead();

            Console.WriteLine("Merkle Hash tree :");
            block.merkleTree().ForEach( i => Console.WriteLine(i + ","));

            // get a transaction from block 
            Transaction tx = block.getTransactions()[0];

            // see if hash is valid... using merkle Tree...
            block.isTransactionValid(tx);
            Assert.IsTrue(block.isTransactionValid(tx));

            // mutate the transaction data 
            tx.setValue("Z");

            Assert.IsFalse(block.isTransactionValid(tx));
        }

        [TestMethod]
        public void BlockMinerTest()
        {
            // create 30 transactions, that should result in 3 blocks in the chain.
            SimpleBlockchain<Transaction> chain = new SimpleBlockchain<Transaction>();

            // Respresents a proof of work miner
            // Creates 
            Miner<Transaction> miner = new Miner<Transaction>(chain);

            // This represents transactions being created by a network 
            for (int i = 0; i < 30; i++)
            {
                miner.mine(new Transaction("" + i));
            }

            Console.WriteLine("Number of Blocks Mined = " + chain.getChain().Count);
            Assert.IsTrue(chain.getChain().Count == 3);
        }
    }
}
