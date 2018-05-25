using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Service.Contracts;
using System;

namespace Sanaap.App.ViewModels
{
    public class MasterDetailViewModel : BitViewModelBase
    {
        public MasterDetailViewModel(INavigationService navigationService, ISecurityService securityService,
        ILoginValidator loginValidator, IPageDialogService pageDialogService)
        {

        }
    }
}