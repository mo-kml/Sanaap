using System;

namespace Sanaap.App.ItemSources
{
    public class EvlRequestListItemSource
    {
        public int Code { get; set; }

        public string RequestTypeName { get; set; }

        public string Date { get; set; }

        public Guid RequestId { get; set; }
    }
}
