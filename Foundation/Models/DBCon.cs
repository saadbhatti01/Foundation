using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Models
{
    public class DBCon : DbContext
    {
        public DBCon(DbContextOptions options) : base(options){ }

        public DbSet<Country> countries { get; set; }
        public DbSet<City> cities { get; set; }
        public DbSet<Location>  locations { get; set; }
        public DbSet<Person> peoples { get; set; }
        public DbSet<Login> logins { get; set; }
        public DbSet<Donation> donations { get; set; }
        public DbSet<Donation_Type> donation_Types { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<Tax> taxes { get; set; }
    }
}
