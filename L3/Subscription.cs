using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace L3
{
    public class Subscription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SportsmanId { get; set; }
        public string DataOfIssue { get; set; }

        public virtual Sportsman Sportsman { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
        public Subscription()
        {
            Activities = new List<Activity>();
        }
    }
}
