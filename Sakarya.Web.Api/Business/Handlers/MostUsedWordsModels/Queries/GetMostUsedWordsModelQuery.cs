using System;
using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;

namespace Business.Handlers.MostUsedWordsModels.Queries
{
    public class GetMostUsedWordsModelQuery : IRequest<IDataResult<MostUsedWordsModel>>
    {
        public DateTime DateTime { get; set; }

        public class
            GetMostUsedWordsModelQueryHandler : IRequestHandler<GetMostUsedWordsModelQuery,
                IDataResult<MostUsedWordsModel>>
        {
            private readonly IMediator _mediator;
            private readonly IMostUsedWordsModelRepository _mostUsedWordsModelRepository;

            public GetMostUsedWordsModelQueryHandler(IMostUsedWordsModelRepository mostUsedWordsModelRepository,
                IMediator mediator)
            {
                _mostUsedWordsModelRepository = mostUsedWordsModelRepository;
                _mediator = mediator;
            }

            [LogAspect(typeof(LogstashLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<MostUsedWordsModel>> Handle(GetMostUsedWordsModelQuery request,
                CancellationToken cancellationToken)
            {
                var mostUsedWordsModel =
                    await _mostUsedWordsModelRepository.GetByFilterAsync(u => u.DateTime == request.DateTime);
                return new SuccessDataResult<MostUsedWordsModel>(mostUsedWordsModel);
            }
        }
    }
}