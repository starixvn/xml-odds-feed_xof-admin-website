namespace Fanex.OddsFeed.Core
{
    using System.Text;
    using Microsoft.VisualBasic;

    public static class Cryptography
    {
        public static string CfsEnCode(string codeStr)
        {
            const int num = 30;
            const int number2 = 2;
            const int number18 = 18;
            const int chr21 = 21;
            const int length = 3;

            int num2 = num - codeStr.Length;
            if (num2 > 1)
            {
                for (int i = 1; i <= num2; i++)
                {
                    codeStr += Strings.Chr(chr21);
                }
            }

            double num3 = 1.0;
            for (int j = 1; j <= num; j++)
            {
                int num4 = num + Strings.Asc(Strings.Mid(codeStr, j, 1)) * j;
                num3 *= (double)num4;
            }

            codeStr = num3.ToString("G15");

            StringBuilder textBuilder = new StringBuilder();
            for (int k = 1; k <= codeStr.Length; k++)
            {
                textBuilder.Append(CfsCode(Strings.Mid(codeStr, k, length)));
            }
            string text = textBuilder.ToString();


            StringBuilder text2Builder = new StringBuilder();
            for (int l = 20; l <= text.Length - number18; l += number2)
            {
                text2Builder.Append(Strings.Mid(text, l, 1));
            }
            string text2 = text2Builder.ToString();

            return text2;
        }

        private static string CfsCode(string value)
        {
            StringBuilder textBuilder = new StringBuilder();
            for (int i = 1; i <= value.Length; i++)
            {
                textBuilder.Append(Strings.Asc(Strings.Mid(value, i, 1)));
            }
            string text = textBuilder.ToString();

            return Conversion.Hex((object)text);
        }
    }
}
