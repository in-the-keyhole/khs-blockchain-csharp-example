using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestkhsExampleBlockchainCsharp.Helpers
{
    [TestClass]
    public class HashTest
    {
        [TestMethod]
        public void HashTestMethod()
        {
            String hash = KhsExampleBlockchain.Helpers.SHA256.generateHash("TEST String");
            Console.WriteLine(hash);
            Assert.IsTrue(hash.Length == 64);
        }
    }
}
