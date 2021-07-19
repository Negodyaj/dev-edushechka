using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class RaitingService : IRaitingService
    {
        private readonly IRaitingRepository _repository;

        public RaitingService(IRaitingRepository repository)
        {
            _repository = repository;
        }

        public int AddStudentRaiting(StudentRaitingDto studentRaitingDto) => _repository.AddStudentRaiting(studentRaitingDto);

        public void DeleteStudentRaiting(int id) => _repository.DeleteStudentRaiting(id);

        public List<StudentRaitingDto> GetAllStudentRaitings() => _repository.SelectAllStudentRaitings();

        public StudentRaitingDto GetStudentRaitingById(int id) => _repository.SelectStudentRaitingById(id);

        public List<StudentRaitingDto> GetStudentRaitingByUserId(int userId) => _repository.SelectStudentRaitingByUserId(userId);

        public List<StudentRaitingDto> GetStudentRaitingByGroupId(int groupId) => _repository.SelectStudentRaitingByGroupId(groupId);

        public void UpdateStudentRaiting(int id, int value, int periodNumber)
        {
            var dto = new StudentRaitingDto
            {
                Id = id,
                Raiting = value,
                ReportingPeriodNumber = periodNumber
            };
            _repository.UpdateStudentRaiting(dto);
        }
    }
}
