using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;

namespace Business.Handlers.OperationClaims.Queries
{
    public class GetOperationClaimsQuery : IRequest<IDataResult<IEnumerable<OperationClaim>>>
    {
        public class
            GetOperationClaimsQueryHandler : IRequestHandler<GetOperationClaimsQuery,
                IDataResult<IEnumerable<OperationClaim>>>
        {
            private readonly IMediator _mediator;
            private readonly IOperationClaimRepository _operationClaimRepository;

            public GetOperationClaimsQueryHandler(IOperationClaimRepository operationClaimRepository,
                IMediator mediator)
            {
                _operationClaimRepository = operationClaimRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(LogstashLogger))]
            public async Task<IDataResult<IEnumerable<OperationClaim>>> Handle(GetOperationClaimsQuery request,
                CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<OperationClaim>>(
                    await _operationClaimRepository.GetListAsync());
            }
        }
    }
}