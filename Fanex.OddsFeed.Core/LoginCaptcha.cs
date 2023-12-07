namespace Fanex.OddsFeed.Admin.Utility
{
    using System;
    using System.Text;

    public static class LoginCaptcha
    {
        public static string GetCaptchaString(int length)
        {
            const int intZero = '0';
            const int intNine = '9';
            var intCount = 0;

            var captcha = new StringBuilder();
            var random = new Random(DateTime.Now.Millisecond);

            while (intCount < length)
            {
                var intRandomNumber = random.Next(intZero, intNine + 1);
                if ((intRandomNumber >= intZero) && (intRandomNumber <= intNine))
                {
                    captcha.Append((char)intRandomNumber);
                    intCount += 1;
                }
            }

            return captcha.ToString();
        }
    }
}
