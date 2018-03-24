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
        //public override Task<CustomerDto> Create(CustomerDto dto, CancellationToken cancellationToken)
        //{
        //    SingleResult<CustomerDto> customerDto = SingleResult.Create(DtoEntityMapper.FromEntityQueryToDtoQuery((await Repository.GetAllAsync(cancellationToken)))
        //    .Where(cu => cu.NationalCode == 1));
        //    if (customerDto != null)
        //        throw new Exception("قبلا با این کد ملی ثبت نام کرده اید. لطفا وارد شوید.");
        //    dto.OTP = GenerateRandomNo();
        //    dto.IsActive = false;
        //    SendOtpSms("0" + dto.Mobile, "کد ورودی شما در سناپ : " + dto.OTP);
        //    //return await Repository.AddAsync(customer, cancellation);

        //    return base.Create(dto, cancellationToken);
        //}

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

        //[Action]
        //public virtual async Task<Customer> LoginCustomer(Customer customer, CancellationToken cancellation)
        //{
        //    Customer existingCustomer = (await Repository.GetAllAsync(cancellation))
        //        .Where(cu => cu.NationalCode == customer.NationalCode && cu.Mobile == customer.Mobile).FirstOrDefault();
        //    if (existingCustomer == null)
        //        throw new ResourceNotFoundException("کاربری با این مشخصات یافت نشد. لطفا با دقت اطلاعات را وارد نمائید.");
        //    return existingCustomer;
        //}

        [Action]
        public virtual async Task<SingleResult<CustomerDto>> LoginCustomer(Customer customer, CancellationToken cancellationToken)
        {
            SingleResult<CustomerDto> customerDto = SingleResult.Create(DtoEntityMapper.FromEntityQueryToDtoQuery((await Repository.GetAllAsync(cancellationToken)))
            .Where(cu => cu.NationalCode == customer.NationalCode && cu.Mobile == customer.Mobile));
            if (customerDto == null)
                throw new ResourceNotFoundException("کاربری با این مشخصات یافت نشد. لطفا با دقت اطلاعات را وارد نمائید.");
            return customerDto;
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


        private void SendOtpSms(string mobile, string content)
        {

        }

        private int GenerateRandomNo()
        {
            int min = 1000, max = 9999; Random random = new Random(); return random.Next(min, max);
        }
    }
}