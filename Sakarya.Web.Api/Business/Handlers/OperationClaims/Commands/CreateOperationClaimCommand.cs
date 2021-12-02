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

namespace Business.Handlers.OperationClaims.Commands
{
    /// <summary>
    /// </summary>
    public class CreateOperationClaimCommand : IRequest<IResult>
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }


        public class CreateOperationClaimCommandHandler : IRequestHandler<CreateOperationClaimCommand, IResult>
        {
            private readonly IMediator _mediator;
            private readonly IOperationClaimRepository _operationClaimRepository;

            public CreateOperationClaimCommandHandler(IOperationClaimRepository operationClaimRepository,
                IMediator mediator)
            {
                _operationClaimRepository = operationClaimRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateOperationClaimValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(LogstashLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateOperationClaimCommand request, CancellationToken cancellationToken)
            {
                var isThereOperationClaimRecord =
                    !(await _operationClaimRepository.GetByFilterAsync(u => u.Name == request.Name)
                        is null);

                if (isThereOperationClaimRecord)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedOperationClaim = new OperationClaim
                {
                    Name = request.Name,
                    Alias = request.Alias,
                    Description = request.Description
                };

                await _operationClaimRepository.AddAsync(addedOperationClaim);

                return new SuccessResult(Messages.Added);
            }
        }
    }
}