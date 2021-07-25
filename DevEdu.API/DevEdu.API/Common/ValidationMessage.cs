namespace DevEdu.API.Common
{
    public static class ValidationMessage
    {
        public const string IdRequired = "Id must be provided";
        public const string WrongFormatId = "Id must be integer from 1 to int.MaxValue";
        public const string FirstNameRequired = "FirstName must be provided";
        public const string LastNameRequired = "LastName must be provided";
        public const string PatronymicRequired = "Patronymic must be provided";
        public const string EmailRequired = "Email must be provided";
        public const string WrongEmailFormat = "You've got to use the correct email format";
        public const string NameRequired = "Name must be provided";
        public const string StartDateRequired = "StartDate must be provided";
        public const string EndDateRequired = "EndDate must be provided";
        public const string DescriptionRequired = "Description must be provided";
        public const string IsRequiredErrorMessage = "IsRequired must be Provided";
        public const string UsernameRequired = "Username must be provided";
        public const string PasswordRequired = "Password must be provided";
        public const string WrongFormatPassword = "Password must contain at least 8 characters";
        public const string ContractNumberRequired = "ContractNumber must be provided";
        public const string CityIdRequired = "CityId must be provided";
        public const string WrongFormatCityId = "CityId must be integer from 1 to int.MaxValue";
        public const string BirthDateRequired = "BirthDate must be provided";
        public const string GitHubAccountRequired = "GitHubAccount must be provided";
        public const string PhotoRequired = "Photo must be provided";
        public const string WrongFormatPhoto = "Photo must be Url";
        public const string PhoneNumberRequired = "PhoneNumber must be provided";
        public const string DurationRequired = "Duration must be provided";
        public const string FeedbackRequired = "Feedback must be provided";
        public const string AbsenceReasonRequired = "AbsenceReason must be provided";
        public const string AttendanceRequired = "Attendance must be provided";
        public const string PositionRequired = "Position must be provided";
        public const string ContentRequired = "Content must be provided";
        public const string TextRequired = "Comment cannot be empty";
        public const string UserIdRequired = "UserId cannot be empty";
        public const string GroupIdRequired = "GroupId must be provided";
        public const string RaitingTypeIdRequired = "RaitingTypeId must be provided";
        public const string RaitingRequired = "Raiting must be provided";
        public const string ReportingPeriodNumberRequired = "ReportingPeriodNumber must be provided";
        public const string DateRequired = "Date must be provided";
        public const string TeacherCommentRequired = "TeacherComment must be provided";
        public const string TeacherIdRequired = "TeacherId must be provided";
        public const string LinkToRecordIdRequired = "LinkToRecord must be provided";
        public const string SumRequired = "Sum must be provided";
        public const string IsPaidRequired = "IsPaid cannot be empty";
        public const string WrongFormatBirthDate = "BirthDate must be format 'dd.MM.yyyy'";
        public const string WrongFormatDate = "the date must be in the format dd.mm.yyyy";
        public const string RoleIdRequired = "RoleId cannot be empty";
        public const string WrongFormatEmailFormat = "Email must be provided";
        public const string StudentAnswerRequired = "Answer of student must be provided";
        public const string CommentUserIdRequired = "UserId of comment must be provided";
        public const string CommentTextRequired = "Text of comment must be provided";
    }
}