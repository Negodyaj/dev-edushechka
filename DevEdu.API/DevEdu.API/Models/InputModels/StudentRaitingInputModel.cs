using System;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class StudentRaitingInputModel
    {
        [Required(ErrorMessage = UserIdRequired)]
        public int UserId { get; set; }

        [Required(ErrorMessage = GroupIdRequired)]
        public int GroupId { get; set; }

        [Required(ErrorMessage = RaitingTypeIdRequired)]
        public int RaitingTypeId { get; set; }

        [Required(ErrorMessage = RaitingRequired)]
        public int Raiting { get; set; }

        [Required(ErrorMessage = ReportingPeriodNumberRequired)]
        public int ReportingPeriodNumber { get; set; }

        public override bool Equals(object obj)
        {
            return obj is StudentRaitingInputModel model &&
                   UserId == model.UserId &&
                   GroupId == model.GroupId &&
                   RaitingTypeId == model.RaitingTypeId &&
                   Raiting == model.Raiting &&
                   ReportingPeriodNumber == model.ReportingPeriodNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserId, GroupId, RaitingTypeId, Raiting, ReportingPeriodNumber);
        }
    }
}
