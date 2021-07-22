using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.DAL.Models
{
    public class StudentRaitingDto
    {
        public int Id { get; set; }
        public UserDto User { get; set; }
        public GroupDto Group { get; set; }
        public RaitingTypeDto RaitingType { get; set; }
        public int Raiting { get; set; }
        public int ReportingPeriodNumber { get; set; }

        public override bool Equals(object obj)
        {
            return obj is StudentRaitingDto dto &&
                   Id == dto.Id &&
                   EqualityComparer<UserDto>.Default.Equals(User, dto.User) &&
                   EqualityComparer<GroupDto>.Default.Equals(Group, dto.Group) &&
                   EqualityComparer<RaitingTypeDto>.Default.Equals(RaitingType, dto.RaitingType) &&
                   Raiting == dto.Raiting &&
                   ReportingPeriodNumber == dto.ReportingPeriodNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, User, Group, RaitingType, Raiting, ReportingPeriodNumber);
        }
    }
}
