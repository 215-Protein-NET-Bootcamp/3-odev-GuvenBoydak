using FluentValidation;
using JwtHomework.Entities;

namespace JwtHomework.Business
{
    public class AccountRegisterDtoValidator:AbstractValidator<AccountRegisterDto>
    {
        public AccountRegisterDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı Adı Boş olamaz.").MinimumLength(5).WithMessage("Kullanıcı Adı minimim 5 kadakter olmalıdır.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Kullanıcı Password alanı Boş olamaz.").MinimumLength(5).WithMessage("Kullanıcı password alanı  minimim 5 kadakter olmalıdır.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Kullanıcı Email alanı Boş olamaz.").EmailAddress().WithMessage("Kullanıcı Email alanı  email formatinda olmalıdır.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Kullanıcı İsim alanı Boş olamaz.");
            
        }
    }
}
