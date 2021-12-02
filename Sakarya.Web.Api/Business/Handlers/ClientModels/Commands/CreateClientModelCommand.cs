using System;
using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.ClientModels.ValidationRules;
using Business.Helpers;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;

namespace Business.Handlers.ClientModels.Commands
{
    /// <summary>
    /// </summary>
    public class CreateClientModelCommand : IRequest<IResult>
    {
        public string UId { get; set; }
        public int IsPleased { get; set; }

        public class CreateClientModelCommandHandler : IRequestHandler<CreateClientModelCommand, IResult>
        {
            private readonly IClientModelRepository _clientModelRepository;
            private readonly IMediator _mediator;

            public CreateClientModelCommandHandler(IClientModelRepository clientModelRepository, IMediator mediator)
            {
                _clientModelRepository = clientModelRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateClientModelValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(LogstashLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateClientModelCommand request, CancellationToken cancellationToken)
            {
                var isThereClientModelRecord = !(_clientModelRepository
                    .GetByFilterAsync(u => u.UId == request.UId).Result is null);

                if (isThereClientModelRecord)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedClientModel = new ClientModel
                {
                    UId = HexStringHelper.GenerateHexString(16),
                    IsPleased = request.IsPleased,
                    DateTime = DateTime.Now
                };

                await _clientModelRepository.AddAsync(addedClientModel);

                return new SuccessResult(Messages.Added);
            }
        }
    }
}