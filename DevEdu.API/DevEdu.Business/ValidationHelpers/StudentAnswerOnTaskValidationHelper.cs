using System.Linq;
using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.ValidationHelpers
{
    public class StudentAnswerOnTaskValidationHelper : IStudentAnswerOnTaskValidationHelper
    {
        private readonly IStudentAnswerOnTaskRepository _studentAnswerOnTaskRepository;
        private readonly IGroupRepository _groupRepository;

        public StudentAnswerOnTaskValidationHelper
        (
            IStudentAnswerOnTaskRepository studentAnswerOnTaskRepository,
            IGroupRepository groupRepository
        )
        {
            _studentAnswerOnTaskRepository = studentAnswerOnTaskRepository;
            _groupRepository = groupRepository;
        }

        public void CheckStudentAnswerOnTaskExistence(StudentAnswerOnTaskDto dto)
        {
            var studentAnswerOnTask = _studentAnswerOnTaskRepository.GetStudentAnswerOnTaskByTaskIdAndStudentId(dto);
            if (studentAnswerOnTask == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentAnswerOnTask), dto)); // Andrey im so sorry =0
        }

        public StudentAnswerOnTaskDto CheckStudentAnswerOnTaskExistence(int id)
        {
            var studentAnswerOnTask = _studentAnswerOnTaskRepository.GetStudentAnswerOnTaskById(id);
            if (studentAnswerOnTask == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentAnswerOnTask), id));
            return studentAnswerOnTask;
        }
        
        public void CheckUserInStudentAnswerAccess(int studentId, int userId)
        {
            var groupsByStudent= _groupRepository.GetGroupsByUserId(studentId);
            var groupsByUser= _groupRepository.GetGroupsByUserId(userId);
            var result = groupsByUser.FirstOrDefault(gu => groupsByStudent.Any(gs => gs.Id == gu.Id));
            if (result == default)
                throw new AuthorizationException(string.Format(ServiceMessages.UserHasNoAccessMessage, userId));
        }
    }
}