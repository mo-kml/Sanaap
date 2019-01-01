using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Sanaap.App.Services.Implementations
{
    public class InsurerService : IInsurerService
    {
        public InsurersItemSource[] GetAllInsurers()
        {
            return new List<InsurersItemSource>
            {
                new InsurersItemSource { Name = "آرمان", Id = 9, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "آسماری", Id = 27, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "آسیا", Id = 2, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "البرز", Id = 3, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "امید", Id = 23, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "ایران", Id = 25, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "ایران معین", Id = 36, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "بیمه گر خارجی", Id = 34, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "پارسیان", Id = 17, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "پارسیان ( یاری رسان )", Id = 37, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "پاسارگاد", Id = 18, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "تامین اجتماعی", Id = 32, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "تجارت نو", Id = 31, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "تعاون", Id = 26, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "حافظ", Id = 22, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "حکمت صبا", Id = 35, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "دانا", Id = 4, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "دی", Id = 16, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "رازی", Id = 13, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "سامان", Id = 6, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "سرمد", Id = 28, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "سلامت", Id = 33, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "سینا", Id = 5, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "شهر", Id = 21, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "کار آفرین", Id = 7, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "کوثر", Id = 10, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "ما", Id = 11, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "معلم", Id = 15, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "ملت", Id = 8, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "میهن", Id = 20, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
                new InsurersItemSource { Name = "نوین", Id = 12, Image = ImageSource.FromResource("Sanaap.App.Images.photo.jpg") },
            }.ToArray();
        }
    }
}
