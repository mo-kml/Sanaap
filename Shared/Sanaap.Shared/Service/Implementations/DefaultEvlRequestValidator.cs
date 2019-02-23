using Sanaap.Dto;
using Sanaap.Service.Contracts;
using System;

namespace Sanaap.Service.Implementations
{
    public class DefaultEvlRequestValidator : IEvlRequestValidator
    {
        public bool IsDetailValid(EvlRequestDto request, out string message)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrEmpty(request.OwnerFirstName))
            {
                message = $"{nameof(EvlRequestDto.OwnerFirstName)}IsEmpty";
                return false;
            }

            if (string.IsNullOrEmpty(request.OwnerLastName))
            {
                message = $"{nameof(EvlRequestDto.OwnerLastName)}IsEmpty";
                return false;
            }

            if (string.IsNullOrEmpty(request.InsurerNo))
            {
                message = $"{nameof(EvlRequestDto.InsurerNo)}IsEmpty";
                return false;
            }

            if (string.IsNullOrEmpty(request.PlateNumber))
            {
                message = $"{nameof(EvlRequestDto.PlateNumber)}IsEmpty";
                return false;
            }

            message = null;

            return true;
        }

        public bool IsLostDetailValid(EvlRequestDto request, out string message)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrEmpty(request.LostFirstName))
            {
                message = $"{nameof(EvlRequestDto.LostFirstName)}IsEmpty";
                return false;
            }

            if (string.IsNullOrEmpty(request.LostLastName))
            {
                message = $"{nameof(EvlRequestDto.LostLastName)}IsEmpty";
                return false;
            }

            if (string.IsNullOrEmpty(request.LostPlateNumber))
            {
                message = $"{nameof(EvlRequestDto.LostPlateNumber)}IsEmpty";
                return false;
            }

            if (request.LostCarId == 0)
            {
                message = $"{nameof(EvlRequestDto.LostCarId)}IsEmpty";
                return false;
            }

            message = null;

            return true;
        }
    }
}
