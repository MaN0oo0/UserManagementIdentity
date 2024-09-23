using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserManagmentWithIdentity.ViewModels;

namespace UserManagmentWithIdentity.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var Roles = await _roleManager.Roles.ToListAsync();
            var ss = new SelectList(Roles, "Id", "Name");
            ViewBag.SelectList = ss;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RoleFormViewModel Model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Index), await _roleManager.Roles.ToListAsync());
            }

            var roleExists = await _roleManager.RoleExistsAsync(Model.Name);
            if (roleExists)
            {
                ModelState.AddModelError("Name", "Role Is Exist !");
                return View(nameof(Index),await _roleManager.Roles.ToListAsync());
            }
            await _roleManager.CreateAsync(new IdentityRole { Name = Model.Name.Trim() });
            ModelState.Clear();
            return View(nameof(Index), await _roleManager.Roles.ToListAsync());

        }
    }
}
