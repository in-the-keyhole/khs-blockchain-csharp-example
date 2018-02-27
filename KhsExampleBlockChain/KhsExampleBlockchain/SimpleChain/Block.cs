using KhsExampleBlockchain.SimpleChain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace KhsExampleBlockchain.SimpleChain
{
    public class Block<T> where T : Tx
    {
        public long timeStamp;
        private int index;
        private List<T> transactions = new List<T>();
        private string hash;
        private string previousHash;
        private string merkleRoot;
        private string nonce = "0000";

        // caches Transaction SHA256 hashes
        public Dictionary<string, T> map = new Dictionary<string, T>();

        public Block<T> add(T tx)
        {
            transactions.Add(tx);
            map[tx.getHash()] = tx;
            computeMerkleRoot();
            computeHash();
            return this;
        }


        public void computeMerkleRoot()
        {
            List<String> treeList = merkleTree();
            // Last element is the merkle root hash if transactions
            setMerkleRoot(treeList.Last());
        }


        public Block<T> Clone()
        {
            // Object serialized then rehydrated into a new instance of an object so
            // memory conflicts don't happen
            // There are more efficent ways but this is the most reaadable
            Block<T> clone = new Block<T>();
            clone.setIndex(this.getIndex());
            clone.setPreviousHash(this.getPreviousHash());
            clone.setMerkleRoot(this.getMerkleRoot());
            clone.setTimeStamp(this.getTimeStamp());
            //clone.setTransactions(this.getTransactions());

            List<T> clonedtx = new List<T>();
            Action<T> consumer = (t) => clonedtx.Add(t);
            this.getTransactions().ForEach(consumer);
            clone.setTransactions(clonedtx);

            return clone;
        }

        public bool isTransactionValid(Transaction tx)
        {
            try
            {
                T hash = map[tx.getHash()];
            } catch(KeyNotFoundException e)
            {
                Console.WriteLine("No such Key exists: " + tx.getHash());
                Console.WriteLine("For Transaction: " + tx.getValue());
                hash = null;
            }
            return hash != null;
        }


        /*
            This method was adapted from the https://github.com/bitcoinj/bitcoinj project

            The Merkle root is based on a tree of hashes calculated from the
            transactions:

                 root
                 / \
                 A B
                 / \ / \
               t1 t2 t3 t4

            The tree is represented as a list: t1,t2,t3,t4,A,B,root where each
            entry is a hash.

            The hashing algorithm is SHA-256. The leaves are a hash of the
            serialized contents of the transaction.
            The interior nodes are hashes of the concatenation of the two child
            hashes.

            This structure allows the creation of proof that a transaction was
            included into a block without having to
            provide the full block contents. Instead, you can provide only a
            Merkle branch. For example to prove tx2 was
            in a block you can just provide tx2, the hash(tx1) and A. Now the
            other party has everything they need to
            derive the root, which can be checked against the block header. These
            proofs aren't used right now but
            will be helpful later when we want to download partial block
            contents.

            Note that if the number of transactions is not even the last tx is
            repeated to make it so (see
            tx3 above). A tree with 5 transactions would look like this:

                root
                / \
                1 5
               / \ / \
               2 3 4 4
               / \ / \ / \
              t1 t2 t3 t4 t5 t5

        */
        public List<string> merkleTree()
        {
            List<string> tree = new List<string>();
            // Start by adding all the hashes of the transactions as leaves of the
            // tree.
            foreach (T t in transactions)
            {
                tree.Add(t.getHash());
            }
            int levelOffset = 0; // Offset in the list where the currently processed
                                 // level starts.
                                 // Step through each level, stopping when we reach the root (levelSize
                                 // == 1).
            for (int levelSize = transactions.Count(); levelSize > 1; levelSize = (levelSize + 1) / 2)
            {
                // For each pair of nodes on that level:
                for (int left = 0; left < levelSize; left += 2)
                {
                    // The right hand node can be the same as the left hand, in the
                    // case where we don't have enough
                    // transactions.
                    int right = Math.Min(left + 1, levelSize - 1);
                    string tleft = tree.ElementAt(levelOffset + left);
                    string tright = tree.ElementAt(levelOffset + right);
                    tree.Add(Helpers.SHA256.generateHash(tleft + tright));
                }
                // Move to the next level.
                levelOffset += levelSize;
            }
            return tree;
        }

        public void computeHash()
        {
            string serializedData = JsonConvert.SerializeObject(transactions);
            setHash(Helpers.SHA256.generateHash(timeStamp + index + merkleRoot + serializedData + nonce + previousHash));
        }

        public String getHash()
        {

            // calc hash if not defined, just for testing...
            if (hash == null)
            {
                computeHash();
            }

            return hash;
        }

        public void setHash(String h)
        {
            this.hash = h;
        }

        public long getTimeStamp()
        {
            return timeStamp;
        }

        public void setTimeStamp(long timeStamp)
        {
            this.timeStamp = timeStamp;
        }

        public int getIndex()
        {
            return index;
        }

        public void setIndex(int index)
        {
            this.index = index;
        }

        public String getPreviousHash()
        {
            return previousHash;
        }

        public void setPreviousHash(String previousHash)
        {
            this.previousHash = previousHash;
        }

        public List<T> getTransactions()
        {
            return transactions;
        }

        public void setTransactions(List<T> transactions)
        {
            this.transactions = transactions;
        }

        public String getMerkleRoot()
        {
            return merkleRoot;
        }

        public void setMerkleRoot(String merkleRoot)
        {
            this.merkleRoot = merkleRoot;
        }

        public String getNonce()
        {
            return nonce;
        }

        public void setNonce(String nonce)
        {
            this.nonce = nonce;
        }

    }
}
