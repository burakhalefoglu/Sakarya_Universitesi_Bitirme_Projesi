using Core.DataAccess;
using Core.Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IOperationClaimRepository : IDocumentDbRepository<OperationClaim>
    {
    }
}