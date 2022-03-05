using System.Collections.Generic;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public static class LessonServiceTestCaseSources
    {
        public static IEnumerable<TestCaseData> GetTestCaseDataForUpdateLessonAsyncTest()
        {
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithRole(Role.Teacher, 3);
            var lessonId = LessonData.LessonId;
            var updatedLesson = LessonData.GetUpdatedLessonDto();
            updatedLesson.Topics = new List<TopicDto>();
            var expected = LessonData.GetSelectedLessonDto();
            expected.Topics = new List<TopicDto>();

            yield return new TestCaseData(userIdentity, lessonId, LessonData.GetUpdatedLessonDto(), expected, 1);
            yield return new TestCaseData(userIdentity, lessonId, updatedLesson, LessonData.GetSelectedLessonDto(), 2);

            var updatedLessonTwo = LessonData.GetUpdatedLessonDto();
            updatedLessonTwo.Topics.Remove(updatedLessonTwo.Topics[2]);
            yield return new TestCaseData(userIdentity, lessonId, updatedLessonTwo, LessonData.GetSelectedLessonDto(), 3);

            var expectedTwo = LessonData.GetSelectedLessonDto();
            expectedTwo.Topics.Remove(expectedTwo.Topics[2]);
            yield return new TestCaseData(userIdentity, lessonId, LessonData.GetUpdatedLessonDto(), expectedTwo, 4);

            var updatedLessonThree = LessonData.GetUpdatedLessonDto();
            updatedLessonThree.Topics = new List<TopicDto> { new() { Id = 4 }, new() { Id = 5 }, new() { Id = 6 } };
            yield return new TestCaseData(userIdentity, lessonId, updatedLessonThree, LessonData.GetSelectedLessonDto(), 5);
        }

        public static void CheckVerifyForUpdateLessonAsyncTestByVariantsAndUpdatedLesson(this Mock<ILessonRepository> lessonRepository, int variant,
            LessonDto updatedLesson)
        {
            var lessonId = LessonData.LessonId;
            lessonRepository.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Exactly(2));
            lessonRepository.Verify(x => x.UpdateLessonAsync(updatedLesson), Times.Once);

            switch (variant)
            {
                case 1:
                    lessonRepository.Verify(x => x.AddTopicToLessonAsync(lessonId, 2), Times.Once);
                    lessonRepository.Verify(x => x.AddTopicToLessonAsync(lessonId, 3), Times.Once);
                    lessonRepository.Verify(x => x.AddTopicToLessonAsync(lessonId, 4), Times.Once);
                    break;
                case 2:
                    lessonRepository.Verify(x => x.DeleteTopicFromLessonAsync(lessonId, 2), Times.Once);
                    lessonRepository.Verify(x => x.DeleteTopicFromLessonAsync(lessonId, 3), Times.Once);
                    lessonRepository.Verify(x => x.DeleteTopicFromLessonAsync(lessonId, 4), Times.Once);
                    break;
                case 3:
                    lessonRepository.Verify(x => x.DeleteTopicFromLessonAsync(lessonId, 3), Times.Once);
                    break;
                case 4:
                    lessonRepository.Verify(x => x.AddTopicToLessonAsync(lessonId, 3), Times.Once);
                    break;
                case 5:
                    lessonRepository.Verify(x => x.DeleteTopicFromLessonAsync(lessonId, 2), Times.Once);
                    lessonRepository.Verify(x => x.DeleteTopicFromLessonAsync(lessonId, 3), Times.Once);
                    lessonRepository.Verify(x => x.AddTopicToLessonAsync(lessonId, 5), Times.Once);
                    lessonRepository.Verify(x => x.AddTopicToLessonAsync(lessonId, 6), Times.Once);
                    break;
            }
        }
    }
}