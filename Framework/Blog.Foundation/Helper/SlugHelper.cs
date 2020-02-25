using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Blog.Foundation.Helper
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string phrase)
        {
            string str = phrase.RemoveAccent().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // Remove is, an, the, 
            str = str.Replace(" is ", " ").Replace(" an ", " ").Replace(" a ", " ");
            if (str.Length > 70)
                str = str.Replace(" this ", " ").Replace(" and ", " ").Replace(" a ", " ");

            // cut and trim 
            str = str.Substring(0, str.Length <= 60 ? str.Length : 60).Trim();
            str = str + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}
