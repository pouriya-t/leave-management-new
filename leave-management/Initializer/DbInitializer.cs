using leave_management.Data;
using leave_management.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initalize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            if (_db.Roles.Any(x => x.Name == Helper.Admin)) return;


            _roleManager.CreateAsync(new IdentityRole(Helper.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.Manager)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.User)).GetAwaiter().GetResult();



            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Firstname = "Admin"

            }, "Admin123*").GetAwaiter().GetResult();

            ApplicationUser admin = _db.Users.FirstOrDefault(u => u.Email == "admin@gmail.com");
            _userManager.AddToRoleAsync(admin, Helper.Admin).GetAwaiter().GetResult();





            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "manager@gmail.com",
                Email = "manager@gmail.com",
                EmailConfirmed = true,
                Firstname = "manager"

            }, "Admin123*").GetAwaiter().GetResult();

            ApplicationUser manager = _db.Users.FirstOrDefault(u => u.Email == "manager@gmail.com");
            _userManager.AddToRoleAsync(manager, Helper.Manager).GetAwaiter().GetResult();





            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "user@gmail.com",
                Email = "user@gmail.com",
                EmailConfirmed = true,
                Firstname = "user"

            }, "Admin123*").GetAwaiter().GetResult();

            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Email == "user@gmail.com");
            _userManager.AddToRoleAsync(user, Helper.User).GetAwaiter().GetResult();

        }
    }
}
