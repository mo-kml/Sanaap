﻿using Sanaap.Dto;
using Sanaap.Service.Contracts;
using System.Collections.Generic;

namespace Sanaap.Service.Implementations
{
    public class SanaapAppTranslateService : ISanaapAppTranslateService
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
            { "InvalidUserNameOrPassword" , "نام کاربری یا رمز عبور اشتباه است" },
            { "CustomerCouldNotBeFound" , "کاربری با این مشخصات یافت نشد" },
            { "CustomerIsAlreadyRegistered" , "این کاربر قبلا ثبت نام شده است" },
            { $"{nameof(CustomerDto.NationalCode)}IsEmpty" , "کد ملی خالی است" },
            { $"{nameof(CustomerDto.NationalCode)}IsInvalid" , "کد ملی اشتباه است" },
            { $"{nameof(CustomerDto.FirstName)}IsEmpty" , "نام خالی است" },
            { $"{nameof(CustomerDto.FirstName)}MustAtLeast2Characters" , "نام باید حداقل دو حرف باشد" },
            { $"{nameof(CustomerDto.LastName)}IsEmpty", "نام خانوادگی خالی است" },
            { $"{nameof(CustomerDto.LastName)}MustAtLeast2Characters" , "نام خانوادگی باید حداقل دو حرف باشد" },
            { $"{nameof(CustomerDto.Mobile)}IsEmpty", "شماره تلفن خالی است" },
            { $"{nameof(CustomerDto.Mobile)}IsInvalid" , "شماره تلفن اشتباه است. مثال درست : 09121234567" },
            { $"{nameof(InsurancePolicyDto.ChasisNo)}IsEmpty" , "شماره شاسی خالی است" },
            { $"{nameof(InsurancePolicyDto.InsurerNo)}IsEmpty" , "لطفا شماره بیمه نامه را وارد کنید" },
            { $"{nameof(InsurancePolicyDto.PlateNumber)}IsEmpty" , "لطفا شماره پلاک را وارد کنید" },
            { $"{nameof(InsurancePolicyDto.VIN)}IsEmpty" , "لطفا شماره VIN را وارد کنید" },
            { $"{nameof(EvlRequestDto.AccidentReason)}IsEmpty" , "لطفا علت حادثه را وارد کنید" },
            { $"{nameof(EvlRequestDto.OwnerFirstName)}IsEmpty" , "نام بیمه گذار خالی میباشد" },
            { $"{nameof(EvlRequestDto.OwnerLastName)}IsEmpty", "نام خانوادگی بیمه گذار خالی میباشد" },
            { $"{nameof(EvlRequestDto.AccidentDate)}IsEmpty" , "لطفا تاریخ بروز حادثه را وارد کنید" },
            { $"{nameof(EvlRequestDto.LostFirstName)}IsEmpty" , "لطفا نام زیان دیده را وارد کنید" },
            { $"{nameof(EvlRequestDto.LostLastName)}IsEmpty" , "لطفا نام خانوادگی زیان دیده را وارد کنید" },
            { $"{nameof(EvlRequestDto.LostPlateNumber)}IsEmpty" , "لطفا شماره پلاک زیان دیده را وارد کنید" },
            { $"{nameof(EvlRequestDto.LostCarId)}IsEmpty" , "لطفا خودرو زیان دیده را انتخاب کنید" },
            { $"{nameof(CommentDto.Description)}IsEmpty" , "لطفا توضیحات پیام را وارد نمایید" },

    };
    }
}
