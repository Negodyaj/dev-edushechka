using System;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;


namespace DevEdu.API.Models.InputModels
{
    public class PaymentInputModel
    {
        [Required(ErrorMessage = DateRequired)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = SummRequired)]
        public int Sum { get; set; }

        [Required(ErrorMessage = UserIdRequired)]
        public int UserId { get; set; }

        [Required(ErrorMessage = IsPaidRequired)]
        public int IsPaid { get; set; }

    }
}
