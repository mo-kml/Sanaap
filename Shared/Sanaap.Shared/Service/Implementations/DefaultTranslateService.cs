using Sanaap.Dto;
using Sanaap.Service.Contracts;
using System.Collections.Generic;

namespace Sanaap.Service.Implementations
{
    public class DefaultTranslateService : ITranslateService
    {
        public virtual bool Translate(string str, out string translateResult)
        {
            return translateKeys.TryGetValue(str, out translateResult);
        }

        public virtual string Translate(string str)
        {
            return translateKeys[str];
        }

        private readonly IDictionary<string, string> translateKeys = new Dictionary<string, string>
        {
            { "InvalidUserNameOrPassword" , "نام کاربری یا رمز عبور به درستی وارد نشده است" },
            { "CustomerCouldNotBeFound" , "اطلاعات شما یافت نشد" },
            { "CustomerIsAlreadyRegistered" , "اطلاعات مد نظر قبلا ثبت شده است" },
            { $"{nameof(CustomerDto.NationalCode)}IsEmpty" , "کد ملی خالی است" },
            { $"{nameof(CustomerDto.NationalCode)}IsInvalid" , "کد ملی اشتباه است" },
            { $"{nameof(CustomerDto.FirstName)}IsEmpty" , "نام خالی است" },
            { $"{nameof(CustomerDto.FirstName)}MustAtLeast2Characters" , "نام باید حداقل دو کارکتر باشد" },
            { $"{nameof(CustomerDto.LastName)}IsEmpty", "نام خانوادگی خالی است" },
            { $"{nameof(CustomerDto.LastName)}MustAtLeast2Characters" , "نام خانوادگی باید لااقل دو کارکتر باشد" },
            { $"{nameof(CustomerDto.Mobile)}IsEmpty", "شماره تلفن خالی است" },
            { $"{nameof(CustomerDto.Mobile)}IsInvalid" , "شماره تلفن غلط است. مثال درست 09123456789" }
        };
    }
}
