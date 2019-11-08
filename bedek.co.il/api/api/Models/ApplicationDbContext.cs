using Api.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using Api.Models;
using api.Enums;
using api.Dtos;
using System.Data.Common;

namespace api.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option)
            : base(option) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ------Default values:

            modelBuilder.Entity<User>()
                .Property(p => p.DateRegistered)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<ApartmentDoc>()
                .Property(p => p.DateUploaded)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Service>()
                .Property(p => p.IsEnable)
                .HasDefaultValue(true);

            modelBuilder.Entity<User>()
                .Property(x => x.IsDeleted)
                .HasDefaultValue(false);






            //-----Primary Key:

            // UserInRole: set both fields as primary
            modelBuilder.Entity<UserInRole>().HasKey(table => new
            {
                table.RoleId,
                table.UserId
            });



            // ServiceInUser: set both fields as primary
            modelBuilder.Entity<ServiceInHandyman>().HasKey(table => new
            {
                table.ServiceId,
                table.UserId
            });

            modelBuilder.Entity<ServiceInHandymanInBuilding>().HasKey(table => new
            {
                table.BuildingId,
                table.UserId,
                table.ServiceId
            });

            modelBuilder.Entity<ServiceInHandymanInBuildingInServiceCall> ().HasKey(table => new
            {
                table.ServiceCallId,
                table.ServiceInHandymanInBuildingId              
            });


            // ----------- Isert default data:
            // Insert Administrator & Lead Role to Role table
            modelBuilder.Entity<Role>().HasData(new Role() { RoleId = 1, RoleName = Enum.GetName(typeof(UserRoles), 1) }); // Admin
            modelBuilder.Entity<Role>().HasData(new Role() { RoleId = 2, RoleName = Enum.GetName(typeof(UserRoles), 2) }); // Handyman
            modelBuilder.Entity<Role>().HasData(new Role() { RoleId = 3, RoleName = Enum.GetName(typeof(UserRoles), 3) }); // Resident

            var salt = "";

            // Insert Admin User Eyal
            var firstAdminUserId = Guid.NewGuid();                                 
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    UserId = firstAdminUserId,
                    FirstName = "Eyal",
                    LastName = "Ankri",
                    Email = "eyal.ank@gmail.com",
                    Password = Encryption.Sha256($"5224287ea{salt}"),
                    Phone1 = "054-6680240",
                    IsAcceptEmails = false,
                    DateRegistered = DateTime.Now,   
                    IdentityCardId = "033913450"

                });


            // Insert to Eyal Ankri user
            modelBuilder.Entity<UserInRole>().HasData(
                new UserInRole()
                {
                    RoleId = (int)UserRoles.Administrator,
                    UserId = firstAdminUserId
                });


            // Insert Admin User Carmel
            firstAdminUserId = Guid.NewGuid();                        
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    UserId = firstAdminUserId,
                    FirstName = "Carmel",
                    LastName = "Malca",
                    Email = "carmelm@maozdaniel.co.il",
                    Password = Encryption.Sha256($"Carmel2620{salt}"),
                    Phone1 = "054-2446997",
                    IsAcceptEmails = false,
                    DateRegistered = DateTime.Now,
                    IdentityCardId = "000000000"

                });


            // Insert to Eyal Ankri user
            modelBuilder.Entity<UserInRole>().HasData(
                new UserInRole()
                {
                    RoleId = (int)UserRoles.Administrator,
                    UserId = firstAdminUserId
                });


            modelBuilder.Entity<Service>().HasData(
                new Service()
                {
                    ServiceId = 1,
                    ServiceName = "אינסטלציה",
                    WarrantyPeriodInMonths = 60, // 5 years...
                    IsEnable = true
                });




            //DbCommand cmd = Database.Connection.CreateCommand();
            //cmd.CommandText = "create stored procedure ....";
            //cmd.ExecuteNonQuery();

        }


        public DbSet<User> Users { get; set; }
        public DbSet<UserInRole> UserInRoles { get; set; }
        public DbSet<User> Roles { get; set; }
        public DbSet<Building> Buildings { get; set; }    
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<ApartmentDoc> ApartmentDocs { get; set; }     
        public DbSet<Service> Service { get; set; }
        public DbSet<ServiceInHandyman> ServiceInUser { get; set; }
        public DbSet<ServiceInHandymanInBuilding> ServiceInHandymanInBuilding { get; set; }
        public DbSet<ServiceCall> ServiceCall { get; set; }

    }
}
