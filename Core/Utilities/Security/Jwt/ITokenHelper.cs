using Core.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Core.Utilities.Security.Jwt
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);
        IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims);
    }
}
