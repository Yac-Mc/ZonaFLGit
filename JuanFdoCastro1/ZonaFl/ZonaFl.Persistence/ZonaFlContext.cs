using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZonaFl.Persistence.Entities;
using ZonaFl.Migrations;
namespace ZonaFl.Persistence
{
   public  class ZonaFlContext: DbContext
    {

      
        public ZonaFlContext(): base("DefaultConnection") 
    {
            
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ZonaFlContext, MyObjextContextMigration>());

            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ZonaFlContext, Configuration>());
            //Database.SetInitializer<ZonaFlContext>(new CreateDatabaseIfNotExists<ZonaFlContext>());

            // Database.SetInitializer<ZonaFlContext>(new DropCreateDatabaseIfModelChanges<ZonaFlContext>());


            //Database.SetInitializer<ZonaFlContext>(new DropCreateDatabaseAlways<ZonaFlContext>());
            //Database.SetInitializer<ZonaFlContext>(new SchoolDBInitializer());
        }


        public DbSet<Category> Categorias { get; set; }
       public DbSet<Skill> Skills { get; set; }
       //public DbSet<SubCategory> SubCategory { get; set; }
        //public DbSet<UserSkills> UserSkills { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{

        //    modelBuilder.Entity<Category>()
        //        .Property(s => s.Id).IsRequired();
        //    modelBuilder.Entity<Category>()
        //         .Property(s => s.Name).IsRequired();
        //    modelBuilder.Entity<Category>().HasMany(s => s.SubCategories);




        //    modelBuilder.Entity<Skill>()
        //        .Property(s => s.Id).IsRequired();
        //    modelBuilder.Entity<Skill>()
        //       .Property(s => s.Name).IsRequired();
        //    modelBuilder.Entity<Skill>()
        //        .HasRequired<Category>(s => s.Category);


        //}

    }
}
