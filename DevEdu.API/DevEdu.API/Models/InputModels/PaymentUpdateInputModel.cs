using DevEdu.API.Common;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class PaymentUpdateInputModel
    {
        [Required(ErrorMessage = DateRequired)]
        [CustomDateFormatAttribute(ErrorMessage = WrongFormatDate)]
        public string Date { get; set; }

        [Required(ErrorMessage = SumRequired)]
        public int Sum { get; set; }

        [Required(ErrorMessage = IsPaidRequired)]
        public int IsPaid { get; set; }
    }
}
