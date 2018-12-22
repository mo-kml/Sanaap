using System;

namespace Sanaap.Api.Implementations
{
    public class VerificationCode
    {
        public static string SendVerifyCode(string mobileNumber)
        {
            string code = "";

            Random rand = new Random();

            for (int i = 0; i < 5; i++)
            {
                code += rand.Next(0, 9).ToString();
            }

            return code;
        }
    }
}
