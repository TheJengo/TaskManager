using Core.Entity.Concrete;
using Entity.Concrete;
using Entity.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework.Contexts
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext()
        {
            SetOptions();
        }

        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options)
        {
            SetOptions();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // local
            optionsBuilder.UseSqlServer(@"Server =EXCALIBUR; database = TaskManagerDB; Trusted_Connection = True; MultipleActiveResultSets = True; Integrated Security = true");

            // test

            // live
        }

        public void SetOptions()
        {
            if (Database != null && !string.IsNullOrWhiteSpace(Database.ProviderName) && Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                Database.SetCommandTimeout(360);
                ChangeTracker.AutoDetectChangesEnabled = true;
                ChangeTracker.LazyLoadingEnabled = false;
                ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OperationClaim>()
                .HasMany(e => e.UserOperationClaims)
                .WithOne(e => e.OperationClaim)
                .IsRequired(false)
                .HasForeignKey(e => e.OperationClaimId);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserOperationClaims)
                .WithOne(e => e.User)
                .IsRequired()
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<UserOperationClaim>().HasKey(e => new { e.UserId, e.OperationClaimId });

            modelBuilder.Entity<TaskType>()
                 .Property(e => e.Name)
                 .HasConversion<string>();

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskType>().HasData(
               new TaskType
               {
                   Id = (int)TaskScheduleType.Daily,
                   Name = TaskScheduleType.Daily,
               },
               new TaskType
               {
                   Id = (int)TaskScheduleType.Weekly,
                   Name = TaskScheduleType.Weekly,
               },
               new TaskType
               {
                   Id = (int)TaskScheduleType.Monthly,
                   Name = TaskScheduleType.Monthly,
               });
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<OperationClaim> OperationClaims { get; set; }
        public virtual DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public virtual DbSet<UserTask> UserTasks { get; set; }
        public virtual DbSet<TaskType> TaskTypes { get; set; }
    }
}
