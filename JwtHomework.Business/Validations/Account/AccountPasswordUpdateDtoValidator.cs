using FluentValidation;
using JwtHomework.Entities;
using System.Text.RegularExpressions;

namespace JwtHomework.Business
{
    public class AccountPasswordUpdateDtoValidator:AbstractValidator<AccountPasswordUpdateDto>
    {
        public AccountPasswordUpdateDtoValidator()
        {
            RuleFor(x => x.NewPassword).Must(CheckPassword).WithMessage("Password En az 6 karakter, en az bir sayi, en az bir harf içermelidir."); 
        }

        private bool CheckPassword(string arg)
        {
            Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$");
            if(regex.IsMatch(arg))
                return true;
            else 
                return false;
        }
    }
}
