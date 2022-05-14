using B.AdvertisementApp.Common.Enums;
using B.AdvertisementApp.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.AdvertisementApp.Business.ValidationRules.FluentValidation
{
    public class AdvertisementAppUserCreateDtoValidator:AbstractValidator<AdvertisementAppUserCreateDto>
    {
        public AdvertisementAppUserCreateDtoValidator()
        {
            RuleFor(x=>x.AdvertisementAppUserStatusId).NotEmpty();
            RuleFor(x=>x.AdvertisementId).NotEmpty();
            RuleFor(x=>x.AppUserId).NotEmpty();
            RuleFor(x=>x.CvFile).NotEmpty().WithMessage("Özgeçmiş yükleyiniz.");
            RuleFor(x => x.EndDate).NotEmpty().When(x => x.MilitaryStatusId == (int)MilitaryStatusType.Tecilli).WithMessage("Tecil tarihi boş bırakılamaz.");
            
        }
    }
}
