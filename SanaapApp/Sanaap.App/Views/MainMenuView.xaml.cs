using Acr.UserDialogs;
using Sanaap.App.Controls;
using Sanaap.App.Helpers;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Sanaap.App.Views
{
    public partial class MainMenuView : ContentPage
    {
        private int count = 0;
        private int slideCount;
        private readonly IAppUtilities _utility;
        private IUserDialogs _userDialogs;
        public MainMenuView(IAppUtilities utility, IUserDialogs userDialogs)
        {
            _utility = utility;
            _userDialogs = userDialogs;
            InitializeComponent();


        }
        public void ToggleMenu(AbsoluteLayout menu)
        {
            if (menu.TranslationX == 0)
            {
                menu.FindByName<Button>("menuButton").IsVisible = false;

                menu.TranslateTo(DeviceDisplay.MainDisplayInfo.Width, 0, 350);
            }
            else
            {
                menu.TranslateTo(0, 0, 350);

                menu.FindByName<Button>("menuButton").IsVisible = true;
            }
        }

        public void ToggleMenuButton(object sender, EventArgs e)
        {
            AbsoluteLayout menu;

            if (sender is IconButton iconButton)
            {
                menu = (AbsoluteLayout)iconButton.BindingContext;
            }
            else if (sender is StackLayout stackLayout)
            {
                menu = (AbsoluteLayout)stackLayout.BindingContext;
            }
            else
            {
                menu = ((AbsoluteLayout)((Button)sender).Parent);
            }

            ToggleMenu(menu);
        }
        protected override bool OnBackButtonPressed()
        {
            count++;
            if (count == 1)
            {
                _userDialogs.Toast("برای بستن برنامه یک بار دیگر بازگشت را بزنید");
            }
            if (count == 2)
            {
                _utility.Exit();
                return false;
            }
            else
            {
                return true;
            }
        }

        protected override void OnAppearing()
        {
            Device.StartTimer(TimeSpan.FromSeconds(4), () =>
            {
                if (slideCount < 3)
                {
                    Carousel.Position = slideCount++;
                }
                else
                {
                    slideCount = 0;
                }
                return true;
            });

            count = 0;
        }
    }
}
