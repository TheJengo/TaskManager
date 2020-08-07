using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using Business.Constants;
using Castle.Core;
using Entity.Dtos;
using FluentValidation;
using FluentValidation.Resources;
using FluentValidation.Validators;

namespace Business.Validations.FluentValidation
{
    public class UserForRegisterDtoValidator : AbstractValidator<UserForRegisterDto>
    {
        public UserForRegisterDtoValidator()
        {            
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Email"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Email"))
                .EmailAddress().WithMessage(ValidationMessages.InvalidEmail);

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Şifre"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Şifre"))
                .MinimumLength(6).WithMessage(string.Format(ValidationMessages.MinLength, "Şifre", "{MinLength}", "{TotalLength}"));
        }

        public static bool IsFullDigit(string text)
        {
            return text.ToCharArray().Any(x => !Char.IsDigit(x)) ? false : true;
        }
    }
}
