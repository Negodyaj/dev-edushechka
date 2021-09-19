using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ICourseValidationHelper _courseValidationHelper;
        private readonly ITopicValidationHelper _topicValidationHelper;
        private readonly ITaskValidationHelper _taskValidationHelper;
        private readonly IMaterialValidationHelper _materialValidationHelper;

        public CourseService
        (
            ICourseRepository courseRepository,
            ITopicRepository topicRepository,
            ITaskRepository taskRepository,
            IMaterialRepository materialRepository,
            IGroupRepository groupRepository,
            ICourseValidationHelper courseValidationHelper,
            ITopicValidationHelper topicValidationHelper,
            IMaterialValidationHelper materialValidationHelper,
            ITaskValidationHelper taskValidationHelper
        )
        {
            _courseRepository = courseRepository;
            _topicRepository = topicRepository;
            _taskRepository = taskRepository;
            _materialRepository = materialRepository;
            _groupRepository = groupRepository;
            _courseValidationHelper = courseValidationHelper;
            _topicValidationHelper = topicValidationHelper;
            _materialValidationHelper = materialValidationHelper;
            _taskValidationHelper = taskValidationHelper;
        }

        public async Task<CourseDto> AddCourseAsync(CourseDto courseDto)
        {
            int addedCourseId = await _courseRepository.AddCourseAsync(courseDto);
            return await _courseRepository.GetCourseAsync(addedCourseId);
        }

        public async Task DeleteCourseAsync(int id)
        {
            await _courseValidationHelper.GetCourseByIdAndThrowIfNotFoundAsync(id);
            await _courseRepository.DeleteCourseAsync(id);
        }

        public async Task<CourseDto> GetCourseAsync(int id)
        {
            var course = await _courseValidationHelper.GetCourseByIdAndThrowIfNotFoundAsync(id);
            return course;
        }

        public async Task<CourseDto> GetFullCourseInfoAsync(int id, UserIdentityInfo userToken)
        {
            var course = await GetCourseAsync(id);

            if (!userToken.Roles.Contains(Role.Admin) && !userToken.Roles.Contains(Role.Methodist))
                await _courseValidationHelper.CourseAccessValidateAsync(course, userToken.UserId);

            course.Tasks = await _taskRepository.GetTasksByCourseIdAsync(course.Id);
            course.Materials = await _materialRepository.GetMaterialsByCourseIdAsync(course.Id);
            course.Groups = await _groupRepository.GetGroupsByCourseIdAsync(course.Id);

            return course;
        }

        public async Task<List<CourseDto>> GetCoursesAsync() => await _courseRepository.GetCoursesAsync();

        public async Task<CourseDto> UpdateCourseAsync(int id, CourseDto courseDto)
        {
            await _courseValidationHelper.GetCourseByIdAndThrowIfNotFoundAsync(id);
            courseDto.Id = id;
            await _courseRepository.UpdateCourseAsync(courseDto);
            return await _courseRepository.GetCourseAsync(id);
        }

        public async Task<int> AddTopicToCourseAsync(int courseId, int topicId, CourseTopicDto dto)
        {
            await CheckCourseAndTopicExistencesAsync(courseId, topicId);
            dto.Course = new CourseDto() { Id = courseId };
            dto.Topic = new TopicDto { Id = topicId };
            return _topicRepository.AddTopicToCourse(dto);
        }

        public async Task<List<int>> AddTopicsToCourseAsync(int courseId, List<CourseTopicDto> listDto)
        {
            await _courseValidationHelper.GetCourseByIdAndThrowIfNotFoundAsync(courseId);
            _topicValidationHelper.GetTopicByListDtoAndThrowIfNotFound(listDto);
            foreach (var topic in listDto)
            {
                topic.Course = new CourseDto() { Id = courseId };
            }
            return _topicRepository.AddTopicsToCourse(listDto);
        }

        public async Task DeleteTopicFromCourseAsync(int courseId, int topicId)
        {
            await CheckCourseAndTopicExistencesAsync(courseId, topicId);
            _topicRepository.DeleteTopicFromCourse(courseId, topicId);
        }

        public async Task<List<CourseTopicDto>> SelectAllTopicsByCourseIdAsync(int courseId)
        {
            await _courseValidationHelper.GetCourseByIdAndThrowIfNotFoundAsync(courseId);
            var list = await _courseRepository.SelectAllTopicsByCourseIdAsync(courseId);
            return list;
        }

        public async Task DeleteTaskFromCourseAsync(int courseId, int taskId)
        {
            await _courseValidationHelper.GetCourseByIdAndThrowIfNotFoundAsync(courseId);
            await _taskValidationHelper.GetTaskByIdAndThrowIfNotFoundAsync(taskId);
            await _courseRepository.DeleteTaskFromCourseAsync(courseId, taskId);
        }


        public async Task AddTaskToCourseAsync(int courseId, int taskId)
        {
            await _courseValidationHelper.GetCourseByIdAndThrowIfNotFoundAsync(courseId);
            await _taskValidationHelper.GetTaskByIdAndThrowIfNotFoundAsync(taskId);
            await _courseRepository.AddTaskToCourseAsync(courseId, taskId);
        }


        public async Task<int> AddCourseMaterialReferenceAsync(int courseId, int materialId)
        {
            await _courseValidationHelper.GetCourseByIdAndThrowIfNotFoundAsync(courseId);
            _materialValidationHelper.GetMaterialByIdAndThrowIfNotFound(materialId);
            return await _courseRepository.AddCourseMaterialReferenceAsync(courseId, materialId);
        }

        public async Task RemoveCourseMaterialReferenceAsync(int courseId, int materialId)
        {
            await _courseValidationHelper.GetCourseByIdAndThrowIfNotFoundAsync(courseId);
            _materialValidationHelper.GetMaterialByIdAndThrowIfNotFound(materialId);
            await _courseRepository.RemoveCourseMaterialReferenceAsync(courseId, materialId);
        }

        public async Task<List<int>> UpdateCourseTopicsByCourseIdAsync(int courseId, List<CourseTopicDto> topics)
        {
            List<int> response;
            if (topics == null || topics.Count == 0)
                throw new EntityNotFoundException(ServiceMessages.EntityNotFound);
            await _courseValidationHelper.GetCourseByIdAndThrowIfNotFoundAsync(courseId);
            CheckUniquenessTopics(topics);
            CheckUniquenessPositions(topics);
            _topicValidationHelper.GetTopicByListDtoAndThrowIfNotFound(topics);
            var topicsInDatabase = await _courseRepository.SelectAllTopicsByCourseIdAsync(courseId);
            if (
                topicsInDatabase != null &&
                topicsInDatabase.Count != 0 &&
                topics.Count != topicsInDatabase.Count
            )
            {
                await DeleteAllTopicsByCourseIdAsync(courseId);
                response = await AddTopicsToCourseAsync(courseId, topics);
            }
            else if (topicsInDatabase == null || topicsInDatabase.Count == 0)
            {
                response = await AddTopicsToCourseAsync(courseId, topics);
            }
            else
            {
                response = new List<int>();
                foreach (var topic in topics)
                {
                    topic.Course = new CourseDto() { Id = courseId };
                    response.Add(topic.Id);
                }
                await _courseRepository.UpdateCourseTopicsByCourseId(topics);

            }
            return response;
        }
        public CourseTopicDto GetCourseTopicById(int id)
        {
            return _topicValidationHelper.GetCourseTopicByIdAndThrowIfNotFound(id);
        }

        public List<CourseTopicDto> GetCourseTopicBySeveralId(List<int> ids)
        {
            return _topicValidationHelper.GetCourseTopicBySeveralIdAndThrowIfNotFound(ids);
        }

        public async Task DeleteAllTopicsByCourseIdAsync(int courseId)
        {
            await _courseValidationHelper.GetCourseByIdAndThrowIfNotFoundAsync(courseId);
            await _courseRepository.DeleteAllTopicsByCourseIdAsync(courseId);
        }

        private static void CheckUniquenessPositions(List<CourseTopicDto> topics)
        {
            if (topics.GroupBy(n => n.Position).Any(c => c.Count() > 1))
            {
                throw new ValidationException(nameof(CourseTopicDto.Position), ServiceMessages.SamePositionsInCourseTopics);
            }
        }

        private static void CheckUniquenessTopics(List<CourseTopicDto> topics)
        {
            if (topics.GroupBy(n => n.Topic.Id).Any(c => c.Count() > 1))
            {
                throw new ValidationException(nameof(CourseTopicDto.Topic), ServiceMessages.SameTopicsInCourseTopics);
            }
        }

        private async Task CheckCourseAndTopicExistencesAsync(int courseId, int topicId)
        {
            await _courseValidationHelper.GetCourseByIdAndThrowIfNotFoundAsync(courseId);
            _topicValidationHelper.GetTopicByIdAndThrowIfNotFound(topicId);
        }
    }
}