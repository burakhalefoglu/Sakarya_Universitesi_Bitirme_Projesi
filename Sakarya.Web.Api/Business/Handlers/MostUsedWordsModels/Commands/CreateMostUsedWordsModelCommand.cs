using System;
using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.MostUsedWordsModels.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;

namespace Business.Handlers.MostUsedWordsModels.Commands
{
    /// <summary>
    /// </summary>
    public class CreateMostUsedWordsModelCommand : IRequest<IResult>
    {
        public string[] Words { get; set; }
        public DateTime DateTime { get; set; }


        public class CreateMostUsedWordsModelCommandHandler : IRequestHandler<CreateMostUsedWordsModelCommand, IResult>
        {
            private readonly IMediator _mediator;
            private readonly IMostUsedWordsModelRepository _mostUsedWordsModelRepository;

            public CreateMostUsedWordsModelCommandHandler(IMostUsedWordsModelRepository mostUsedWordsModelRepository,
                IMediator mediator)
            {
                _mostUsedWordsModelRepository = mostUsedWordsModelRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateMostUsedWordsModelValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(LogstashLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateMostUsedWordsModelCommand request,
                CancellationToken cancellationToken)
            {
                var isThereMostUsedWordsModelRecord =
                    !(await _mostUsedWordsModelRepository.GetByFilterAsync(u =>
                        u.DateTime == request.DateTime) is null);

                if (isThereMostUsedWordsModelRecord)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedMostUsedWordsModel = new MostUsedWordsModel
                {
                    Words = request.Words,
                    DateTime = request.DateTime
                };

                await _mostUsedWordsModelRepository.AddAsync(addedMostUsedWordsModel);

                return new SuccessResult(Messages.Added);
            }
        }
    }
}