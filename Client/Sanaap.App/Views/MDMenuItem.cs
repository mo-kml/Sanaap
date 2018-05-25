using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanaap.App.Views
{

    public class MDMenuItem
    {
        public MDMenuItem()
        {
            TargetType = typeof(MDDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}