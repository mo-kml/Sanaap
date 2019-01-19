using Prism.Events;
using Sanaap.App.ItemSources;

namespace Sanaap.App.Events
{
    public class InsuranceEventArgs
    {
        public PolicyItemSource Policy { get; set; }
    }
    public class InsuranceEvent : PubSubEvent<InsuranceEventArgs>
    {
    }
}
