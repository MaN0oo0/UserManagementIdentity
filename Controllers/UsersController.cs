using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserManagmentWithIdentity.Helpers;
using UserManagmentWithIdentity.Models;
using UserManagmentWithIdentity.ViewModels;

namespace UserManagmentWithIdentity.Controllers
{
    [Authorize(Roles =Roles.Admin)]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
          
            var Users =  _userManager.Users.Select( x=>new UserViewModel() { 
                Email=x.Email,
                FirstName=x.FirstName,
                LastName=x.LastName,
                Id=x.Id,
                UserName=x.UserName,
                Roles= _userManager.GetRolesAsync(x).Result
             }).ToList();


            return View(Users);
        }
    }
}
