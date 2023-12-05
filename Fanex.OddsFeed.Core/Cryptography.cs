namespace Fanex.OddsFeed.Core
{
    using Microsoft.VisualBasic;

    public static class Cryptography1
    {
        public static string CfsEnCode(string codeStr)
        {
            int num = 30;
            int num2 = num - codeStr.Length;
            if (num2 > 1)
            {
                for (int i = 1; i <= num2; i++)
                {
                    codeStr += Strings.Chr(21);
                }
            }

            double num3 = 1.0;
            for (int j = 1; j <= num; j++)
            {
                int num4 = num + Strings.Asc(Strings.Mid(codeStr, j, 1)) * j;
                num3 *= (double)num4;
            }

            codeStr = num3.ToString("G15");
            string text = string.Empty;
            for (int k = 1; k <= codeStr.Length; k++)
            {
                text += CfsCode(Strings.Mid(codeStr, k, 3));
            }

            string text2 = string.Empty;
            for (int l = 20; l <= text.ToString().Length - 18; l += 2)
            {
                text2 += Strings.Mid(text, l, 1);
            }

            return text2;
        }

        private static string CfsCode(string value)
        {
            string text = string.Empty;
            for (int i = 1; i <= value.Length; i++)
            {
                text += Strings.Asc(Strings.Mid(value, i, 1));
            }

            return Conversion.Hex((object)text);
        }
    }
}
