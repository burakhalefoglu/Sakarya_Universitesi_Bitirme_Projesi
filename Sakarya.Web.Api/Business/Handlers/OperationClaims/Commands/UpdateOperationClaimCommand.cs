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
    public class UpdateOperationClaimCommand : IRequest<IResult>
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }

        public class UpdateOperationClaimCommandHandler : IRequestHandler<UpdateOperationClaimCommand, IResult>
        {
            private readonly IMediator _mediator;
            private readonly IOperationClaimRepository _operationClaimRepository;

            public UpdateOperationClaimCommandHandler(IOperationClaimRepository operationClaimRepository,
                IMediator mediator)
            {
                _operationClaimRepository = operationClaimRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateOperationClaimValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(LogstashLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateOperationClaimCommand request,
                CancellationToken cancellationToken)
            {
                var model = await _operationClaimRepository
                    .GetByFilterAsync(u => u.Name == request.Name);

                if (model is null)
                    return new ErrorResult(Messages.DefaultError);

                var operationClaim = new OperationClaim
                {
                    Name = request.Name,
                    Alias = request.Alias,
                    Description = request.Description
                };

                await _operationClaimRepository.UpdateAsync(model.Id, operationClaim);

                return new SuccessResult(Messages.Updated);
            }
        }
    }
}