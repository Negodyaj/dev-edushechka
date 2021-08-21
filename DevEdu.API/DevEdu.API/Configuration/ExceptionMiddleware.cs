using DevEdu.Business.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using DevEdu.API.Common;

namespace DevEdu.API.Configuration
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private const string _messageAuthorization = "Authorization exception";
        private const string _messageValidation = "Validation exception";
        private const string _messageEntity = "Entity not found exception";
        private const string _messageUnknown = "Unknown error";
        private const int _authorizationCode = 2000;
        private const int _entityCode = 400;
        private const int _UnknownCode = 3000;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (AuthorizationException ex)
            {
                await HandlerExceptionMessageAsync(context, ex, HttpStatusCode.Forbidden);
            }
            catch (EntityNotFoundException ex)
            {
                await HandlerExceptionMessageAsync(context, ex, HttpStatusCode.NotFound);
            }
            catch (ValidationException ex)
            {
                await HandleValidationExceptionMessageAsync(context, ex, _messageValidation);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex);
            }
        }

        private static Task HandlerExceptionMessageAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {

            var code = statusCode == HttpStatusCode.Forbidden ? _authorizationCode : _entityCode;
            var message = statusCode == HttpStatusCode.Forbidden ? _messageAuthorization : _messageEntity;

            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(
                new ExceptionResponse
                {
                    Code = code,
                    Message = message,
                    Description = exception.Message
                }
            );
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(result);
        }

        private static Task HandleValidationExceptionMessageAsync(HttpContext context, ValidationException exception, string message)
        {
            var code = GetValidationCode(exception);

            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new ValidationExceptionResponse(exception)
            {
                Code = code,
                Message = message
            });
            context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            return context.Response.WriteAsync(result);
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new
            {
                code = _UnknownCode,
                message = _messageUnknown,
                description = exception.Message
            });
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }

        private static int GetValidationCode(ValidationException exception)
        {
            return exception.Message switch
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
                ValidationMessage.CityIdRequired => 1017,
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
                ValidationMessage.CommentTextRequired => 1045,
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