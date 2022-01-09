using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;

namespace Business.Handlers.ApiInfoModels.Queries
{
    public class GetApiInfoModelByTypeQuery : IRequest<IDataResult<ApiInfoModel>>
    {
        public string Type { get; set; }

        public class
            GetApiInfoModelByTypeQueryHandler : IRequestHandler<GetApiInfoModelByTypeQuery, IDataResult<ApiInfoModel>>
        {
            private readonly IApiInfoModelRepository _apiInfoModelRepository;
            private readonly IMediator _mediator;

            public GetApiInfoModelByTypeQueryHandler(IApiInfoModelRepository apiInfoModelRepository, IMediator mediator)
            {
                _apiInfoModelRepository = apiInfoModelRepository;
                _mediator = mediator;
            }

            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<ApiInfoModel>> Handle(GetApiInfoModelByTypeQuery request,
                CancellationToken cancellationToken)
            {
                return new SuccessDataResult<ApiInfoModel>(
                    await _apiInfoModelRepository.GetByFilterAsync(x => x.TypeKey == request.Type));
            }
        }
    }
}