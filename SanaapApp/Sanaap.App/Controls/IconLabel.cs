﻿using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Sanaap.App.Controls
{
    public class IconLabel : Label
    {
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "Type")
        {
            FontFamily = FontFamilies.FontFamily(Type);

            base.OnPropertyChanged(propertyName);
        }

        public FontAwesomeType Type
        {
            get => (FontAwesomeType)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static readonly BindableProperty TypeProperty =
            BindableProperty.Create("Type", typeof(FontAwesomeType), typeof(FontAwesomeType), FontAwesomeType.Regular);
    }
}
