using Business.Constants;
using Entity.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Validations.FluentValidation
{
    public class UserTaskValidator : AbstractValidator<UserTask>
    {
        public UserTaskValidator()
        {
            RuleFor(x => x.Title)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Başlık"))
               .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Başlık"))
               .MaximumLength(200).WithMessage(string.Format(ValidationMessages.MaxLength, "Başlık", "{MaxLength}", "{TotalLength}"));

            RuleFor(x => x.Description)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Açıklama"))
               .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Açıklama"))
               .MaximumLength(5000).WithMessage(string.Format(ValidationMessages.MaxLength, "Açıklama", "{MaxLength}", "{TotalLength}"));

            RuleFor(x => x.TaskTypeId)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Görev Tipi"))
               .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Görev Tipi"))
               .GreaterThan(0).WithMessage(string.Format(ValidationMessages.MustBeGreaterThan, "Görev Tipi", "{ComparisionValue}"));

            RuleFor(x => x.StartDate)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Başlangıç Tarihi"))
               .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Başlangıç Tarihi"));
        }
    }
}
