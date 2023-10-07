﻿using Application.Interfaces;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Case;
using Domain.Entities.CaseCriminal;
using Domain.Entities.CaseImage;
using Domain.Entities.CrimeReporting;
using Domain.Entities.Criminal;
using Domain.Entities.CriminalImage;
using Domain.Entities.Evidence;
using Domain.Entities.ReportingImage;
using Domain.Entities.User;
using Domain.Entities.WantedCriminal;
using Domain.Entities.Witness;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts
{
    public class ApplicationDbContext : AudtableContext
    {
        private readonly Application.Interfaces.ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService, IDateTimeService dateTimeService) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        private DbSet<AppUser> AppUsers { get; set; } = default!;
        private DbSet<AppRole> AppRoles { get; set; } = default!;
        private DbSet<AppRoleClaim> AppRoleClaims { get; set; } = default!;
        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<CaseCriminal> CaseCriminals { get; set;}
        public virtual DbSet<CaseImage> CaseImages { get; set; }
        public virtual DbSet<CrimeReporting> CrimeReportings { get; set; }
        public virtual DbSet<Criminal> Criminals { get; set; }
        public virtual DbSet<CriminalImage> CriminalImages { get; set; }
        public virtual DbSet<Evidence> Evidences { get; set; }
        public virtual DbSet<ReportingImage> ReportingImages { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<WantedCriminal> WantedCriminals { get; set; }
        public virtual DbSet<Witness> Witnesses { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = _dateTimeService.NowUtc;
                        entry.Entity.CreatedBy = string.IsNullOrEmpty(_currentUserService.UserName) ? "System" : _currentUserService.UserName;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdateAt = _dateTimeService.NowUtc;
                        entry.Entity.UpdateBy = entry.Entity.CreatedBy = string.IsNullOrEmpty(_currentUserService.UserName) ? "System" : _currentUserService.UserName;
                        break;
                }
            }

            return await base.SaveChangesAsync(_currentUserService.UserName, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(entity =>
            {
                entity.ToTable(name: "Users", "Identity");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            builder.Entity<AppRole>(entity =>
            {
                entity.ToTable(name: "Roles", "Identity");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles", "Identity");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims", "Identity");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins", "Identity");
            });

            builder.Entity<AppRoleClaim>(entity =>
            {
                entity.ToTable(name: "RoleClaims", "Identity");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens", "Identity");
            });
        }
    }
}