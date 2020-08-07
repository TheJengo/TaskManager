using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Business.Constants;
using Entity.Dtos;
using FluentValidation;

namespace Business.Validations.FluentValidation
{
    public class UserForLoginDtoValidator : AbstractValidator<UserForLoginDto>
    {
        public UserForLoginDtoValidator()
        {
            ValidatorOptions.LanguageManager.Enabled = false;
            ValidatorOptions.LanguageManager.Culture = new CultureInfo("tr");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Email"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Email"))
                .EmailAddress().WithMessage(ValidationMessages.InvalidEmail);

            RuleFor(x => x.Password)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Şifre"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Şifre"));

        }

        public static bool IsNumber(string text)
        {
            int value;
            if (Int32.TryParse(text, out value))
                return true;
            return false;
        }
    }
}
