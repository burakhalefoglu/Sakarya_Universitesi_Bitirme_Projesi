using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;

namespace Business.Handlers.OperationClaims.Commands
{
    /// <summary>
    /// </summary>
    public class DeleteOperationClaimCommand : IRequest<IResult>
    {
        public string Name { get; set; }

        public class DeleteOperationClaimCommandHandler : IRequestHandler<DeleteOperationClaimCommand, IResult>
        {
            private readonly IMediator _mediator;
            private readonly IOperationClaimRepository _operationClaimRepository;

            public DeleteOperationClaimCommandHandler(IOperationClaimRepository operationClaimRepository,
                IMediator mediator)
            {
                _operationClaimRepository = operationClaimRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(LogstashLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteOperationClaimCommand request, CancellationToken cancellationToken)
            {
                var model = await _operationClaimRepository
                    .GetByFilterAsync(u => u.Name == request.Name);

                if (model is null)
                    return new ErrorResult(Messages.DefaultError);


                await _operationClaimRepository.DeleteAsync(model.Id);

                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}