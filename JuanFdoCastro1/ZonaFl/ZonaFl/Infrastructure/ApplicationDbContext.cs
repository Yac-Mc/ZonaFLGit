using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ZonaFl.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
           
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            //Configuration.AutomaticMigrationsEnabled = true;
            //Database.SetInitializer<ApplicationDbContext>(null);

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<ZonaFl.Models.Country> Countries { get; set; }
    }
}