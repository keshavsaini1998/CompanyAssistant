using CompanyAssistant.Application.Interfaces;
using CompanyAssistant.Application.UseCases.Results;

namespace CompanyAssistant.Application.UseCases.Commands.Auth
{
    public record LoginCommand(string Username, string Password);

    public class LoginCommandHandler
    {
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<LoginResult> Handle(LoginCommand command)
        {
            var userId = await _identityService
                .ValidateUserAsync(command.Username, command.Password);

            if (userId == null)
                throw new UnauthorizedAccessException();

            var token = await _identityService.GenerateJwtAsync(userId.Value);

            return new LoginResult(token);
        }
    }
}
