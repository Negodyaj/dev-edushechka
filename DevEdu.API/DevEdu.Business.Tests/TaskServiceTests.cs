using System;
using System.Collections.Generic;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _taskRepoMock;
        private Mock<ICourseRepository> _courseRepoMock;
        private Mock<IStudentAnswerOnTaskRepository> _studentAnswerRepoMock;

        [SetUp]
        public void Setup()
        {
            _taskRepoMock = new Mock<ITaskRepository>();
            _courseRepoMock = new Mock<ICourseRepository>();
            _studentAnswerRepoMock = new Mock<IStudentAnswerOnTaskRepository>();
        }


        [Test]
        public void AddTask_SimpleDtoWithoutTags_TaskCreated()
        {
            //Given
            var expectedTaskId = 55;
            var taskDto = new TaskDto { Name = "Task1", Description = "Description1", Links = "noLinks", IsRequired = true};

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(expectedTaskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            //When
            var actualTaskId = sut.AddTask(taskDto);

            //Than
            Assert.AreEqual(expectedTaskId, actualTaskId);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddTask_DtoWithTags_TaskWithTagsCreated()
        {
            //Given
            var expectedTaskId = 55;
            var taskDto = new TaskDto {
                Name = "Task1",
                Description = "Description1", 
                Links = "noLinks",
                IsRequired = true,
                Tags = new List<TagDto>
                {
                    new TagDto{ Id = 1 },
                    new TagDto{ Id = 2 },
                    new TagDto{ Id = 3 }
                }
            };

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(expectedTaskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(expectedTaskId, It.IsAny<int>()));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            //When
            var actualTaskId = sut.AddTask(taskDto);

            //Than
            Assert.AreEqual(expectedTaskId, actualTaskId);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(expectedTaskId, It.IsAny<int>()), Times.Exactly(3));
        }

        [Test]
        public void GetTaskById_IntTaskId_ReturnedTaskDto()
        {
            //Given
            var taskDto = new TaskDto
            {
                Name = "Task1",
                Description = "Description1",
                Links = "noLinks",
                IsRequired = true,
                Tags = new List<TagDto>
                {
                    new TagDto{ Id = 1 },
                    new TagDto{ Id = 2 },
                    new TagDto{ Id = 3 }
                }
            };

            var taskId = 1;
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            //When
            var dto = sut.GetTaskById(taskId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
        }

        [Test]
        public void GetTaskWithCoursesById_IntTaskId_ReturnedTaskDtoWithCourses()
        {
            //Given
            var taskDto = new TaskDto
            {
                Name = "Task1",
                Description = "Description1",
                Links = "noLinks",
                IsRequired = true,
                Tags = new List<TagDto>
                {
                    new TagDto{ Id = 1 },
                    new TagDto{ Id = 2 },
                    new TagDto{ Id = 3 }
                }
            };

            var courseDtos = new List<CourseDto>
            {
                new CourseDto{ Id = 1 },
                new CourseDto{ Id = 2 }, 
                new CourseDto{ Id = 3 }
            };

            var taskId = 1;
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _courseRepoMock.Setup(x => x.GetCoursesToTaskByTaskId(taskId)).Returns(courseDtos);
            taskDto.Courses = courseDtos;

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            //When
            var dto = sut.GetTaskWithCoursesById(taskId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesToTaskByTaskId(taskId), Times.Once);
        }

        [Test]
        public void GetTaskWithAnswersById_IntTaskId_ReturnedTaskDtoWithStudentAnswers()
        {
            //Given
            var taskDto = new TaskDto
            {
                Name = "Task1",
                Description = "Description1",
                Links = "noLinks",
                IsRequired = true,
                Tags = new List<TagDto>
                {
                    new TagDto{ Id = 1 },
                    new TagDto{ Id = 2 },
                    new TagDto{ Id = 3 }
                }
            };

            var studentAnswersDtos = new List<StudentAnswerOnTaskForTaskDto>
            {
                new StudentAnswerOnTaskForTaskDto{ Id = 1 },
                new StudentAnswerOnTaskForTaskDto{ Id = 2 },
                new StudentAnswerOnTaskForTaskDto{ Id = 3 }
            };

            var taskId = 1;
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _studentAnswerRepoMock.Setup(x => x.GetStudentAnswersToTaskByTaskId(taskId)).Returns(studentAnswersDtos);
            taskDto.StudentAnswers = studentAnswersDtos;

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            //When
            var dto = sut.GetTaskWithAnswersById(taskId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _studentAnswerRepoMock.Verify(x => x.GetStudentAnswersToTaskByTaskId(taskId), Times.Once);
        }

        [Test]
        public void GetTaskWithCoursesAndAnswersById_IntTaskId_ReturnedTaskDtoWithCoursesAndStudentAnswers()
        {
            //Given
            var taskDto = new TaskDto
            {
                Name = "Task1",
                Description = "Description1",
                Links = "noLinks",
                IsRequired = true,
                Tags = new List<TagDto>
                {
                    new TagDto{ Id = 1 },
                    new TagDto{ Id = 2 },
                    new TagDto{ Id = 3 }
                }
            };

            var courseDtos = new List<CourseDto>
            {
                new CourseDto{ Id = 1 },
                new CourseDto{ Id = 2 },
                new CourseDto{ Id = 3 }
            };

            var studentAnswersDtos = new List<StudentAnswerOnTaskForTaskDto>
            {
                new StudentAnswerOnTaskForTaskDto{ Id = 1 },
                new StudentAnswerOnTaskForTaskDto{ Id = 2 },
                new StudentAnswerOnTaskForTaskDto{ Id = 3 }
            };

            var taskId = 1;
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _courseRepoMock.Setup(x => x.GetCoursesToTaskByTaskId(taskId)).Returns(courseDtos);
            taskDto.Courses = courseDtos;
            _studentAnswerRepoMock.Setup(x => x.GetStudentAnswersToTaskByTaskId(taskId)).Returns(studentAnswersDtos);
            taskDto.StudentAnswers = studentAnswersDtos;

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            //When
            var dto = sut.GetTaskWithCoursesAndAnswersById(taskId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesToTaskByTaskId(taskId), Times.Once);
            _studentAnswerRepoMock.Verify(x => x.GetStudentAnswersToTaskByTaskId(taskId), Times.Once);
        }

        [Test]
        public void GetTasks_NoEntry_ReturnedTaskDtos()
        {
            //Given
            var taskDtos = new List<TaskDto>
            {
                new TaskDto
                {
                    Name = "Task1",
                    Description = "Description1",
                    Links = "noLinks",
                    IsRequired = true,
                    Tags = new List<TagDto>
                    {
                        new TagDto{ Id = 1 },
                        new TagDto{ Id = 2 },
                        new TagDto{ Id = 3 }
                    }
                },
                new TaskDto
                {
                    Name = "Task2",
                    Description = "Description2",
                    Links = "noLinks",
                    IsRequired = true,
                    Tags = new List<TagDto>
                    {
                        new TagDto{ Id = 4 },
                        new TagDto{ Id = 5 },
                        new TagDto{ Id = 6 }
                    }
                },
                new TaskDto
                {
                    Name = "Task3",
                    Description = "Description3",
                    Links = "noLinks",
                    IsRequired = true,
                    Tags = new List<TagDto>
                    {
                        new TagDto{ Id = 2 },
                        new TagDto{ Id = 4 },
                        new TagDto{ Id = 6 }
                    }
                }
            };

            var taskId = 1;
            _taskRepoMock.Setup(x => x.GetTasks()).Returns(taskDtos);

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            //When
            var dtos = sut.GetTasks();

            //Than
            Assert.AreEqual(taskDtos, dtos);
            _taskRepoMock.Verify(x => x.GetTasks(), Times.Once);
        }

        [Test]
        public void UpdateTask_TaskDto_ReturnUpdateTaskDto()
        {
            //Given
            var taskDto = new TaskDto
            {
                Name = "Task1",
                Description = "Description1",
                Links = "noLinks",
                IsRequired = true,
                Tags = new List<TagDto>
                {
                    new TagDto {Id = 1},
                    new TagDto {Id = 2},
                    new TagDto {Id = 3}
                }
            };
            var expectedTaskDto = new TaskDto
            {
                Name = "Task2",
                Description = "Description2",
                Links = "noLinks",
                IsRequired = true,
                Tags = new List<TagDto>
                {
                    new TagDto {Id = 3},
                    new TagDto {Id = 4},
                    new TagDto {Id = 2}
                }
            };
            _taskRepoMock.Setup(x => x.UpdateTask(taskDto));
            _taskRepoMock.Setup(x => x.GetTaskById(taskDto.Id)).Returns(expectedTaskDto);

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            //When
            var actualTaskDto = sut.UpdateTask(taskDto);

            //Then
            Assert.AreEqual(expectedTaskDto, actualTaskDto);
            _taskRepoMock.Verify(x => x.UpdateTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.GetTaskById(taskDto.Id), Times.Once);
        }
    }


}
