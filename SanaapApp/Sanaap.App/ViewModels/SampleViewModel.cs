using Bit.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sanaap.App.ViewModels
{
    public class SampleViewModel : BitViewModelBase
    {
        public SampleViewModel()
        {
            Items = new ObservableCollection<Test>(
                new List<Test>
                {
                    new Test{Text="fdgsdfgsgs"},
                    new Test{Text="fdgsdfgsgs"},
                    new Test{Text="fdgsdfgsgs"},
                    new Test{Text="fdgsdfgsgs"},
                }
                );
        }
        public ObservableCollection<Test> Items { get; set; }
    }
    public class Test
    {
        public string Text { get; set; }
    }
}
