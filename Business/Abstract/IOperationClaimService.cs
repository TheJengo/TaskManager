using Core.Entity.Concrete;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IOperationClaimService
    {
        Task<IDataResult<IList<OperationClaim>>> GetAllOperationClaims();
        Task<IResult> Add(params OperationClaim[] operationClaims);
        Task<IResult> Update(params OperationClaim[] operationClaims);
        Task<IResult> Delete(params OperationClaim[] operationClaims);
    }
}
