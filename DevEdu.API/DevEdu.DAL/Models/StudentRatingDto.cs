using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.DAL.Models
{
    public class StudentRatingDto
    {
        public int Id { get; set; }
        public UserDto User { get; set; }
        public GroupDto Group { get; set; }
        public RatingTypeDto RatingType { get; set; }
        public int Rating { get; set; }
        public int ReportingPeriodNumber { get; set; }

        public override bool Equals(object obj)
        {
            return obj is StudentRatingDto dto &&
                   Id == dto.Id &&
                   EqualityComparer<UserDto>.Default.Equals(User, dto.User) &&
                   EqualityComparer<GroupDto>.Default.Equals(Group, dto.Group) &&
                   EqualityComparer<RatingTypeDto>.Default.Equals(RatingType, dto.RatingType) &&
                   Rating == dto.Rating &&
                   ReportingPeriodNumber == dto.ReportingPeriodNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, User, Group, RatingType, Rating, ReportingPeriodNumber);
        }
    }
}
