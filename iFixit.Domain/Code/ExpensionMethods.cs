using System;
using System.Text;

namespace iFixit.Domain.Code
{
    public static class ExpensionMethods
    {
        public static string CleanCharacters(this string fileName)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in fileName)
            {
                if (Char.IsLetter(c) || Char.IsNumber(c))
                    sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
