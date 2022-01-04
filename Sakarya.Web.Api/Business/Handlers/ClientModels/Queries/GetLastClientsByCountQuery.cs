
using Business.BusinessAspects;
using Core.Utilities.Results;
using Core.Aspects.Autofac.Performance;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.ClientModels.Queries
{

    public class GetLastClientsByCountQuery : IRequest<IDataResult<IEnumerable<ClientModel>>>
    {
        public int Count { get; set; }

        public class GetLastClientsByCountQueryHandler : IRequestHandler<GetLastClientsByCountQuery, IDataResult<IEnumerable<ClientModel>>>
        {
            private readonly IClientModelRepository _clientModelRepository;
            private readonly IMediator _mediator;

            public GetLastClientsByCountQueryHandler(IClientModelRepository clientModelRepository, IMediator mediator)
            {
                _clientModelRepository = clientModelRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<ClientModel>>> Handle(GetLastClientsByCountQuery request, CancellationToken cancellationToken)
            {
                var clients = _clientModelRepository.GetListByLimitAsync(request.Count).Result.ToList();
                return new SuccessDataResult<IEnumerable<ClientModel>>(clients);
            }
        }
    }
}