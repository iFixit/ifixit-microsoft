using System;
using System.Linq;
using System.Text;

namespace iFixit.Domain.Code
{
    public static class ExpensionMethods
    {
        public static string CleanCharacters(this string fileName)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in from char c in fileName where Char.IsLetter(c) || Char.IsNumber(c) select c)
            {
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
