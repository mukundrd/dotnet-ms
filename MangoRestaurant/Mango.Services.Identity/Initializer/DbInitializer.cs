using IdentityModel;
using Mango.Services.Identity.Models;
using Mango.Services.ProductsAPI.Contexts;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static Mango.Services.Identity.SD;

namespace Mango.Services.Identity.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDBContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDBContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (AreRolesCreated())
            {
                return;
            }

            var result1 = CreateUserAndRoles(new ApplicationUser()
            {
                UserName = "admin",
                Email = "admin@mango.com",
                EmailConfirmed = true,
                PhoneNumber = "9876543210",
                FirstName = "Admin",
                LastName = "User"
            }, "Admin@123", SD.UserRole.Admin);

            var result2 = CreateUserAndRoles(new ApplicationUser()
            {
                UserName = "customer",
                Email = "customer1@mango.com",
                EmailConfirmed = true,
                PhoneNumber = "9876543211",
                FirstName = "Cust",
                LastName = "User"
            }, "Customer@123", SD.UserRole.Customer);
        }

        private bool AreRolesCreated()
        {
            if (_roleManager.FindByNameAsync(SD.UserRole.Admin.ToString()).Result != null)
            {
                return true;
            }

            _roleManager.CreateAsync(new IdentityRole(SD.UserRole.Admin.ToString())).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.UserRole.Customer.ToString())).GetAwaiter().GetResult();

            return false;

        }

        private IdentityResult CreateUserAndRoles(ApplicationUser user, string password, UserRole role)
        {
            _userManager.CreateAsync(user, password).GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(user, role.ToString()).GetAwaiter().GetResult();

            return _userManager.AddClaimsAsync(user, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(JwtClaimTypes.GivenName, user.FirstName),
                new Claim(JwtClaimTypes.FamilyName, user.LastName),
                new Claim(JwtClaimTypes.Role, role.ToString())
            }).Result;
        }
    }
}
