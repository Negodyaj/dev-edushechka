using DevEdu.API.Common;
using DevEdu.Business.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace DevEdu.API.Configuration.ExceptionResponses
{
    public class ValidationExceptionResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<ValidationError> Errors { get; set; }

        private const int ValidationCode = 1000;
        private const string MessageValidation = "Validation exception";

        public ValidationExceptionResponse(ValidationException exception)
        {
            Errors = new List<ValidationError>
            {
                new() {Code = ValidationCode, Field = exception.Field, Message = exception.Message}
            };
        }

        public ValidationExceptionResponse(ModelStateDictionary modelState)
        {
            Code = ValidationCode;
            Message = MessageValidation;
            Errors = new List<ValidationError>();
            foreach (var state in modelState)
            {
                if (state.Value.Errors.Count == 0)
                    continue;
                Errors.Add(new ValidationError
                {
                    Code = GetValidationCode(state.Value.Errors[0].ErrorMessage),
                    Field = state.Key,
                    Message = state.Value.Errors[0].ErrorMessage
                });
            }
        }

        private static int GetValidationCode(string exception)
        {
            return exception switch
            {
                ValidationMessage.IdRequired => 1001,
                ValidationMessage.WrongFormatId => 1002,
                ValidationMessage.FirstNameRequired => 1003,
                ValidationMessage.LastNameRequired => 1004,
                ValidationMessage.PatronymicRequired => 1005,
                ValidationMessage.EmailRequired => 1006,
                ValidationMessage.WrongEmailFormat => 1007,
                ValidationMessage.NameRequired => 1008,
                ValidationMessage.StartDateRequired => 1009,
                ValidationMessage.EndDateRequired => 1010,
                ValidationMessage.DescriptionRequired => 1011,
                ValidationMessage.IsRequiredErrorMessage => 1012,
                ValidationMessage.UsernameRequired => 1013,
                ValidationMessage.PasswordRequired => 1014,
                ValidationMessage.WrongFormatPassword => 1015,
                ValidationMessage.ContractNumberRequired => 1016,
                ValidationMessage.CityRequired => 1017,
                ValidationMessage.WrongFormatCityId => 1018,
                ValidationMessage.BirthDateRequired => 1019,
                ValidationMessage.GitHubAccountRequired => 1020,
                ValidationMessage.PhotoRequired => 1021,
                ValidationMessage.WrongFormatPhoto => 1022,
                ValidationMessage.PhoneNumberRequired => 1023,
                ValidationMessage.DurationRequired => 1024,
                ValidationMessage.FeedbackRequired => 1025,
                ValidationMessage.AbsenceReasonRequired => 1026,
                ValidationMessage.AttendanceRequired => 1027,
                ValidationMessage.PositionRequired => 1028,
                ValidationMessage.ContentRequired => 1029,
                ValidationMessage.TextRequired => 1030,
                ValidationMessage.UserIdRequired => 1031,
                ValidationMessage.GroupIdRequired => 1032,
                ValidationMessage.RatingTypeIdRequired => 1033,
                ValidationMessage.RatingRequired => 1034,
                ValidationMessage.ReportingPeriodNumberRequired => 1035,
                ValidationMessage.DateRequired => 1036,
                ValidationMessage.TeacherCommentRequired => 1037,
                ValidationMessage.TeacherIdRequired => 1038,
                ValidationMessage.LinkToRecordIdRequired => 1039,
                ValidationMessage.SumRequired => 1040,
                ValidationMessage.IsPaidRequired => 1041,
                ValidationMessage.WrongFormatBirthDate => 1042,
                ValidationMessage.WrongFormatDate => 1043,
                ValidationMessage.StudentAnswerRequired => 1044,
                ValidationMessage.GroupsRequired => 1046,
                ValidationMessage.CoursesRequired => 1047,
                ValidationMessage.GroupStatusIdRequired => 1048,
                ValidationMessage.TimetableRequired => 1049,
                ValidationMessage.PaymentPerMonthRequired => 1050,
                ValidationMessage.WrongFormatGroupStatusId => 1051,
                ValidationMessage.WrongFormatStartDate => 1052,
                ValidationMessage.WrongValueOfRating => 1053,
                _ => 1500
            };
        }
    }
}