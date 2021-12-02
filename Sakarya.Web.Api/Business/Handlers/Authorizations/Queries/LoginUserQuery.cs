using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Handlers.OperationClaims.Queries;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.ClaimModels;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using MediatR;

namespace Business.Handlers.Authorizations.Queries
{
    public class LoginUserQuery : IRequest<IDataResult<AccessToken>>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, IDataResult<AccessToken>>
        {
            private readonly IMediator _mediator;
            private readonly ITokenHelper _tokenHelper;
            private readonly IUserRepository _userRepository;

            public LoginUserQueryHandler(IUserRepository userRepository,
                ITokenHelper tokenHelper,
                IMediator mediator)
            {
                _userRepository = userRepository;
                _tokenHelper = tokenHelper;
                _mediator = mediator;
            }

            [LogAspect(typeof(LogstashLogger))]
            public async Task<IDataResult<AccessToken>> Handle(LoginUserQuery request,
                CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByFilterAsync(u => u.Email == request.Email);
                // Please use just default error to not give database information !!!
                if (user == null)
                    return new ErrorDataResult<AccessToken>(Messages.DefaultError);

                // Please return just default error to not give database information !!!
                if (!HashingHelper.VerifyPasswordHash(request.Password, user.PasswordSalt, user.PasswordHash))
                    return new ErrorDataResult<AccessToken>(Messages.DefaultError);

                var operationClaims = new List<OperationClaim>();
                var result = await _mediator.Send(new GetOperationClaimsQuery(), cancellationToken);

                if (result.Data.ToList().Count > 0)
                {
                    var selectionItems = result.Data.ToList();

                    operationClaims.AddRange(selectionItems.Select(item =>
                        new OperationClaim {Id = item.Id, Name = item.Name}));
                }

                var accessToken = _tokenHelper.CreateUserToken<AccessToken>(new UserClaimModel
                {
                    UserId = user.UserId,
                    OperationClaims = operationClaims.Select(x => x.Name).ToArray()
                });

                return new SuccessDataResult<AccessToken>(accessToken, Messages.SuccessfulLogin);
            }
        }
    }
}