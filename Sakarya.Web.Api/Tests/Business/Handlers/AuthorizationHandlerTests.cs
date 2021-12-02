using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Handlers.Authorizations.Commands;
using Business.Handlers.Authorizations.Queries;
using Business.Handlers.OperationClaims.Queries;
using Core.Entities.ClaimModels;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using Tests.Helpers;
using static Business.Handlers.Authorizations.Commands.RegisterUserCommand;
using static Business.Handlers.Authorizations.Queries.LoginUserQuery;

namespace Tests.Business.Handlers
{
    [TestFixture]
    public class AuthorizationHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _tokenHelper = new Mock<ITokenHelper>();
            _mediator = new Mock<IMediator>();

            _loginUserQueryHandler = new LoginUserQueryHandler(_userRepository.Object,
                _tokenHelper.Object,
                _mediator.Object);

            _registerUserCommandHandler = new RegisterUserCommandHandler(_userRepository.Object,
                _mediator.Object, _tokenHelper.Object);
        }

        private Mock<IUserRepository> _userRepository;
        private Mock<ITokenHelper> _tokenHelper;
        private Mock<IMediator> _mediator;
        private LoginUserQueryHandler _loginUserQueryHandler;
        private LoginUserQuery _loginUserQuery;
        private RegisterUserCommandHandler _registerUserCommandHandler;
        private RegisterUserCommand _command;

        [Test]
        public async Task Authorization_Login_UserNotFount()
        {
            var user = DataHelper.GetUser("test");
            HashingHelper.CreatePasswordHash("123456",
                out var passwordSalt,
                out var passwordHash);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;
            _userRepository.Setup(x =>
                    x.GetByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult<User>(null));

            _loginUserQuery = new LoginUserQuery
            {
                Email = user.Email,
                Password = "123456"
            };

            _mediator.Setup(x => x.Send(It.IsAny<GetOperationClaimsQuery>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(
                new SuccessDataResult<IEnumerable<OperationClaim>>(
                    new List<OperationClaim>
                    {
                        new()
                        {
                            Alias = "Test_Alias",
                            Name = "Test",
                            Description = "Test_Description"
                        }
                    }));

            var result = await _loginUserQueryHandler.Handle(_loginUserQuery, new CancellationToken());

            result.Success.Should().BeFalse();
            result.Message.Should().Be(Messages.DefaultError);
        }


        [Test]
        public async Task Authorization_Login_PasswordError()
        {
            var user = DataHelper.GetUser("test");
            HashingHelper.CreatePasswordHash("12345",
                out var passwordSalt,
                out var passwordHash);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            _userRepository.Setup(x =>
                    x.GetByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult(user));

            _loginUserQuery = new LoginUserQuery
            {
                Email = user.Email,
                Password = "123456"
            };

            _mediator.Setup(x => x.Send(It.IsAny<GetOperationClaimsQuery>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(
                new SuccessDataResult<IEnumerable<OperationClaim>>(
                    new List<OperationClaim>
                    {
                        new()
                        {
                            Alias = "Test_Alias",
                            Name = "Test",
                            Description = "Test_Description"
                        }
                    }));

            var result = await _loginUserQueryHandler.Handle(_loginUserQuery, new CancellationToken());

            result.Success.Should().BeFalse();
            result.Message.Should().Be(Messages.DefaultError);
        }


        [Test]
        public async Task Authorization_Login_Success()
        {
            var user = DataHelper.GetUser("test");
            HashingHelper.CreatePasswordHash("123456",
                out var passwordSalt,
                out var passwordHash);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            _userRepository.Setup(x =>
                x.GetByFilterAsync(It.IsAny<Expression<Func<User, bool>>>())).Returns
                (Task.FromResult(user));

            _loginUserQuery = new LoginUserQuery
            {
                Email = user.Email,
                Password = "123456"
            };

            _mediator.Setup(x => x.Send(It.IsAny<GetOperationClaimsQuery>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(
                new SuccessDataResult<IEnumerable<OperationClaim>>(
                    new List<OperationClaim>
                    {
                        new()
                        {
                            Alias = "Test_Alias",
                            Name = "Test",
                            Description = "Test_Description"
                        }
                    }));
            _tokenHelper.Setup(x => x.CreateUserToken<AccessToken>(new UserClaimModel
            {
                UserId = user.UserId,
                OperationClaims = null
            })).Returns(new AccessToken());


            var result = await _loginUserQueryHandler.Handle(_loginUserQuery, new CancellationToken());

            result.Success.Should().BeTrue();
            result.Message.Should().Be(Messages.SuccessfulLogin);
        }


        [Test]
        public async Task Authorization_Register_EmailAlreadyExist()
        {
            var registerUser = new User {Email = "test@test.com", Name = "test test"};
            _command = new RegisterUserCommand
            {
                Email = registerUser.Email,
                Password = "123456"
            };
            _mediator.Setup(x => x.Send(It.IsAny<GetOperationClaimsQuery>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(
                new SuccessDataResult<IEnumerable<OperationClaim>>(
                    new List<OperationClaim>
                    {
                        new()
                        {
                            Alias = "Test_Alias",
                            Name = "Test",
                            Description = "Test_Description"
                        }
                    }));

            _userRepository.Setup(x =>
                    x.GetByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult(registerUser));


            var result = await _registerUserCommandHandler.Handle(_command, new CancellationToken());

            result.Message.Should().Be(Messages.DefaultError);
        }


        [Test]
        public async Task Authorization_Register_SuccessfulLogin()
        {
            var registerUser = new User {UserId = "TESTIUD", Email = "test@test.com", Name = "test test"};
            _command = new RegisterUserCommand
            {
                Email = registerUser.Email,
                Password = "123456"
            };

            _userRepository.Setup(x =>
                    x.GetByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult<User>(null));

            _userRepository.Setup(x => x.AddAsync(It.IsAny<User>()));

            _mediator.Setup(x => x.Send(It.IsAny<GetOperationClaimsQuery>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(
                new SuccessDataResult<IEnumerable<OperationClaim>>(
                    new List<OperationClaim>
                    {
                        new()
                        {
                            Alias = "Test_Alias",
                            Name = "Test",
                            Description = "Test_Description"
                        }
                    }));

            _tokenHelper.Setup(x => x.CreateUserToken<AccessToken>(new UserClaimModel
            {
                UserId = registerUser.UserId,
                OperationClaims = null
            })).Returns(new AccessToken());


            var result = await _registerUserCommandHandler.Handle(_command, new CancellationToken());

            result.Message.Should().Be(Messages.SuccessfulLogin);
        }
    }
}