using AutoMapper;
using Business.Abstract;
using Business.Autofac;
using Business.Constants;
using Castle.Core.Logging;
using Core.Aspects.Autofac.Caching;
using Core.CrossCuttingConcern.Logging.Serilog.Loggers;
using Core.DataAccess.Uow;
using Core.Entity.Concrete;
using Core.Extensions;
using Core.Utilities.Helper.Crypto;
using Core.Utilities.Helper.Date;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private IUserDal _userDal;
        private IMapper _mapper;
        private ILoggerService _logger;

        public UserManager(IUserDal userDal,
            IMapper mapper,
            ILoggerService logger)
        {
            _userDal = userDal;
            _mapper = mapper;
            _logger = logger;
        }

        [CacheAspect]
        public List<OperationClaim> GetClaims(User user)
        {
            return _userDal.GetClaims(user);
        }

        [SecuredOperation("Admin", Priority = 1)]
        [CacheAspect(duration: 20, Priority = 2)]
        public async Task<IDataResult<IList<UserListDto>>> GetAllUsers()
        {
            var list = _mapper.Map<IList<UserListDto>>(_userDal.GetAll(x => x.UserOperationClaims));

            return new SuccessDataResult<IList<UserListDto>>(list);
        }

        [SecuredOperation("Admin", Priority = 1)]
        [CacheAspect(duration: 20, Priority = 2)]
        public async Task<IDataResult<UserDetailsDto>> GetMe()
        {
            var userId = SecuredClaimer.GetUserId();

            if (string.IsNullOrEmpty(userId))
                return new ErrorDataResult<UserDetailsDto>();

            var user = _userDal.GetSingle(x => x.Id.Equals(Convert.ToInt32(userId)));

            if (user == null)
                return new ErrorDataResult<UserDetailsDto>();

            var userDetails = _userDal.GetDetailsDto(user);

            return new SuccessDataResult<UserDetailsDto>(_mapper.Map<UserDetailsDto>(user));
        }


        [CacheAspect(duration: 20, Priority = 1)]
        public async Task<IDataResult<User>> GetByMail(string email)
        {
            var user = _userDal.GetSingle(x => x.Email.ToLower().Equals(email.ToLower()));

            if (user == null)
                return new ErrorDataResult<User>();

            return new SuccessDataResult<User>(user);
        }

        [SecuredOperation("Admin, User", Priority = 1)]
        [CacheAspect(duration: 20, Priority = 2)]
        public async Task<IDataResult<UserDetailsDto>> GetUserDetailsByMail(string email)
        {
            var user = await _userDal.GetSingleAsync(x => x.Email.ToLower().Equals(email.ToLower()));

            if (user == null)
                return new ErrorDataResult<UserDetailsDto>();

            return new SuccessDataResult<UserDetailsDto>(_userDal.GetDetailsDto(user));
        }

        [CacheAspect(duration: 20, Priority = 1)]
        public async Task<IDataResult<UserDetailsDto>> GetUserById(int id)
        {
            var user = _userDal.GetSingle(x => x.Id.Equals(id));

            if (user == null)
                return new ErrorDataResult<UserDetailsDto>();

            var userDetails = _mapper.Map<UserDetailsDto>(user);

            if (userDetails == null)
                return new ErrorDataResult<UserDetailsDto>();

            return new SuccessDataResult<UserDetailsDto>(userDetails);
        }

        [SecuredOperation("Admin", Priority = 1)]
        [CacheRemoveAspect("IUserService.Get", Priority = 2)]
        public async Task<IResult> BanUserManually(User user, bool state = true)
        {
            var adminInfo = await GetUserById(Convert.ToInt32(SecuredClaimer.GetUserId()));

            if (adminInfo.Success)
            {
                user.IsBanned = state;
                user.UpdatedDate = DateTime.Now;
                var result = await Update(user);

                return result;
            }

            return new ErrorResult(Messages.AuthorizationDenied);

        }

        [CacheRemoveAspect("IUserService.Get", Priority = 1)]
        [CacheRemoveAspect("IAuthService.UserExists", Priority = 2)]
        public async Task<IResult> Add(User user)
        {
            user.UpdatedDate = null;
            _userDal.Add(user);

            if (_userDal.SaveChanges() > 0)
                return new SuccessResult();

            return new ErrorResult();
        }

        [CacheRemoveAspect("IUserService.Get", Priority = 1)]
        [CacheRemoveAspect("IAuthService.UserExists", Priority = 2)]
        public async Task<IResult> Update(User user)
        {
            var originalUserData = _userDal.GetSingle(x => x.Id == user.Id);

            if (originalUserData == null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            if (user.Password == null)
            {
                user.Password = originalUserData.Password;
            }

            user.UpdatedDate = DateTime.UtcNow;
            _userDal.QuickUpdate(originalUserData, user);

            if (_userDal.SaveChanges() > 0)
                return new SuccessResult();

            return new ErrorResult();
        }
    }
}
