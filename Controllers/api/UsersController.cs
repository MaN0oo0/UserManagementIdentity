using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using UserManagmentWithIdentity.Models;

namespace UserManagmentWithIdentity.Controllers.api
{
    [Authorize(Roles = Roles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;


        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var _user = await _userManager.FindByIdAsync(userId);
            if (_user == null)
                return NotFound();
            var delRes = await _userManager.DeleteAsync(_user);
            if (!delRes.Succeeded)
                throw new Exception();

            return Ok("User Deleted Success");
        }
    }
}
