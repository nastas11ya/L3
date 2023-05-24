using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3
{
    public class Sportsman
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
