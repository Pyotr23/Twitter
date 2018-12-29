using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;

namespace Diplom
{
    class DiplomContext : DbContext
    {
        public DiplomContext()
            : base(@"Data Source=N55S\SQLEXPRESS;Initial Catalog=Diplom.DiplomContext;Integrated Security=True")
        {

        }

        public DbSet<Tweet> Tweets { get; set; }
        
        public DbSet<Hashtag> Hashtags { get; set; }  
        
        public DbSet<User> Users { get; set; }
    }
}
