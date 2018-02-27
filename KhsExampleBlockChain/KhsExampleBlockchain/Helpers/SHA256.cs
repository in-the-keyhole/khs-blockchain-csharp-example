using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KhsExampleBlockchain.Helpers
{
    public class SHA256
    {
        //Realize that this is probbly overkill since there is a SHA256 class in crypto already,
        //Just wanted to help align the Java project and this project so that concepts did not get
        //Muted and take a backseat to the implementation.
        //Please remember this is contrived example code to help explain concepts in the whitepaper.
        public static string generateHash(string value)
        {
            string hash = null;
            try
            {
                System.Security.Cryptography.SHA256 md = System.Security.Cryptography.SHA256.Create();
                byte[] bytes = md.ComputeHash(Encoding.UTF8.GetBytes(value));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
                hash = sb.ToString();
            }
            catch (CryptographicException e)
            {
                Console.Write(e.StackTrace);
            }
            return hash;
        }
    }
}
