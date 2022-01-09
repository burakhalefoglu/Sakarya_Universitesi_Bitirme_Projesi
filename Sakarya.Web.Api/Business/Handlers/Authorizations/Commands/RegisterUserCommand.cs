using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Handlers.Authorizations.ValidationRules;
using Business.Handlers.OperationClaims.Queries;
using Business.Helpers;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.ClaimModels;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using MediatR;

namespace Business.Handlers.Authorizations.Commands
{
    public class RegisterUserCommand : IRequest<IDataResult<AccessToken>>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IDataResult<AccessToken>>
        {
            private readonly IMediator _mediator;
            private readonly ITokenHelper _tokenHelper;
            private readonly IUserRepository _userRepository;

            public RegisterUserCommandHandler(IUserRepository userRepository,
                IMediator mediator,
                ITokenHelper tokenHelper)
            {
                _userRepository = userRepository;
                _mediator = mediator;
                _tokenHelper = tokenHelper;
            }

            [PerformanceAspect(5)]
            [ValidationAspect(typeof(RegisterUserValidator), Priority = 2)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(LogstashLogger))]
            [TransactionScopeAspectAsync]
            public async Task<IDataResult<AccessToken>> Handle(RegisterUserCommand request,
                CancellationToken cancellationToken)
            {
                var userExits = await _userRepository.GetByFilterAsync(u => u.Email == request.Email);

                if (userExits != null)
                    return new ErrorDataResult<AccessToken>(Messages.DefaultError);

                HashingHelper.CreatePasswordHash(request.Password, out var passwordSalt, out var passwordHash);
                var user = new User
                {
                    UserId = HexStringHelper.GenerateHexString(16),
                    Email = request.Email,
                    Name = UserNameCreationHelper.EmailToUsername(request.Email),
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Status = true
                };

                await _userRepository.AddAsync(user);


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