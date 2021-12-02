using Core.Utilities.Results;
using MediatR;

namespace Business.Services.Authentication.Model
{
    /// <summary>
    ///     It contains login information for different user profiles.
    /// </summary>
    public class LoginUserCommand : IRequest<IDataResult<LoginUserResult>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}