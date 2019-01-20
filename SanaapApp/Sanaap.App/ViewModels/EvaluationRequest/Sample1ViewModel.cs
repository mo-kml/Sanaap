using Bit.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class Sample1ViewModel : BitViewModelBase
    {
        public Sample1ViewModel()
        {
            Insurers.Add(new InsImage
            {
                Image = "https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png",
                Name = "Test1",
                IsSelected = true
            });

            Insurers.Add(new InsImage
            {
                Image = "https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png",
                Name = "Test1",
                IsSelected = true
            });
            Insurers.Add(new InsImage
            {
                Image = "https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png",
                Name = "Test1",
                IsSelected = true
            });
            Insurers.Add(new InsImage
            {
                Image = "https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png",
                Name = "Test1",
                IsSelected = true
            });
            Insurers.Add(new InsImage
            {
                Image = "https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png",
                Name = "Test1",
                IsSelected = true
            });
            Insurers.Add(new InsImage
            {
                Image = "https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png",
                Name = "Test1",
                IsSelected = true
            });

        }

        public ObservableCollection<InsImage> Insurers { get; set; } = new ObservableCollection<InsImage>();

    }

    public class InsImage
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
