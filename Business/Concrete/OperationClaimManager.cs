using Business.Abstract;
using Core.Aspects.Autofac.Caching;
using Core.DataAccess.Uow;
using Core.Entity.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class OperationClaimManager : IOperationClaimService
    {
        private IOperationClaimDal _operationClaimDal;

        public OperationClaimManager(IOperationClaimDal operationClaimDal)
        {
            _operationClaimDal = operationClaimDal;
        }

        [CacheAspect]
        public async Task<IDataResult<IList<OperationClaim>>> GetAllOperationClaims()
        {
            var list = await _operationClaimDal.GetAllAsync();

            return new SuccessDataResult<IList<OperationClaim>>(list);
        }

        [CacheRemoveAspect("IOperationClaimService.Get")]
        public async Task<IResult> Add(params OperationClaim[] operationClaims)
        {
            _operationClaimDal.AddRange(operationClaims);

            if (await _operationClaimDal.SaveChangesAsync() > 0)
                return new SuccessResult();

            return new ErrorResult();
        }

        [CacheRemoveAspect("IOperationClaimService.Get")]
        public async Task<IResult> Delete(params OperationClaim[] operationClaims)
        {
            var roles = _operationClaimDal.GetAll(x => x.UserOperationClaims);
            var deletable = roles.Where(x => operationClaims.Any(y => y.Id == x.Id) && x.UserOperationClaims.Count == 0).ToList();

            _operationClaimDal.DeleteRange(deletable.ToArray());

            if (await _operationClaimDal.SaveChangesAsync() > 0)
                return new SuccessResult();

            return new ErrorResult();
        }

        [CacheRemoveAspect("IOperationClaimService.Get")]
        public async Task<IResult> Update(params OperationClaim[] operationClaims)
        {
            foreach (var operationClaim in operationClaims)
            {
                _operationClaimDal.Update(operationClaim);
            }

            if (await _operationClaimDal.SaveChangesAsync() > 0)
                return new SuccessResult();

            return new ErrorResult();
        }
    }
}
