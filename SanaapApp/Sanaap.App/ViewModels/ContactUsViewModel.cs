using Bit.ViewModel;
using Xamarin.Forms;

namespace Sanaap.App.ViewModels
{
    public class ContactUsViewModel : BitViewModelBase
    {
        public BitDelegateCommand WhatsApp { get; set; }

        public BitDelegateCommand Call { get; set; }

        public BitDelegateCommand Telegram { get; set; }

        public BitDelegateCommand Instagram { get; set; }

        public BitDelegateCommand Route { get; set; }

        public ContactUsViewModel()
        {
            WhatsApp = new BitDelegateCommand(async () =>
              {
                  Device.OpenUri(new System.Uri("https://api.whatsapp.com/send?phone=989036193862"));
              });

            Telegram = new BitDelegateCommand(async () =>
            {
                Device.OpenUri(new System.Uri("https://t.me/mo-kml"));
            });

            Instagram = new BitDelegateCommand(async () =>
            {
                Device.OpenUri(new System.Uri("https://www.instagram.com/mo_kml/"));
            });

            Call = new BitDelegateCommand(async () =>
             {
                 Device.OpenUri(new System.Uri("tel:038773729"));
             });

            Route = new BitDelegateCommand(async () =>
             {
                 Device.OpenUri(new System.Uri("geo:35.71022,51.4939886"));
             });

        }
    }
}
