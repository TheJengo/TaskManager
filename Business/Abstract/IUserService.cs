using Core.Entity.Concrete;
using Core.Utilities.Results;
using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService
    {
        List<OperationClaim> GetClaims(User user);

        Task<IResult> Add(User user);

        Task<IResult> Update(User user);

        Task<IDataResult<UserDetailsDto>> GetMe();

        Task<IDataResult<User>> GetByMail(string email);

        Task<IDataResult<UserDetailsDto>> GetUserDetailsByMail(string email);

        Task<IDataResult<UserDetailsDto>> GetUserById(int id);

        Task<IDataResult<IList<UserListDto>>> GetAllUsers();

        Task<IResult> BanUserManually(User user, bool state = true);

    }
}
