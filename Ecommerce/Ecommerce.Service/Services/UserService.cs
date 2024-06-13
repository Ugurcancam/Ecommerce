using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;
using Ecommerce.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
    }
}