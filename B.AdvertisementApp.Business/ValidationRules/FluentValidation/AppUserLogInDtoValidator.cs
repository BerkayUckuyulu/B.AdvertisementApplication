using B.AdvertisementApp.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.AdvertisementApp.Business.ValidationRules.FluentValidation
{
    public class AppUserLogInDtoValidator:AbstractValidator<AppUserLogInDto>
    {
        public AppUserLogInDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı adı boş olamaz");
            RuleFor(x=>x.Password).NotEmpty().WithMessage("Parola Boş Olamaz.");
        }
    }
}
