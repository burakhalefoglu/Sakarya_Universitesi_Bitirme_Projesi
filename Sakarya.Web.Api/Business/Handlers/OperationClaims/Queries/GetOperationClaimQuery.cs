using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;

namespace Business.Handlers.OperationClaims.Queries
{
    public class GetOperationClaimQuery : IRequest<IDataResult<OperationClaim>>
    {
        public string Name { get; set; }

        public class
            GetOperationClaimQueryHandler : IRequestHandler<GetOperationClaimQuery, IDataResult<OperationClaim>>
        {
            private readonly IMediator _mediator;
            private readonly IOperationClaimRepository _operationClaimRepository;

            public GetOperationClaimQueryHandler(IOperationClaimRepository operationClaimRepository, IMediator mediator)
            {
                _operationClaimRepository = operationClaimRepository;
                _mediator = mediator;
            }

            [LogAspect(typeof(LogstashLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<OperationClaim>> Handle(GetOperationClaimQuery request,
                CancellationToken cancellationToken)
            {
                var operationClaim = await _operationClaimRepository
                    .GetByFilterAsync(u => u.Name == request.Name);
                return new SuccessDataResult<OperationClaim>(operationClaim);
            }
        }
    }
}