using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.ValidationHelpers
{
    public class StudentAnswerOnTaskValidationHelper : IStudentAnswerOnTaskValidationHelper
    {
        private readonly IStudentAnswerOnTaskRepository _studentAnswerOnTaskRepository;

        public StudentAnswerOnTaskValidationHelper(IStudentAnswerOnTaskRepository studentAnswerOnTaskRepository)
        {
            _studentAnswerOnTaskRepository = studentAnswerOnTaskRepository;
        }

        public void CheckStudentAnswerOnTaskExistence(StudentAnswerOnTaskDto dto)
        {
            var studentAnswerOnTask = _studentAnswerOnTaskRepository.GetStudentAnswerOnTaskByTaskIdAndStudentId(dto.Task.Id, dto.User.Id);
            if (studentAnswerOnTask == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(studentAnswerOnTask), dto)); // Andrey im so sorry =0
        }
    }
}