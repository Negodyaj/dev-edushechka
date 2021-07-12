using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.DAL.Models
{
    public class PaymentDto:BaseDto
    {

        public DateTime Date { get; set; }
        public int Sum { get; set; }
        public UserDto User { get; set; }
        public bool IsPaid { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PaymentDto dto &&
                   Id == dto.Id &&
                   IsDeleted == dto.IsDeleted &&
                   Date == dto.Date &&
                   Sum == dto.Sum &&
                   EqualityComparer<UserDto>.Default.Equals(User, dto.User) &&
                   IsPaid == dto.IsPaid;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, IsDeleted, Date, Sum, User, IsPaid);
        }
    }
}
