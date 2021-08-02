using System;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class StudentRatingInputModel
    {
        [Required(ErrorMessage = UserIdRequired)]
        public int UserId { get; set; }

        [Required(ErrorMessage = GroupIdRequired)]
        public int GroupId { get; set; }

        [Required(ErrorMessage = RatingTypeIdRequired)]
        public int RatingTypeId { get; set; }

        [Required(ErrorMessage = RatingRequired)]
        [Range(minimum: 1, maximum: 100, ErrorMessage = WrongValueOfRating)]
        public int Rating { get; set; }

        [Required(ErrorMessage = ReportingPeriodNumberRequired)]
        public int ReportingPeriodNumber { get; set; }

        public override bool Equals(object obj)
        {
            return obj is StudentRatingInputModel model &&
                   UserId == model.UserId &&
                   GroupId == model.GroupId &&
                   RatingTypeId == model.RatingTypeId &&
                   Rating == model.Rating &&
                   ReportingPeriodNumber == model.ReportingPeriodNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserId, GroupId, RatingTypeId, Rating, ReportingPeriodNumber);
        }
    }
}
