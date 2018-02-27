using KhsExampleBlockchain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace KhsExampleBlockchain.SimpleChain
{
    public class Transaction : Tx
    {


        private string hash;
        private string value;

        public string getHash() { return hash; }

        public Transaction(string value)
        {
            this.hash = SHA256.generateHash(value);
            this.setValue(value);
        }


        public string getValue()
        {
            return value;
        }
        public void setValue(string value)
        {

            // new value need to recalc hash
            this.hash = SHA256.generateHash(value);
            this.value = value;
        }

        public string toString()
        {
            return this.hash + " : " + this.getValue();
        }

    }
}
