﻿using apiTest1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiTest1.Data
{
    public class apiDBContext : DbContext
    {

        public DbSet<UserRegisterModel> RegisterUser { get; set; }
        public DbSet<UserServicesModel> UserServices { get; set; }
        public DbSet<UserServiceDataModel> UserData { get; set; }
        public DbSet<DeviceSetupModel> SetupModels { get; set; }
        public DbSet<MasterKeys> FieldMasterKey { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ///  modelBuilder.Entity<UserRegisterModel>().HasOne<UserServicesModel>(c => c.Services).WithMany(g => g.Users).HasForeignKey(c => c.ApiKeyId);

            modelBuilder.Entity<UserServicesModel>().HasIndex(c => c.ServiceName).IsUnique(true);
            modelBuilder.Entity<UserRegisterModel>().HasIndex(c => c.ApiKeyId).IsUnique(true);
            modelBuilder.Entity<UserRegisterModel>().HasIndex(c => c.Email).IsUnique(true);

            modelBuilder.Entity<UserServicesModel>(table =>
            {

                table.HasOne(x => x.Users)
                .WithMany(x => x.services)
                .HasForeignKey(x => x.ApiKeyId)
                .HasPrincipalKey(x => x.ApiKeyId)//<<== here is core code to let foreign key userId point to User.Id.
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserServiceDataModel>(table =>
            {

                table.HasOne(x => x.Services)
                .WithMany(x => x.ServicesData)
                .HasForeignKey(x => x.DeviceId)
                .HasPrincipalKey(x => x.DeviceId)//<<== here is core code to let foreign key userId point to User.Id.
                .OnDelete(DeleteBehavior.Cascade);
            });

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=tcp:smartngdbserver.database.windows.net,1433;Initial Catalog=SmartNG_db;User Id=SmartNG@smartngdbserver;Password=temp1234");

        }
    }
}
