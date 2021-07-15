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

        public void UpdateStudentRaiting(StudentRaitingDto studentRaitingDto) => _repository.UpdateStudentRaiting(studentRaitingDto);
    }
}
