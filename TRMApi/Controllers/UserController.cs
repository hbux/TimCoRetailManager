using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TRMApi.Data;
using TRMApi.Models;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserData _data;

        public UserController(ApplicationDbContext context, 
            UserManager<IdentityUser> userManager,
            IUserData data)
        {
            _context = context;
            _userManager = userManager;
            _data = data;
        }

        [HttpGet]
        public UserModel GetById()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return _data.GetUserById(userId).First();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> allUsers = new List<ApplicationUserModel>();

            var users = _context.Users.ToList();
            var userRoles = from userRole in _context.UserRoles
                            join role in _context.Roles
                            on userRole.RoleId equals role.Id
                            select new { userRole.UserId, userRole.RoleId, role.Name };

            foreach (var user in users)
            {
                ApplicationUserModel u = new ApplicationUserModel()
                {
                    Id = user.Id,
                    Email = user.Email
                };

                u.Roles = userRoles.Where(x => x.UserId == u.Id).ToDictionary(key => key.RoleId, value => value.Name);

                allUsers.Add(u);
            }

            return allUsers;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
            return _context.Roles.ToDictionary(x => x.Id, x => x.Name);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Admin/AddUserToRole")]
        public async Task AddUserToRole(UserRolePairModel pairing)
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);
            await _userManager.AddToRoleAsync(user, pairing.Role);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Admin/RemoveUserFromRole")]
        public async Task RemoveUserFromRole(UserRolePairModel pairing)
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);
            await _userManager.RemoveFromRoleAsync(user, pairing.Role);
        }
    }
}
