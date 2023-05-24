using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public Activity()
        {
            Subscriptions = new List<Subscription>();
        }
    }
}
