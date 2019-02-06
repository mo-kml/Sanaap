using Prism.Services;
using Sanaap.App.Helpers.Contracts;
using Sanaap.App.ItemSources;
using Sanaap.Constants;
using System;

namespace Sanaap.App.Helpers.Implementations
{
    public class LicenseHelper : ILicenseHelper
    {
        private readonly IPageDialogService _dialogService;
        public LicenseHelper(IPageDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public LicensePlateItemSource ConvertToItemSource(string licensePlate)
        {
            if (string.IsNullOrEmpty(licensePlate))
            {
                throw new ArgumentNullException(nameof(licensePlate));
            }

            string[] splitedPlateNumber = licensePlate.Split('|');

            if (splitedPlateNumber.Length == 4)
            {
                return new LicensePlateItemSource
                {
                    FirstNumber = splitedPlateNumber[0],
                    Alphabet = splitedPlateNumber[1],
                    SecondNumber = splitedPlateNumber[2],
                    ProvinceNumber = splitedPlateNumber[3]
                };
            }
            else
            {
                throw new ArgumentException($"{nameof(licensePlate)} is not correct");
            }
        }

        public bool ConvertToPlateNumber(LicensePlateItemSource licensePlateItemSource, out string plateNumber)
        {
            if (licensePlateItemSource == null)
            {
                throw new ArgumentNullException(nameof(licensePlateItemSource));
            }

            if (licensePlateItemSource.FirstNumber.Length != 2 || string.IsNullOrEmpty(licensePlateItemSource.Alphabet) || licensePlateItemSource.SecondNumber.Length != 3 || licensePlateItemSource.ProvinceNumber.Length != 2)
            {
                _dialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.NumberPlateIsNotValid, ConstantStrings.Ok);

                plateNumber = string.Empty;

                return false;
            }

            plateNumber = licensePlateItemSource.FirstNumber + "|" + licensePlateItemSource.Alphabet + "|" + licensePlateItemSource.SecondNumber + "|" + licensePlateItemSource.ProvinceNumber;

            return true;
        }
    }
}
