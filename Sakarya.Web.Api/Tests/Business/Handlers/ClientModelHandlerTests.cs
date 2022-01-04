using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Handlers.ClientModels.Queries;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentAssertions;
using MediatR;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using static Business.Handlers.ClientModels.Queries.GetLastClientsByCountQuery;
using static Business.Handlers.ClientModels.Queries.GetPositiveSentimentRateQuery;
using static Business.Handlers.ClientModels.Queries.GetSentimentRateByDateFilterQuery;
using static Business.Handlers.ClientModels.Queries.GetTotalClientCountQuery;

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


            _getLastClientsByCountQueryHandler =
                new GetLastClientsByCountQueryHandler( _clientModelRepository.Object, _mediator.Object);
            _getTotalClientCountQueryHandler =
                new GetTotalClientCountQueryHandler( _clientModelRepository.Object, _mediator.Object);
            _getPositiveSentimentRateQueryHandler =
                new GetPositiveSentimentRateQueryHandler(_clientModelRepository.Object, _mediator.Object);
            _getSentimentRateByDateFilterQueryHandler =
                new GetSentimentRateByDateFilterQueryHandler(_clientModelRepository.Object, _mediator.Object);
        }

        private Mock<IClientModelRepository> _clientModelRepository;
        private Mock<IMediator> _mediator;

        private GetLastClientsByCountQueryHandler _getLastClientsByCountQueryHandler;
        private GetPositiveSentimentRateQueryHandler _getPositiveSentimentRateQueryHandler;
        private GetSentimentRateByDateFilterQueryHandler _getSentimentRateByDateFilterQueryHandler;
        private GetTotalClientCountQueryHandler _getTotalClientCountQueryHandler;

        [Test]
        public async Task GetLastClientsByCount_GetQuery_Success()
        {
            //Arrange
            var query = new GetLastClientsByCountQuery
            {
               Count = 2
            };

            _clientModelRepository.Setup(x =>
                    x.GetListByLimitAsync(It.IsAny<int>(),
                        It.IsAny<Expression<Func<ClientModel, bool>>>()))
                .ReturnsAsync(new List<ClientModel>
                {
                    new()
                    {
                        Name = "ABC***"
                    },
                    
                    new()
                    {
                        Name = "CDE***"
                    }

                }.AsQueryable());

            //Act
            var x = await _getLastClientsByCountQueryHandler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.Count().Should().Be(2);
        }

        [Test]
        public async Task GetTotalClientCount_GetQuery_Success()
        {
            //Arrange
            var query = new GetTotalClientCountQuery();

            _clientModelRepository.Setup(x =>
                    x.GetListAsync(It.IsAny<Expression<Func<ClientModel, bool>>>()))
                .ReturnsAsync(new List<ClientModel>
                {
                    new()
                    {
                        Name = "ABC***"
                    },

                    new()
                    {
                        Name = "CDE***"
                    }

                }.AsQueryable());

            //Act
            var x = await _getTotalClientCountQueryHandler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.Should().Be(2);
        }
        [Test]
        public async Task GetPositiveSentimentRateQuery_GetQueries_Success()
        {
            //Arrange
            var query = new GetPositiveSentimentRateQuery();

            _clientModelRepository.Setup(x => x.GetListAsync(null))
                .ReturnsAsync(new List<ClientModel>
                {
                    new()
                    {
                        Name = "ABC***"
                    },

                    new()
                    {
                        Name = "CDE***"
                    },

                    new()
                    {
                        Name = "GHE***"
                    },
                    new()
                    {
                        Name = "XYZ***"
                    },


                }.AsQueryable());

                _clientModelRepository.Setup(x => x.GetListAsync(a => a.user_sentiment == 1))
                .ReturnsAsync(new List<ClientModel>
                {
                    new()
                    {
                        Name = "ABC***"
                    }

                }.AsQueryable());
            //Act
            var x = await _getPositiveSentimentRateQueryHandler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.Should().Be(25);
        }


        [Test]
        public async Task GetSentimentRateByDateFilter_GetQuery_Success()
        {
            //Arrange
            var query = new GetSentimentRateByDateFilterQuery
            {
                startDate = 20211207,
                finishDate = 20211212
            };

            _clientModelRepository.Setup(a => a.GetListAsync(x => x.createdAt >= query.startDate &&
                                                                  x.createdAt <= query.finishDate))
                .ReturnsAsync(new List<ClientModel>
                {
                    new()
                    {
                        Name = "ABC***"
                    },

                    new()
                    {
                        Name = "CDE***"
                    },

                    new()
                    {
                        Name = "GHE***"
                    },
                    new()
                    {
                        Name = "XYZ***"
                    },


                }.AsQueryable());

            _clientModelRepository.Setup(a =>
                    a.GetListAsync(x => x.user_sentiment == 1
                                          && x.createdAt >= query.startDate
                                          && x.createdAt <= query.finishDate))
                .ReturnsAsync(new List<ClientModel>
                {
                    new()
                    {
                        Name = "ABC***"
                    }

                }.AsQueryable());

            //Act
            var x = await _getSentimentRateByDateFilterQueryHandler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.Should().Be(25);
        }

    }
}