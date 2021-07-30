using System;
using System.Collections.Generic;

namespace DevEdu.API.Models
{
    public class StudentRatingOutputModel
    {
        public int Id { get; set; }
        public UserInfoOutPutModel User { get; set; }
        public GroupInfoOutputModel Group { get; set; }
        public RatingTypeOutputModel RatingType { get; set; }
        public int Rating { get; set; }
        public int ReportingPeriodNumber { get; set; }

        public override bool Equals(object obj)
        {
            return obj is StudentRatingOutputModel model &&
                   Id == model.Id &&
                   EqualityComparer<UserInfoOutPutModel>.Default.Equals(User, model.User) &&
                   EqualityComparer<GroupInfoOutputModel>.Default.Equals(Group, model.Group) &&
                   EqualityComparer<RatingTypeOutputModel>.Default.Equals(RatingType, model.RatingType) &&
                   Rating == model.Rating &&
                   ReportingPeriodNumber == model.ReportingPeriodNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, User, Group, RatingType, Rating, ReportingPeriodNumber);
        }
    }
}
