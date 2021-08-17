using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Tests.TestDataHelpers
{
    public class PaymentData
    {
        public static List<PaymentDto> GetPeyments() =>
            new List<PaymentDto>
            {
                new PaymentDto(){ Id = 1 },
                new PaymentDto(){ Id = 2 },
                new PaymentDto(){ Id = 3}
            };
        public static PaymentDto GetPayment() => new PaymentDto()
        {
            Id = 4,
            Date = DateTime.Now,
            IsDeleted = false,
            User = new UserDto() { Id = 1 }
        };

    }
}
