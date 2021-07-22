

using System;
using System.Collections.Generic;

namespace DevEdu.API.Models.OutputModels
{
    public class StudentRaitingOutputModel
    {
        public int Id { get; set; }
        public UserInfoOutPutModel User { get; set; } 
        public int GroupId { get; set; } // change to GroupOutputModel
        public RaitingTypeOutputModel RaitingType { get; set; }
        public int Raiting { get; set; }
        public int ReportingPeriodNumber { get; set; }

        public override bool Equals(object obj)
        {
            return obj is StudentRaitingOutputModel model &&
                   Id == model.Id &&
                   EqualityComparer<UserInfoOutPutModel>.Default.Equals(User, model.User) &&
                   GroupId == model.GroupId &&
                   EqualityComparer<RaitingTypeOutputModel>.Default.Equals(RaitingType, model.RaitingType) &&
                   Raiting == model.Raiting &&
                   ReportingPeriodNumber == model.ReportingPeriodNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, User, GroupId, RaitingType, Raiting, ReportingPeriodNumber);
        }
    }
}
