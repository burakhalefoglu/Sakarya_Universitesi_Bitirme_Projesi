
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;

namespace Business.Handlers.ApiInfoModels.Queries
{
    public class GetApiInfoModelQuery : IRequest<IDataResult<ApiInfoModel>>
    {
        public string Type { get; set; }

        public class GetApiInfoModelQueryHandler : IRequestHandler<GetApiInfoModelQuery, IDataResult<ApiInfoModel>>
        {
            private readonly IMlInfoModelRepository _mlInfoModelRepository;
            private readonly IMediator _mediator;

            public GetApiInfoModelQueryHandler(IMlInfoModelRepository mlInfoModelRepository, IMediator mediator)
            {
                _mlInfoModelRepository = mlInfoModelRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<ApiInfoModel>> Handle(GetApiInfoModelQuery request, CancellationToken cancellationToken)
            {
                var mlInfoModel = await _mlInfoModelRepository.GetByFilterAsync(x=> x.TypeKey == request.Type);
                return new SuccessDataResult<ApiInfoModel>(mlInfoModel);
            }
        }

    }
}
