﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;
using System.Linq;
using System.Threading.Tasks;
using DevEdu.API.Common;

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
