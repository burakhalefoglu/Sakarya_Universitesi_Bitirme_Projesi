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
    public class GetTotalClientCountQuery : IRequest<IDataResult<int>>
    {
        public class
            GetTotalClientCountQueryHandler : IRequestHandler<GetTotalClientCountQuery, IDataResult<int>>
        {
            private readonly IClientModelRepository _clientModelRepository;
            private readonly IMediator _mediator;

            public GetTotalClientCountQueryHandler(IClientModelRepository clientModelRepository, IMediator mediator)
            {
                _clientModelRepository = clientModelRepository;
                _mediator = mediator;
            }

            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<int>> Handle(GetTotalClientCountQuery request,
                CancellationToken cancellationToken)
            {
                return new SuccessDataResult<int>(_clientModelRepository.GetListAsync().Result.Count());
            }
        }
    }
}