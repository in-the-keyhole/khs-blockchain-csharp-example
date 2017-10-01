using System;

namespace BlockChain._01.SimpleChain
{ 
    class Program
    {
        static void Main(string[] args)
        {
            var chain1 = new SimpleBlockchain<string>();
            var chain2 = new SimpleBlockchain<string>();            

            chain1.Add("A")
                  .Add("B")
                  .Add("C")
                  .Add("D");

            chain2.Add("A")
                  .Add("B")
                  .Add("D")
                  .Add("C");
            
            Console.WriteLine($"Chain 1 Hash: {chain1.BlockchainHash}");
            Console.WriteLine($"Chain 2 Hash: {chain2.BlockchainHash}");
            Console.WriteLine($"Chains Are In Sync: {chain1.BlockchainHash == chain2.BlockchainHash} \n\n");


            bool foundLastGoodHash = false;
            int checkIndex = chain2.Count;
            
            //Synchronize the blockchain order

            while(!foundLastGoodHash)
            {
                checkIndex--;
                foundLastGoodHash = chain1[checkIndex].GetHash() == chain2[checkIndex].GetHash();                
            }

            chain2.DeleteAfterIndex(checkIndex);

            for (int i = checkIndex + 1; i < chain1.Count; i++)
            {
                chain2.Add(chain1[i].Data);
            }

            Console.WriteLine($"Chain 1 Hash: {chain1.BlockchainHash}");
            Console.WriteLine($"Chain 2 Hash: {chain2.BlockchainHash}");
            Console.WriteLine($"Chains Are In Sync: {chain1.BlockchainHash == chain2.BlockchainHash}");

            Console.ReadKey();
        }
    }
}
