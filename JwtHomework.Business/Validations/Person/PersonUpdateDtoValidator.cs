using FluentValidation;
using JwtHomework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtHomework.Business
{
    public class PersonUpdateDtoValidator:AbstractValidator<PersonUpdateDto>
    {
        public PersonUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id alanı boş geçilemez").GreaterThan(0).WithMessage("Id alanı 0 dan büyük olmalıdır."); 
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("İsim alanı boş geçilemez");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyisim alanı boş geçilemez");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email alanı boş geçilemez");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Telefon alanı boş geçilemez");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı boş geçilemez");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Doğum tarihi alanı boş geçilemez");
            RuleFor(x => x.AccountId).NotEmpty().WithMessage("kullanıcı Id alanı boş geçilemez").GreaterThan(0).WithMessage("Kullanıcı Id alanı 0 dan büyük olmalıdır.");
        }
    }
}
