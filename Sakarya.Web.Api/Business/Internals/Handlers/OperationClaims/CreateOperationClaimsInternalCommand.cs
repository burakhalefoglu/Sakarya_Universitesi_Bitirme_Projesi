using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.OperationClaims.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;

namespace Business.Internals.Handlers.OperationClaims
{
    /// <summary>
    /// </summary>
    public class CreateOperationClaimsInternalCommand : IRequest<IResult>
    {
        public class
            CreateOperationClaimsInternalCommandHandler : IRequestHandler<CreateOperationClaimsInternalCommand, IResult>
        {
            private readonly IOperationClaimRepository _operationClaimRepository;

            public CreateOperationClaimsInternalCommandHandler(IOperationClaimRepository operationClaimRepository)
            {
                _operationClaimRepository = operationClaimRepository;
            }

            [ValidationAspect(typeof(CreateOperationClaimValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(LogstashLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateOperationClaimsInternalCommand request,
                CancellationToken cancellationToken)
            {
                var operationClaimNameList = new List<string>
                {
                    "GetPositiveSentimentRateQuery",
                    "GetSentimentRateByDateFilterQuery",
                    "GetLastClientsByCountQuery",
                    "GetTotalClientCountQuery",
                    "GetApiInfoModelByTypeQuery"
                };
                var operationClaims = new List<OperationClaim>();

                operationClaimNameList.ForEach(o =>
                {
                    operationClaims.Add(new OperationClaim
                    {
                        Name = o
                    });
                });

                operationClaims.ForEach(CreateOperationClaim);
                return new SuccessResult(Messages.Added);
            }

            private async void CreateOperationClaim(OperationClaim oc)
            {
                var isOcExist = !(await _operationClaimRepository.GetByFilterAsync(x => x.Name == oc.Name) is null);
                if (!isOcExist) await _operationClaimRepository.AddAsync(oc);
            }
        }
    }
}