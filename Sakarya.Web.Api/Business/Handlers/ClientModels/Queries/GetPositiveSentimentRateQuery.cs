
using System.Linq;
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading; 
using System.Threading.Tasks;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Logging;
namespace Business.Handlers.ClientModels.Queries
{

    public class GetPositiveSentimentRateQuery : IRequest<IDataResult<int>>
    {
        public class GetPositiveSentimentRateQueryHandler : IRequestHandler<GetPositiveSentimentRateQuery, IDataResult<int>>
        {
            private readonly IClientModelRepository _clientModelRepository;
            private readonly IMediator _mediator;

            public GetPositiveSentimentRateQueryHandler(IClientModelRepository clientModelRepository, IMediator mediator)
            {
                _clientModelRepository = clientModelRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<int>> Handle(GetPositiveSentimentRateQuery request, CancellationToken cancellationToken)
            {
                var result = 0;
                var totalClientCount = _clientModelRepository.GetListAsync().Result.Count();
                var sentimentPositiveCount =
                    _clientModelRepository.GetListAsync(x => x.user_sentiment == 1).Result.Count();
                if(totalClientCount != 0 && sentimentPositiveCount != 0)
                    result = sentimentPositiveCount * 100 / totalClientCount;
                return new SuccessDataResult<int>(result);
            }
        }
    }
}
