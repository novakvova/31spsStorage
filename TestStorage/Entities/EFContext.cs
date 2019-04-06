using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStorage.Entities
{
    public class EFContext : DbContext
    {
        public EFContext() : base("DefaultConnection")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
    }
}
