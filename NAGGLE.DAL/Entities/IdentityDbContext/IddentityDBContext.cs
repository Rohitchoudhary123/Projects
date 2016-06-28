using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.DAL.Entities.IddentityDBContext
{
    public class IdentityDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole,
        ApplicationUserClaim>
    {
        public IdentityDBContext()
            : base("DefaultConnection")
        {
        }
        public static IdentityDBContext Create()
        {
            return new IdentityDBContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("User");

            modelBuilder.Entity<ApplicationRole>().ToTable("Role");

            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRole");

            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<ApplicationRole>().ToTable("Role").Property(e => e.Name).HasColumnAnnotation(
       "Index",
       new IndexAnnotation(new[]
           {
                new IndexAttribute("RoleNameIndex") { IsUnique = false }
           }));
        }
    }
}
