using Core.Entity.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {
        Task<IDataResult<UserDetailsDto>> UserRegister(UserForRegisterDto userForRegisterDto, string password);
        Task<IDataResult<UserDetailsDto>> UserLogin(UserForLoginDto userForLoginDto);
        Task<IResult> UserExists(string username);
        IDataResult<AccessToken> CreateAccessToken(User user);
        IDataResult<IEnumerable<Claim>> GetUserClaims(User user);
        Task<IResult> Logout(int id = 0);
    }
}
