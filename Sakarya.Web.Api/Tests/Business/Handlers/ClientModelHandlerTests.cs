using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Handlers.ClientModels.Commands;
using Business.Handlers.ClientModels.Queries;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentAssertions;
using MediatR;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using static Business.Handlers.ClientModels.Queries.GetClientModelQuery;
using static Business.Handlers.ClientModels.Queries.GetClientModelsQuery;
using static Business.Handlers.ClientModels.Commands.CreateClientModelCommand;

namespace Tests.Business.Handlers
{
    [TestFixture]
    public class ClientModelHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _clientModelRepository = new Mock<IClientModelRepository>();
            _mediator = new Mock<IMediator>();


            _getClientModelQueryHandler =
                new GetClientModelQueryHandler(_clientModelRepository.Object, _mediator.Object);
            _getClientModelsQueryHandler =
                new GetClientModelsQueryHandler(_clientModelRepository.Object, _mediator.Object);
            _createClientModelCommandHandler =
                new CreateClientModelCommandHandler(_clientModelRepository.Object, _mediator.Object);
        }

        private Mock<IClientModelRepository> _clientModelRepository;
        private Mock<IMediator> _mediator;

        private GetClientModelQueryHandler _getClientModelQueryHandler;
        private GetClientModelsQueryHandler _getClientModelsQueryHandler;
        private CreateClientModelCommandHandler _createClientModelCommandHandler;

        [Test]
        public async Task ClientModel_GetQuery_Success()
        {
            //Arrange
            var query = new GetClientModelQuery
            {
                UId = "12C41A215B4"
            };

            _clientModelRepository.Setup(x =>
                    x.GetByFilterAsync(It.IsAny<Expression<Func<ClientModel, bool>>>()))
                .ReturnsAsync(new ClientModel
                {
                    UId = "12C41A215B4",
                    IsPleased = 0,
                    DateTime = DateTime.Now
                });

            //Act
            var x = await _getClientModelQueryHandler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.UId.Should().Be("12C41A215B4");
        }

        [Test]
        public async Task ClientModel_GetQueries_Success()
        {
            //Arrange
            var query = new GetClientModelsQuery();

            _clientModelRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<ClientModel, bool>>>()))
                .ReturnsAsync(new List<ClientModel>
                {
                    new()
                    {
                        DateTime = DateTime.Now,
                        IsPleased = 0,
                        UId = "12C41A215B4"
                    },

                    new()
                    {
                        DateTime = DateTime.Now,
                        IsPleased = 1,
                        UId = "82A41A215B9"
                    }
                }.AsQueryable());

            //Act
            var x = await _getClientModelsQueryHandler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.ToList().Count.Should().BeGreaterThan(1);
        }

        [Test]
        public async Task ClientModel_CreateCommand_Success()
        {
            var command = new CreateClientModelCommand
            {
                UId = "TestUId"
            };

            _clientModelRepository.Setup(x => x.GetByIdAsync(It.IsAny<ObjectId>()))
                .ReturnsAsync((ClientModel) null);

            _clientModelRepository.Setup(x => x.Add(It.IsAny<ClientModel>()));

            var x = await _createClientModelCommandHandler.Handle(command, new CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task ClientModel_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateClientModelCommand
            {
                UId = "TestUId"
            };

            _clientModelRepository.Setup(x => x.GetByFilterAsync(
                    It.IsAny<Expression<Func<ClientModel, bool>>>()))
                .ReturnsAsync(new ClientModel
                {
                    UId = "TestUId"
                });

            _clientModelRepository.Setup(x => x.Add(It.IsAny<ClientModel>()));

            var x = await _createClientModelCommandHandler.Handle(command, new CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }
    }
}