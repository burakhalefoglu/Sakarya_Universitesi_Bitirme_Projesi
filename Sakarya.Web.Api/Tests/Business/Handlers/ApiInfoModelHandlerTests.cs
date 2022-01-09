using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Business.Handlers.ApiInfoModels.Queries;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using static Business.Handlers.ApiInfoModels.Queries.GetApiInfoModelByTypeQuery;

namespace Tests.Business.Handlers
{
    [TestFixture]
    public class ApiInfoModelHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _mlInfoModelRepository = new Mock<IApiInfoModelRepository>();
            _mediator = new Mock<IMediator>();

            _getMlInfoModelQueryHandler =
                new GetApiInfoModelByTypeQueryHandler(_mlInfoModelRepository.Object, _mediator.Object);
        }

        private Mock<IApiInfoModelRepository> _mlInfoModelRepository;
        private Mock<IMediator> _mediator;

        private GetApiInfoModelByTypeQueryHandler _getMlInfoModelQueryHandler;

        [Test]
        public async Task MlInfoModel_GetQuery_Success()
        {
            //Arrange
            var query = new GetApiInfoModelByTypeQuery
            {
                Type = "TwitterApi"
            };

            _mlInfoModelRepository.Setup(x => x.GetByFilterAsync(It.IsAny<Expression<Func<ApiInfoModel, bool>>>()))
                .ReturnsAsync(new ApiInfoModel
                    {
                        MlResultAccuracyRate = 1,
                        RemainingRequest = 500000,
                        TotalRequestLimit = 500000
                    }
                );


            //Act
            var x = await _getMlInfoModelQueryHandler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.MlResultAccuracyRate.Should().Be(1);
        }
    }
}