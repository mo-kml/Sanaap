using Bit.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sanaap.App.ViewModels
{
    public class SampleViewModel : BitViewModelBase
    {
        public SampleViewModel()
        {
            Policies.Add(new Test
            {
                CarName = "پژو",
                ColorName = "صولتی",
                InsuranceTypeName = "بیمه ایران",
                InsImage = "https://img.game.co.uk/ml2/7/3/0/3/730331_scr3_a.png"
            });
        }
        public ObservableCollection<Test> Policies { get; set; } = new ObservableCollection<Test>();
    }
    public class Test
    {
        public string CarName { get; set; }
        public string ColorName { get; set; }
        public string InsuranceTypeName { get; set; }
        public string InsImage { get; set; }
    }
}
