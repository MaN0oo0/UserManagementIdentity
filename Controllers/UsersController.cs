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
    [Authorize(Roles = Roles.Admin)]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public  IActionResult Index()
        {

            var Users = _userManager.Users.Select(x => new UserViewModel()
            {
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Id = x.Id,
                UserName = x.UserName,
                Roles = _userManager.GetRolesAsync(x).Result
            }).ToList();


            return View(Users);
        }

        public async Task<IActionResult> ManageRole(string UserId)
        {
            try
            {
                var User = await _userManager.FindByIdAsync(UserId);
                if (User == null)
                    return NotFound();

                var Roles = await _roleManager.Roles.ToListAsync();
                var ViewModel = new UserRoleViewModel()
                {
                    UserId = UserId,
                    UserName = User.UserName,
                    Roles = Roles.Select(role => new RoleViewModel()
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                        IsSelected = _userManager.IsInRoleAsync(User, role.Name).Result
                    }).ToList()
                };
                return View(ViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRole(UserRoleViewModel Model)
        {
            try
            {
                var User = await _userManager.FindByIdAsync(Model.UserId);
                if (User == null)
                    return NotFound();




                var userRoles =  _userManager.GetRolesAsync(User).Result;
                foreach (var item in Model.Roles)
                {
                    if (userRoles.Any(r=>r==item.RoleName)&&!item.IsSelected)
                    {
                       await  _userManager.RemoveFromRoleAsync(User, item.RoleName);
                    }
                    if(!userRoles.Any(r => r == item.RoleName) && item.IsSelected)
                    {
                        await _userManager.AddToRoleAsync(User, item.RoleName);
                    }
                    
                }


                return RedirectToAction(nameof(Index));

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
                throw;
            }
        }
    }
}
