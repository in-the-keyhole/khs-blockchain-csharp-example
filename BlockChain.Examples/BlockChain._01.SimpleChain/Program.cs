using System;

namespace BlockChain._01.SimpleChain
{ 
    class Program
    {
        static void Main(string[] args)
        {
            var chain1 = new SimpleBlockchain<string>();
            
            chain1.Add("A")
                  .Add("B")
                  .Add("C");

            var chain2 = chain1.Clone();

            chain1.Add("D");
            chain2.Add("E");

            Console.WriteLine($"Chain 1 Hash: {chain1.BlockchainHash}");
            Console.WriteLine($"Chain 2 Hash: {chain2.BlockchainHash}");
            Console.WriteLine($"Chains Are In Sync: {chain1.BlockchainHash == chain2.BlockchainHash} \n\n");

            
            //Synchronize the blockchain order
            chain2 = chain1.Clone();
            chain2.Add("E");

            chain1 = chain2.Clone();

            Console.WriteLine($"Chain 1 Hash: {chain1.BlockchainHash}");
            Console.WriteLine($"Chain 2 Hash: {chain2.BlockchainHash}");
            Console.WriteLine($"Chains Are In Sync: {chain1.BlockchainHash == chain2.BlockchainHash} \n\n");

            Console.WriteLine("Current Chain: ");
            for (int i = 0; i < chain1.Count; i++)
            {
                Console.WriteLine(chain1[i].Data);
            }

            Console.ReadKey();
        }
    }
}
