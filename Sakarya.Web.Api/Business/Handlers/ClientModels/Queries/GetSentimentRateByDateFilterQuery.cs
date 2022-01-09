using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;

namespace Business.Handlers.ClientModels.Queries
{
    public class GetSentimentRateByDateFilterQuery : IRequest<IDataResult<int>>
    {
        public int startDate { get; set; }
        public int finishDate { get; set; }

        public class
            GetSentimentRateByDateFilterQueryHandler : IRequestHandler<GetSentimentRateByDateFilterQuery,
                IDataResult<int>>
        {
            private readonly IClientModelRepository _clientModelRepository;
            private readonly IMediator _mediator;

            public GetSentimentRateByDateFilterQueryHandler(IClientModelRepository clientModelRepository,
                IMediator mediator)
            {
                _clientModelRepository = clientModelRepository;
                _mediator = mediator;
            }

            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<int>> Handle(GetSentimentRateByDateFilterQuery request,
                CancellationToken cancellationToken)
            {
                var result = 0;
                var totalClientCount = _clientModelRepository
                    .GetListAsync(x => x.createdAt >= request.startDate &&
                                       x.createdAt <= request.finishDate).Result.Count();
                var sentimentPositiveCount =
                    _clientModelRepository.GetListAsync(x => x.user_sentiment == 1
                                                             && x.createdAt >= request.startDate
                                                             && x.createdAt <= request.finishDate).Result.Count();
                if (totalClientCount != 0 && sentimentPositiveCount != 0)
                    result = sentimentPositiveCount * 100 / totalClientCount;
                return new SuccessDataResult<int>(result);
            }
        }
    }
}