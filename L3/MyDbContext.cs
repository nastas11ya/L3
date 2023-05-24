using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace L3
{
    public class MyDbContext: DbContext
    {
        public MyDbContext() : base("DbConnectionString")
        { }

        //укажем все наборы таблиц, которые будут использоваться в Entity Framework
        public DbSet<Sportsman> Sportsmen { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}
