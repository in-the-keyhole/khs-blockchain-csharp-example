using System;
using KhsExampleBlockchain.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestkhsExampleBlockchainCsharp.Nonce
{
    [TestClass]
    public class NonceTest
    {
        [TestMethod]
        public void NonceTestMethod()
        {
            string message = "Keyhole Software";

            Console.WriteLine("Message: " + message);

            string hashValue = SHA256.generateHash(message);

            Console.WriteLine(string.Format("Hash: {0}", hashValue));

            string nonceKey = "12345";
            long nonce = 0;
            bool nonceFound = false;
            string nonceHash = "";

            long start = TimeUtils.CurrentTimeMillis();

            while (!nonceFound)
            {

                nonceHash = SHA256.generateHash(message + nonce);
                nonceFound = nonceHash.Substring(0, nonceKey.Length).Equals(nonceKey);
                nonce++;

            }

            long ms = TimeUtils.CurrentTimeMillis() - start;

            Console.WriteLine(string.Format("Nonce: {0} ", ms));
            Console.WriteLine(string.Format("Nonce Hash: {0}", nonceHash));
            Console.WriteLine(string.Format("Nonce Search Time: {0} ms", ms));

            Assert.IsTrue(nonceFound);
        }
    }
}
