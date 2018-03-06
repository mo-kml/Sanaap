using Bit.OData.ODataControllers;
using Bit.Owin.Exceptions;
using Sanaap.Dto;
using Sanaap.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sanaap.Api.Controllers
{
    public class CustomersController : SanaapDtoSetController<CustomerDto, Customer>
    {
        //public virtual ICustomersRepository CustomersRepository { get; set; }

        [Action]
        //public virtual async Task<Customer> AddCustomer(Customer customer, CancellationToken cancellation)
        //{
        //    Customer findedCustomer = (await Repository.GetAllAsync(cancellation)).Where(cu => cu.NationalCode == customer.NationalCode).FirstOrDefault();
        //    if (findedCustomer != null)
        //    {
        //        throw new Exception("قبلا ثبت نام کرده اید. لطفا وارد شوید.");
        //    }

        //    customer.OTP = GenerateRandomNo();
        //    customer.IsActive = false;
        //    SendOtpSms("0" + customer.Mobile, "کد ورودی شما در سناپ : " + findedCustomer.OTP);
        //    return await Repository.AddAsync(customer, cancellation);
        //}

        //[Function]
        //public virtual async Task<bool> ConfirmOTP(Customer customer, int otp, CancellationToken cancellation)
        //{
        //    Customer findedCustomer = (await Repository.GetAllAsync(cancellation))
        //        .Where(cu => cu.Id == customer.Id).FirstOrDefault();
        //    if (findedCustomer.OTP == otp)
        //    {
        //        return true;
        //    }

        //    else return false;
        //}

        [Function]
        public virtual async Task<Customer> LoginCustomer(long nationalCode, long mobile, CancellationToken cancellation)
        {
            Customer findedCustomer = await (await Repository.GetAllAsync(cancellation))
                .Where(cu => cu.NationalCode == nationalCode && cu.Mobile == mobile && cu.IsActive == true).FirstOrDefaultAsync(cancellation);
            if (findedCustomer == null)
                throw new ResourceNotFoundException("کاربری با این مشخصات یافت نشد. لطفا با دقت اطلاعات را وارد نمائید.");

            return findedCustomer;
        }

        //private void SendOtpSms(string mobile, string content)
        //{

        //}

        //private int GenerateRandomNo()
        //{
        //    int min = 1000;
        //    int max = 9999;
        //    Random random = new Random();
        //    return random.Next(min, max);
        //}
    }
}