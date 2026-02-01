using CompanyAssistant.Application.Interfaces;
using CompanyAssistant.Infrastructure.Identity.Jwt;
using Microsoft.AspNetCore.Identity;

namespace CompanyAssistant.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public IdentityService(UserManager<AppUser> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<Guid?> ValidateUserAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return null;

            var valid = await _userManager.CheckPasswordAsync(user, password);

            return valid ? user.Id : null;
        }

        public async Task<string> GenerateJwtAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var roles = await _userManager.GetRolesAsync(user!);

            return _jwtTokenService.GenerateToken(user!, roles);
        }
    }
}
