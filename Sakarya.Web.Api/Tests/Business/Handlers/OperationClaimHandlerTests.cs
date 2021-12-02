using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Handlers.OperationClaims.Commands;
using Business.Handlers.OperationClaims.Queries;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using FluentAssertions;
using MediatR;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using static Business.Handlers.OperationClaims.Queries.GetOperationClaimQuery;
using static Business.Handlers.OperationClaims.Queries.GetOperationClaimsQuery;
using static Business.Handlers.OperationClaims.Commands.CreateOperationClaimCommand;
using static Business.Handlers.OperationClaims.Commands.UpdateOperationClaimCommand;
using static Business.Handlers.OperationClaims.Commands.DeleteOperationClaimCommand;

namespace Tests.Business.Handlers
{
    [TestFixture]
    public class OperationClaimHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _operationClaimRepository = new Mock<IOperationClaimRepository>();
            _mediator = new Mock<IMediator>();

            _getOperationClaimQueryHandler =
                new GetOperationClaimQueryHandler(_operationClaimRepository.Object, _mediator.Object);
            _getOperationClaimsQueryHandler =
                new GetOperationClaimsQueryHandler(_operationClaimRepository.Object, _mediator.Object);
            _createOperationClaimCommandHandler =
                new CreateOperationClaimCommandHandler(_operationClaimRepository.Object, _mediator.Object);
            _updateOperationClaimCommandHandler =
                new UpdateOperationClaimCommandHandler(_operationClaimRepository.Object, _mediator.Object);
            _deleteOperationClaimCommandHandler =
                new DeleteOperationClaimCommandHandler(_operationClaimRepository.Object, _mediator.Object);
        }

        private Mock<IOperationClaimRepository> _operationClaimRepository;
        private Mock<IMediator> _mediator;

        private GetOperationClaimQueryHandler _getOperationClaimQueryHandler;
        private GetOperationClaimsQueryHandler _getOperationClaimsQueryHandler;
        private CreateOperationClaimCommandHandler _createOperationClaimCommandHandler;
        private UpdateOperationClaimCommandHandler _updateOperationClaimCommandHandler;
        private DeleteOperationClaimCommandHandler _deleteOperationClaimCommandHandler;

        [Test]
        public async Task OperationClaim_GetQuery_Success()
        {
            //Arrange
            var query = new GetOperationClaimQuery
            {
                Name = "Test"
            };

            _operationClaimRepository.Setup(x =>
                    x.GetByFilterAsync(It.IsAny<Expression<Func<OperationClaim, bool>>>()))
                .ReturnsAsync(new OperationClaim
                {
                    Name = "Test",
                    Alias = "Test_Alias"
                });

            //Act
            var x = await _getOperationClaimQueryHandler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.Name.Should().Be("Test");
        }

        [Test]
        public async Task OperationClaim_GetQueries_Success()
        {
            //Arrange
            var query = new GetOperationClaimsQuery();

            _operationClaimRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<OperationClaim, bool>>>()))
                .ReturnsAsync(new List<OperationClaim>
                {
                    new()
                    {
                        Name = "Test"
                    },

                    new()
                    {
                        Name = "Test2"
                    }
                }.AsQueryable());

            //Act
            var x = await _getOperationClaimsQueryHandler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.ToList().Count.Should().BeGreaterThan(1);
        }


        [Test]
        public async Task OperationClaim_CreateCommand_Success()
        {
            var command = new CreateOperationClaimCommand
            {
                Alias = "Test_Alias",
                Description = "Test_Desc",
                Name = "Test"
            };

            _operationClaimRepository.Setup(x => x.GetByFilterAsync(It.IsAny<Expression<Func<OperationClaim, bool>>>()))
                .ReturnsAsync((OperationClaim) null);

            _operationClaimRepository.Setup(x => x.Add(It.IsAny<OperationClaim>()));

            var x = await _createOperationClaimCommandHandler.Handle(command, new CancellationToken());

            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task OperationClaim_CreateCommand_NameAlreadyExist()
        {
            var command = new CreateOperationClaimCommand
            {
                Alias = "Test_Alias",
                Description = "Test_Desc",
                Name = "Test"
            };

            _operationClaimRepository.Setup(x => x.GetByFilterAsync(It.IsAny<Expression<Func<OperationClaim, bool>>>()))
                .ReturnsAsync(new OperationClaim
                {
                    Name = "Test"
                });

            _operationClaimRepository.Setup(x => x.Add(It.IsAny<OperationClaim>()));

            var x = await _createOperationClaimCommandHandler.Handle(command, new CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task OperationClaim_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateOperationClaimCommand
            {
                Name = "test",
                Alias = "Test_Alias",
                Description = "Test_Desc"
            };

            _operationClaimRepository.Setup(x => x.GetByFilterAsync
                    (It.IsAny<Expression<Func<OperationClaim, bool>>>()))
                .ReturnsAsync(new OperationClaim
                {
                    Name = "Test"
                });

            _operationClaimRepository.Setup(x => x.UpdateAsync(It.IsAny<ObjectId>(), It.IsAny<OperationClaim>()));

            var x = await _updateOperationClaimCommandHandler.Handle(command, new CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task OperationClaim_UpdateCommand_DefaultError()
        {
            //Arrange
            var command = new UpdateOperationClaimCommand
            {
                Name = "test",
                Alias = "Test_Alias",
                Description = "Test_Desc"
            };

            _operationClaimRepository.Setup(x => x.GetByFilterAsync
                    (It.IsAny<Expression<Func<OperationClaim, bool>>>()))
                .ReturnsAsync((OperationClaim) null);

            _operationClaimRepository.Setup(x => x.UpdateAsync(It.IsAny<ObjectId>(), It.IsAny<OperationClaim>()));

            var x = await _updateOperationClaimCommandHandler.Handle(command, new CancellationToken());


            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.DefaultError);
        }

        [Test]
        public async Task OperationClaim_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteOperationClaimCommand();
            command.Name = "Test";

            _operationClaimRepository.Setup(x => x.GetByFilterAsync(It.IsAny<Expression<Func<OperationClaim, bool>>>()))
                .ReturnsAsync(new OperationClaim
                {
                    Name = "Test"
                });

            _operationClaimRepository.Setup(x => x.Delete(It.IsAny<OperationClaim>()));

            var x = await _deleteOperationClaimCommandHandler.Handle(command, new CancellationToken());

            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }


        [Test]
        public async Task OperationClaim_DeleteCommand_DefaultError()
        {
            //Arrange
            var command = new DeleteOperationClaimCommand
            {
                Name = "Test"
            };

            _operationClaimRepository.Setup(x => x.GetByFilterAsync(It.IsAny<Expression<Func<OperationClaim, bool>>>()))
                .ReturnsAsync((OperationClaim) null);

            _operationClaimRepository.Setup(x => x.Delete(It.IsAny<OperationClaim>()));

            var x = await _deleteOperationClaimCommandHandler.Handle(command, new CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.DefaultError);
        }
    }
}