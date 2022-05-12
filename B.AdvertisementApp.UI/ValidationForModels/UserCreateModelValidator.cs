using B.AdvertisementApp.UI.Models;
using FluentValidation;

namespace B.AdvertisementApp.UI.ValidationForModels
{
    public class UserCreateModelValidator:AbstractValidator<UserCreateModel>
    {
        [Obsolete]
        public UserCreateModelValidator()
        {
            //CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x=>x.Password).NotEmpty();
            RuleFor(x => x.Password).MinimumLength(3);          
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword).WithMessage("Parolalar eşleşmiyor");
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.UserName).MinimumLength(3);
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.GenderId).NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => new
            {
                x.FirstName,
                x.UserName
            }).Must(x=>CanNotFirstName(x.UserName,x.FirstName)).WithMessage("Kullanıcı adınız isminizi içermemeli.").When(x=>x.UserName!=null && x.FirstName!=null);
           
           
            
            
        }

        private bool CanNotFirstName(string userName,string firstName)
        {
            return !userName.Contains(firstName);
            
        }
    }
}
