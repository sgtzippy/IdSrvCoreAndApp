using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application1.Models
{
    public class SubscriptionViewModel
    {
        public IEnumerable<string> Names { get; private set; } = new List<string>();

        public SubscriptionViewModel(IEnumerable<string> names)
        {
            Names = names;
        }
    }
}
