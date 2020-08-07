using Business.Abstract;
using Business.Autofac;
using Business.Constants;
using Business.Validations.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Entity;
using Core.Entity.Concrete;
using Core.Utilities.Helper.Crypto;
using Core.Utilities.Helper.Date;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;

        public AuthManager(IUserService userService,
            ITokenHelper tokenHelper
            )
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            if (user != null)
            {
                var claims = _userService.GetClaims(user);
                var accessToken = _tokenHelper.CreateToken(user, claims);
                return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
            }

            return new ErrorDataResult<AccessToken>(Messages.AccessTokenFailed);
        }

        public IDataResult<IEnumerable<Claim>> GetUserClaims(User user)
        {
            if (user != null)
            {
                var operationClaims = _userService.GetClaims(user);
                var claims = _tokenHelper.SetClaims(user, operationClaims);
                return new SuccessDataResult<IEnumerable<Claim>>(claims);
            }

            return new ErrorDataResult<IEnumerable<Claim>>(Messages.AccessTokenFailed);
        }

        [ValidationAspect(typeof(UserForLoginDtoValidator))]
        public async Task<IDataResult<UserDetailsDto>> UserLogin(UserForLoginDto userForLoginDto)
        {
            var userToCheck = (await _userService.GetByMail(userForLoginDto.Email)).Data;

            if (userToCheck == null)
            {
                return new ErrorDataResult<UserDetailsDto>(Messages.UserNotFound);
            }

            if (!CryptoHelper.ValidatePassword(userForLoginDto.Password, userToCheck.Password))
            {
                return new ErrorDataResult<UserDetailsDto>(Messages.PasswordError);
            }

            if (userToCheck.IsBanned)
            {
                return new ErrorDataResult<UserDetailsDto>(Messages.UserBanned);
            }

            var result = await _userService.GetUserById(userToCheck.Id);
         
            return result;
        }

        [ValidationAspect(typeof(UserForRegisterDtoValidator))]
        [CacheRemoveAspect("IAuthService.UserExists")]
        [CacheRemoveAspect("IUserService.Get")]
        public async Task<IDataResult<UserDetailsDto>> UserRegister(UserForRegisterDto userForRegisterDto, string password)
        {
            var user = new User()
            {
                Password = CryptoHelper.HashPassword(password),
                Email = userForRegisterDto.Email,
            };

            user.UserOperationClaims = SetUserClaims(user);
            var addResult = await _userService.Add(user);

            if (addResult.Success)
            {
                var result = await _userService.GetUserById(user.Id);

                return new SuccessDataResult<UserDetailsDto>(result.Data, Messages.RegisterSucessfull);
            }

            return new ErrorDataResult<UserDetailsDto>(Messages.RegisterFailed);
        }

        [SecuredOperation("Admin, User")]
        public async Task<IResult> Logout(int id = 0)
        {
            return default(IResult);
        }

        [CacheAspect]
        public async Task<IResult> UserExists(string email)
        {
            var result = await _userService.GetByMail(email);

            if (result.Success)
                return new ErrorResult(Messages.UserAlreadyExists);

            return new SuccessResult(Messages.UserAvailable);
        }

        [CacheAspect()]
        private List<UserOperationClaim> SetUserClaims(User user)
        {
            return new List<UserOperationClaim>
                    {
                        new UserOperationClaim
                        {
                            OperationClaimId = 2,
                            User = user,
                            Date = DateTime.Now
                        }
                    };
        }
    }
}
