using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Handlers.MostUsedWordsModels.Commands;
using Business.Handlers.MostUsedWordsModels.Queries;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using static Business.Handlers.MostUsedWordsModels.Queries.GetMostUsedWordsModelQuery;
using static Business.Handlers.MostUsedWordsModels.Queries.GetMostUsedWordsModelsQuery;
using static Business.Handlers.MostUsedWordsModels.Commands.CreateMostUsedWordsModelCommand;

namespace Tests.Business.Handlers
{
    [TestFixture]
    public class MostUsedWordsModelHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _mostUsedWordsModelRepository = new Mock<IMostUsedWordsModelRepository>();
            _mediator = new Mock<IMediator>();

            _getMostUsedWordsModelQueryHandler =
                new GetMostUsedWordsModelQueryHandler(_mostUsedWordsModelRepository.Object, _mediator.Object);
            _getMostUsedWordsModelsQueryHandler =
                new GetMostUsedWordsModelsQueryHandler(_mostUsedWordsModelRepository.Object, _mediator.Object);
            _createMostUsedWordsModelCommandHandler =
                new CreateMostUsedWordsModelCommandHandler(_mostUsedWordsModelRepository.Object, _mediator.Object);
        }

        private Mock<IMostUsedWordsModelRepository> _mostUsedWordsModelRepository;
        private Mock<IMediator> _mediator;

        private GetMostUsedWordsModelQueryHandler _getMostUsedWordsModelQueryHandler;
        private GetMostUsedWordsModelsQueryHandler _getMostUsedWordsModelsQueryHandler;
        private CreateMostUsedWordsModelCommandHandler _createMostUsedWordsModelCommandHandler;


        [Test]
        public async Task MostUsedWordsModel_GetQuery_Success()
        {
            //Arrange
            var query = new GetMostUsedWordsModelQuery
            {
                DateTime = new DateTime(2021, 11, 12)
            };

            _mostUsedWordsModelRepository.Setup(x =>
                    x.GetByFilterAsync(It.IsAny<Expression<Func<MostUsedWordsModel, bool>>>()))
                .ReturnsAsync(new MostUsedWordsModel
                {
                    DateTime = new DateTime(2021, 11, 12),
                    Words = new[]
                    {
                        "Test",
                        "Test2"
                    }
                });
            //Act
            var x = await _getMostUsedWordsModelQueryHandler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.DateTime.Should().Be(new DateTime(2021, 11, 12));
        }

        [Test]
        public async Task MostUsedWordsModel_GetQueries_Success()
        {
            //Arrange
            var query = new GetMostUsedWordsModelsQuery();

            _mostUsedWordsModelRepository.Setup(x =>
                    x.GetListAsync(It.IsAny<Expression<Func<MostUsedWordsModel, bool>>>()))
                .ReturnsAsync(new List<MostUsedWordsModel>
                {
                    new()
                    {
                        DateTime = new DateTime(2021, 11, 12)
                    },

                    new()
                    {
                        DateTime = new DateTime(2021, 11, 13)
                    }
                }.AsQueryable());

            //Act
            var x = await _getMostUsedWordsModelsQueryHandler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.ToList().Count.Should().BeGreaterThan(1);
        }

        [Test]
        public async Task MostUsedWordsModel_CreateCommand_Success()
        {
            //Arrange
            var command = new CreateMostUsedWordsModelCommand
            {
                DateTime = new DateTime(2021, 11, 12),
                Words = new[]
                {
                    "Test",
                    "Test2"
                }
            };

            _mostUsedWordsModelRepository.Setup(x
                    => x.GetByFilterAsync(It.IsAny<Expression<Func<MostUsedWordsModel, bool>>>()))
                .ReturnsAsync((MostUsedWordsModel) null);

            _mostUsedWordsModelRepository.Setup(x =>
                x.Add(It.IsAny<MostUsedWordsModel>()));

            var x = await _createMostUsedWordsModelCommandHandler.Handle(command, new CancellationToken());

            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task MostUsedWordsModel_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateMostUsedWordsModelCommand
            {
                DateTime = new DateTime(2021, 11, 12),
                Words = new[]
                {
                    "Test",
                    "Test2"
                }
            };

            _mostUsedWordsModelRepository.Setup(x
                    => x.GetByFilterAsync(It.IsAny<Expression<Func<MostUsedWordsModel, bool>>>()))
                .ReturnsAsync(new MostUsedWordsModel
                {
                    DateTime = new DateTime(2021, 11, 12),
                    Words = new[]
                    {
                        "Test"
                    }
                });

            _mostUsedWordsModelRepository.Setup(x => x.Add(It.IsAny<MostUsedWordsModel>()));

            var x = await _createMostUsedWordsModelCommandHandler.Handle(command, new CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }
    }
}