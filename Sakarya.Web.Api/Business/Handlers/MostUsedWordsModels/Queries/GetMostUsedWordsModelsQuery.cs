using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;

namespace Business.Handlers.MostUsedWordsModels.Queries
{
    public class GetMostUsedWordsModelsQuery : IRequest<IDataResult<IEnumerable<MostUsedWordsModel>>>
    {
        public class GetMostUsedWordsModelsQueryHandler : IRequestHandler<GetMostUsedWordsModelsQuery,
            IDataResult<IEnumerable<MostUsedWordsModel>>>
        {
            private readonly IMediator _mediator;
            private readonly IMostUsedWordsModelRepository _mostUsedWordsModelRepository;

            public GetMostUsedWordsModelsQueryHandler(IMostUsedWordsModelRepository mostUsedWordsModelRepository,
                IMediator mediator)
            {
                _mostUsedWordsModelRepository = mostUsedWordsModelRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(LogstashLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<MostUsedWordsModel>>> Handle(GetMostUsedWordsModelsQuery request,
                CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<MostUsedWordsModel>>(await _mostUsedWordsModelRepository
                    .GetListAsync());
            }
        }
    }
}