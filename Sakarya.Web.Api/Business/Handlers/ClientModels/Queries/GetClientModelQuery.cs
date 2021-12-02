using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;

namespace Business.Handlers.ClientModels.Queries
{
    public class GetClientModelQuery : IRequest<IDataResult<ClientModel>>
    {
        public string UId { get; set; }

        public class GetClientModelQueryHandler : IRequestHandler<GetClientModelQuery, IDataResult<ClientModel>>
        {
            private readonly IClientModelRepository _clientModelRepository;
            private readonly IMediator _mediator;

            public GetClientModelQueryHandler(IClientModelRepository clientModelRepository, IMediator mediator)
            {
                _clientModelRepository = clientModelRepository;
                _mediator = mediator;
            }

            [LogAspect(typeof(LogstashLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<ClientModel>> Handle(GetClientModelQuery request,
                CancellationToken cancellationToken)
            {
                var clientModel = await _clientModelRepository.GetByFilterAsync(u => u.UId == request.UId);
                return new SuccessDataResult<ClientModel>(clientModel);
            }
        }
    }
}