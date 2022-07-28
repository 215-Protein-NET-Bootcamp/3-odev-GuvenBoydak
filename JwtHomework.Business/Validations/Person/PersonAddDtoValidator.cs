using FluentValidation;
using FluentValidation.Results;
using JwtHomework.Entities;

namespace JwtHomework.Business
{
    public class PersonAddDtoValidator:AbstractValidator<PersonAddDto>
    {
        public PersonAddDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("İsim alanı boş geçilemez");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyisim alanı boş geçilemez");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email alanı boş geçilemez");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Telefon alanı boş geçilemez");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı boş geçilemez");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Doğum tarihi alanı boş geçilemez");
            RuleFor(x => x.AccountId).NotEmpty().WithMessage("Kullanıcı Id alanı boş geçilemez").GreaterThan(0).WithMessage("Kullanıcı Id alanı 0 dan büyük olmalıdır.");
        }
    }
}
