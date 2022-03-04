using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models
{
    public class UserChangePasswordInputModel
    {
        [Required(ErrorMessage = OldPasswordRequired)]
        [MinLength(8, ErrorMessage = WrongFormatPassword)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = PasswordRequired)]
        [MinLength(8, ErrorMessage = WrongFormatPassword)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = PasswordRequired)]
        [Compare(nameof(NewPassword))]
        public string NewPasswordRepeat { get; set; }

    }
}
