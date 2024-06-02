using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Moviegram.Models.Domain;

namespace Moviegram.Data
{
	public class MoviegramDbContext: IdentityDbContext
    {
		public MoviegramDbContext(DbContextOptions<MoviegramDbContext> options) : base(options) 
		{
		}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Roles (user,admin and superadmin)

            var adminRoleId = "6afa0c31-ba74-4ebb-a3da-36f858bf6882";
            var superAdminRoleId = "f4207e3f-0726-48c2-b577-007f4410b83c";
            var userRoleId = "e1660311-160a-4547-bd96-5fd247935aa9";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                },
            };

            builder.Entity<IdentityRole>().HasData(roles);

            //superadmin user
            var superAdminId = "acb61761-258a-4463-a926-7d5dec16b0f0";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadminumut@moviegram.com",
                Email = "superadminumut@moviegram.com",
                NormalizedEmail = "superadminumut@moviegram.com".ToUpper(),
                NormalizedUserName = "superadminumut@moviegram.com".ToUpper(),
                Id = superAdminId
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "Superadminumut@2024");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            // addingroles to superadmin user

            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId,
                },

                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId,
                },

                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId,
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);

        }
        public DbSet<MoviePost> MoviePosts { get; set; }

		public DbSet<Tag> Tags { get; set; }

        public DbSet<MoviePostLike> MoviePostLike { get; set; }

        public DbSet<MoviePostComment> MoviePostComment { get; set; }
    }
}
