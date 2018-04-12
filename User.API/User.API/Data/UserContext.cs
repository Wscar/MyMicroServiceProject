using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User.API.Models;
namespace User.API.Data
{
    public class UserContext:DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }
        protected override  void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<APPUser>()
                .ToTable("Users")
                .HasKey(u => u.Id);



            modelBuilder.Entity<UserProperty>().Property(u => u.Key).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>().Property(u => u.Value).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>()
                .ToTable("UserProperties")
                .HasKey(u => new { u.Key, u.APPUserId, u.Value });



            modelBuilder.Entity<UserTag>().Property(u => u.Tag).HasMaxLength(100);
            modelBuilder.Entity<UserTag>()
                 .ToTable("UserTags")
                 .HasKey(u => new { u.UserId, u.Tag });
         
            modelBuilder.Entity<BPFile>()
                .ToTable("BPFiles")
                .HasKey(b => b.Id);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<APPUser> Users { get; set; }
        public DbSet<UserProperty> UserProperties { get; set; }
        public DbSet<UserTag> UserTag { get; set; }
        public DbSet<BPFile> BPFiles { get; set; }
    }
}
