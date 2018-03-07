using Bit.OData.ODataControllers;
using Bit.Owin.Exceptions;
using Sanaap.Dto;
using Sanaap.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sanaap.Api.Controllers
{
    public class CustomersController : SanaapDtoSetController<CustomerDto, Customer>
    {
        [Action]
        public virtual async Task<Customer> AddCustomer(Customer customer, CancellationToken cancellation)
        {
            Customer existingCustomer = (await Repository.GetAllAsync(cancellation)).Where(cu => cu.NationalCode == customer.NationalCode).FirstOrDefault();
            if (existingCustomer != null)
                throw new Exception("قبلا با این کد ملی ثبت نام کرده اید. لطفا وارد شوید.");
            customer.OTP = GenerateRandomNo();
            customer.IsActive = false;
            SendOtpSms("0" + customer.Mobile, "کد ورودی شما در سناپ : " + customer.OTP);
            return await Repository.AddAsync(customer, cancellation);
        }

        [Function]
        public virtual async Task<bool> ConfirmOTP(Guid customerId, int otp, CancellationToken cancellation)
        {
            Customer existingCustomer = (await Repository.GetAllAsync(cancellation)).Where(cu => cu.Id == customerId).FirstOrDefault();
            if (existingCustomer.OTP == otp)
            {
                existingCustomer.IsActive = true;
                await Repository.UpdateAsync(existingCustomer, cancellation);
                return true;
            }
            else return false;
        }

        [Function]
        public virtual async Task<SingleResult<CustomerDto>> LoginCustomer(long nationalCode, long mobile, CancellationToken cancellationToken)
        {
            SingleResult<CustomerDto> customerDto = SingleResult.Create(DtoEntityMapper.FromEntityQueryToDtoQuery((await Repository.GetAllAsync(cancellationToken)))
            .Where(cu => cu.NationalCode == nationalCode && cu.Mobile == mobile && cu.IsActive == true));
            if (customerDto == null)
                throw new ResourceNotFoundException("کاربری با این مشخصات یافت نشد. لطفا با دقت اطلاعات را وارد نمائید.");
            return customerDto;
        }

        private void SendOtpSms(string mobile, string content)
        {

        }

        private int GenerateRandomNo()
        {
            int min = 1000, max = 9999; Random random = new Random(); return random.Next(min, max);
        }
    }
}