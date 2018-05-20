using Bit.ViewModel;
using Plugin.Geolocator.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Dto;
using Simple.OData.Client;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sanaap.App.ViewModels
{
    public class MyEvlRequestsViewModel : BitViewModelBase
    {
        private ObservableCollection<EvlRequestDto> items;

        public MyEvlRequestsViewModel(INavigationService navigationService, IGeolocator geolocator, IODataClient odataClient
            , IPageDialogService pageDialogService)
        {
            items = new ObservableCollection<EvlRequestDto>() {
            new EvlRequestDto()
            {
                Id = new Guid(),
                InsuranceTypeId = new Guid("73AAA9F9-EB55-E811-80D0-00155D0A0301"),
                Latitude = 35,
                Longitude = 51
            }
            //,
            //  new EvlRequestDto()
            //{
            //    Id = new Guid(),
            //    InsuranceTypeId = new Guid("73AAA9F9-EB55-E811-80D0-00155D0A0301"),
            //    Latitude = 35,
            //    Longitude = 51
            //},

            };
        }
    }
}