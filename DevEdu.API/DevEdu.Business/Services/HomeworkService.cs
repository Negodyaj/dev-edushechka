using System.Collections.Generic;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.Services
{
    public class HomeworkService : IHomeworkService
    {
        private readonly IHomeworkRepository _homeworkRepository;

        public HomeworkService
        (
            IHomeworkRepository homeworkRepository
        )
        {
            _homeworkRepository = homeworkRepository;
        }

        public HomeworkDto AddHomework(int groupId, int taskId, HomeworkDto dto)
        {
            dto.Group = new GroupDto { Id = groupId };
            dto.Task = new TaskDto { Id = taskId };
            var id= _homeworkRepository.AddHomework(dto);
            return _homeworkRepository.GetHomework(id);
        }

        public void DeleteHomework(int homeworkId) => _homeworkRepository.DeleteHomework(homeworkId);

        public HomeworkDto GetHomework(int homeworkId) => _homeworkRepository.GetHomework(homeworkId);

        public List<HomeworkDto> GetHomeworkByGroupId(int groupId) => _homeworkRepository.GetHomeworkByGroupId(groupId);

        public List<HomeworkDto> GetHomeworkByTaskId(int taskId) => _homeworkRepository.GetHomeworkByTaskId(taskId);

        public HomeworkDto UpdateHomework(int homeworkId, HomeworkDto dto)
        {
            dto.Id = homeworkId;
            _homeworkRepository.UpdateHomework(dto);
            return _homeworkRepository.GetHomework(homeworkId);
        }
    }
}