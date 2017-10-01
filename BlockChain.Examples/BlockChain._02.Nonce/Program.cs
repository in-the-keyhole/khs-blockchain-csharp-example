using System;
using System.Diagnostics;

namespace BlockChain._02.Nonce
{
    class Program
    {
        static void Main(string[] args)
        {
            var message = "Keyhole Software";

            Console.WriteLine($"Message: {message}");

            var hashValue = message.ToSHA512Hash();

            Console.WriteLine($"Hash: {hashValue}\n");

            var nonceKey = "12345";
            long nonce = 0;
            var nonceFound = false;
            string nonceHash = string.Empty;

            var stopWatch = Stopwatch.StartNew();

            while (!nonceFound)
            {

                nonceHash = $"{message}{nonce}".ToSHA512Hash();
                nonceFound = nonceHash.Substring(0, nonceKey.Length) == nonceKey;

                nonce++;
            }

            stopWatch.Stop();

            Console.WriteLine($"Nonce: {nonce:N0}");
            Console.WriteLine($"Nonce Hash: {nonceHash}");
            Console.WriteLine($"Nonce Search Time: {stopWatch.ElapsedMilliseconds:N0} ms");

            Console.ReadLine();
        }
    }
}
