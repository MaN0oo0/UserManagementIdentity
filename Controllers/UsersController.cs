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

        public IActionResult Index()
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

        public async Task<IActionResult> Add()
        {
            var Roles = await _roleManager.Roles.Select(r => new RoleViewModel() { RoleId = r.Id, RoleName = r.Name, IsSelected = false }).ToListAsync();
            var ViewModel = new AddUserViewModel()
            {
                Roles = Roles,
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Error Happend !");
                return View(model);
            }
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "Email Is Exists !");
                return View(model);
            }
            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                ModelState.AddModelError("UserName", "UserName Is Exists !");
                return View(model);
            }
            var NewUser = new ApplicationUser()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,

            };
            var res = await _userManager.CreateAsync(NewUser, model.Password);
            if (!res.Succeeded)
            {
                foreach (var err in res.Errors)
                {

                    ModelState.AddModelError("Roles", err.Description);
                }
                return View(model);
            }
            if (model.Roles.Any(x => x.IsSelected))
            {

                var AddToRoles = await _userManager.AddToRolesAsync(NewUser, model.Roles.Where(x => x.IsSelected).Select(r => r.RoleName));
                if (!AddToRoles.Succeeded)
                {
                    ModelState.AddModelError("Error", "Error Happend !");
                    return View(model);
                }

            }
            else
            {
                ModelState.AddModelError("Roles", "Please select at least one role");
                return View(model);
            }
            //  var AddToDefaultRole = await _userManager.AddToRoleAsync(NewUser, Roles.User);

            return RedirectToAction(nameof(Index));


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




                var userRoles = _userManager.GetRolesAsync(User).Result;
                foreach (var item in Model.Roles)
                {
                    if (userRoles.Any(r => r == item.RoleName) && !item.IsSelected)
                    {
                        await _userManager.RemoveFromRoleAsync(User, item.RoleName);
                    }
                    if (!userRoles.Any(r => r == item.RoleName) && item.IsSelected)
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



        public async Task<IActionResult> Edit(string userId)
        {
            try
            {
                var _User = await _userManager.FindByIdAsync(userId);
                if (_User == null)
                    return NotFound();
                var ViewModel = new ProfileFormViewModel()
                {
                    Email = _User.Email,
                    FirstName = _User.FirstName,
                    LastName = _User.LastName,
                    Id = userId,
                    UserName = _User.UserName,
                };
                return View(ViewModel);
            }
            catch (Exception EX)
            {
                ModelState.AddModelError(string.Empty, EX.Message);

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Error Happend !");
                return View(model);
            }

            var TargetUser = await _userManager.FindByIdAsync(model.Id);
            if (TargetUser == null)
                return NotFound();
            var TargetUserEmail = await _userManager.FindByEmailAsync(model.Email);
            if (TargetUserEmail != null && TargetUserEmail.Id != model.Id)
            {
                ModelState.AddModelError("Email", "This email is already assigend to another user");
                return View(model);
            }
            var TargetUserName = await _userManager.FindByNameAsync(model.UserName);
            if (TargetUserName != null && TargetUserName.Id != model.Id)
            {
                ModelState.AddModelError("UserName", "User Name is already assigend to another user");
                return View(model);
            }

            TargetUser.Email = model.Email;
            TargetUser.FirstName = model.FirstName;
            TargetUser.LastName = model.LastName;
            TargetUser.UserName = model.UserName;
            var res = await _userManager.UpdateAsync(TargetUser);
            if (!res.Succeeded)
            {
                foreach (var error in res.Errors)
                {

                    ModelState.AddModelError(string.Empty, error.Description);
                    return View(model);
                }
            }

            return RedirectToAction(nameof(Index));

        }
    }
}
