using FluentValidation;
using JwtHomework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtHomework.Business
{
    public class AccountUpdateDtoValidator:AbstractValidator<AccountUpdateDto>
    {
        public AccountUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Kullanıcı Id alanı boş olamaz").InclusiveBetween(1, int.MaxValue).WithMessage("kullanıcı Id si 0 dan büyük olalıdır.");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı Adı Boş olamaz.").MinimumLength(5).WithMessage("Kullanıcı Adı minimim 5 kadakter olmalıdır.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Kullanıcı Email alanı Boş olamaz.").EmailAddress().WithMessage("Kullanıcı Email alanı  email formatinda olmalıdır.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Kullanıcı İsim alanı Boş olamaz.");
        }
    }
}
