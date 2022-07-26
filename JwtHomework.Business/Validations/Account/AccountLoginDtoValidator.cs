using FluentValidation;
using JwtHomework.Entities;

namespace JwtHomework.Business
{
    public class AccountLoginDtoValidator:AbstractValidator<AccountLoginDto>
    {
        public AccountLoginDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı Adı Boş olamaz.").MinimumLength(5).WithMessage("Kullanıcı Adı minimim 5 kadakter olmalıdır.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Kullanıcı Password alanı Boş olamaz.").MinimumLength(5).WithMessage("Kullanıcı password alanı  minimim 5 kadakter olmalıdır.");
        }
    }
}
