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

namespace Business.Handlers.ClientModels.Queries
{
    public class GetClientModelsQuery : IRequest<IDataResult<IEnumerable<ClientModel>>>
    {
        public class
            GetClientModelsQueryHandler : IRequestHandler<GetClientModelsQuery, IDataResult<IEnumerable<ClientModel>>>
        {
            private readonly IClientModelRepository _clientModelRepository;
            private readonly IMediator _mediator;

            public GetClientModelsQueryHandler(IClientModelRepository clientModelRepository, IMediator mediator)
            {
                _clientModelRepository = clientModelRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(LogstashLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<ClientModel>>> Handle(GetClientModelsQuery request,
                CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<ClientModel>>(await _clientModelRepository.GetListAsync());
            }
        }
    }
}