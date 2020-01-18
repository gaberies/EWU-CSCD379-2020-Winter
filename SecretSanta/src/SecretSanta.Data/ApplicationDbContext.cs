﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<UserGroupRelationship> UserGroupRelationships { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroupRelationship>().HasKey(pt => new { pt.UserId, pt.GroupId });

            modelBuilder.Entity<UserGroupRelationship>()
                .HasOne(pt => pt.Group)
                .WithMany(p => p.Relationship)
                .HasForeignKey(pt => pt.GroupId);

            modelBuilder.Entity<UserGroupRelationship>()
                .HasOne(pt => pt.User)
                .WithMany(t => t.Relationship)
                .HasForeignKey(pt => pt.UserId);
        }

        public override int SaveChanges()
        {
            AddFingerPrinting();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            AddFingerPrinting();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddFingerPrinting()
        {
            var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
            var added = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);

            foreach (var entry in added)
            {
                var fingerPrintEntry = entry.Entity as FingerPrintEntityBase;
                if (fingerPrintEntry != null)
                {
                    fingerPrintEntry.CreatedOn = DateTime.UtcNow;
                    fingerPrintEntry.CreatedBy = HttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value ?? "";
                    fingerPrintEntry.ModifiedOn = DateTime.UtcNow;
                    fingerPrintEntry.ModifiedBy = HttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value ?? "";
                }
            }

            foreach (var entry in modified)
            {
                var fingerPrintEntry = entry.Entity as FingerPrintEntityBase;
                if (fingerPrintEntry != null)
                {
                    fingerPrintEntry.ModifiedOn = DateTime.UtcNow;
                    fingerPrintEntry.ModifiedBy = HttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value ?? "";
                }
            }
        }
    }
}
