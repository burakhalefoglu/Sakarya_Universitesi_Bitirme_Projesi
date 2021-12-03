using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Handlers.ApiInfoModels.Queries;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using static Business.Handlers.ApiInfoModels.Queries.GetApiInfoModelQuery;

namespace Tests.Business.Handlers
{
    [TestFixture]
    public class ApiInfoModelHandlerTests
    {
        Mock<IMlInfoModelRepository> _mlInfoModelRepository;
        Mock<IMediator> _mediator;

        private GetApiInfoModelQueryHandler _getMlInfoModelQueryHandler;

        [SetUp]
        public void Setup()
        {
            _mlInfoModelRepository = new Mock<IMlInfoModelRepository>();
            _mediator = new Mock<IMediator>();

            _getMlInfoModelQueryHandler = new GetApiInfoModelQueryHandler(_mlInfoModelRepository.Object, _mediator.Object);

        }

        [Test]
        public async Task MlInfoModel_GetQuery_Success()
        {
            //Arrange
            var query = new GetApiInfoModelQuery
            {
                Type = "TwitterApi"
            };

            _mlInfoModelRepository.Setup(x => x.GetByFilterAsync(It.IsAny<Expression<Func<ApiInfoModel, bool>>>())).ReturnsAsync(new ApiInfoModel()
            {
                MlResultRate = 1,
                RemainingRequest = 500000,
                TotalRequestLimit = 500000
            }
            );


            //Act
            var x = await _getMlInfoModelQueryHandler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.MlResultRate.Should().Be(1);

        }
    }
}

