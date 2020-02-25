using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Foundation.Helper
{
    public class Crypt
    {
        public static string GetMD5Hash(string text)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
                bytes = md5.ComputeHash(bytes);

                StringBuilder strBuilder = new StringBuilder();
                foreach (byte x in bytes)
                {
                    strBuilder.Append(x.ToString());
                }
                return strBuilder.ToString();
            }
        }
    }
}
