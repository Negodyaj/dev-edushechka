using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class GroupServiceTests
    {
        /// <summary>
        /// Осталось, сменить имена методам
        /// Создать правильные експектеды
        /// Заполнить Дтохи
        /// Заменить Тмп
        /// Добавить новые методы
        /// </summary>
        private Mock<IGroupRepository> _mock;
        private int tmp = 1;
        [SetUp]
        public void SetUp()
        {
            _mock = new Mock<IGroupRepository>();
        }

        [Test]
        public void AddGroup()
        {
            //Given
            var dto = GroupData.GetGroupDto();
            _mock.Setup(mock => mock.AddGroup(dto)).Returns(GroupData.ExpectedAffectedRows);
            var service = new GroupService(_mock.Object);

            //When
            var actual = service.AddGroup(dto);

            //Then
            Assert.AreEqual(GroupData.ExpectedAffectedRows, actual);
            _mock.Verify(mock => mock.AddGroup(dto), Times.Once);
        }


        [Test]
        public void GetGroup()
        {
            //Given
            _mock.Setup(mock => mock.GetGroup(tmp)).Returns(new GroupDto());

            var _service = new GroupService(_mock.Object);

            //When
            var actual = _service.GetGroup(tmp);

            //Then
            Assert.AreEqual(tmp, actual);
            _mock.Verify(mock => mock.GetGroup(tmp), Times.Once);
        }

        [Test]
        public void GetGroups()
        {
            //Given
            _mock.Setup(mock => mock.GetGroups())
                .Returns(new List<GroupDto>())
                .Verifiable();

            var _service = new GroupService(_mock.Object);

            //When
            var actual = _service.GetGroups();

            //Then
            Assert.AreEqual(tmp, actual);
            _mock.Verify(mock => mock.GetGroups(), Times.Once);
        }

        [Test]
        public void AddGroupMaterialReference()
        {
            //Given
            _mock.Setup(mock => mock.AddGroupMaterialReference(tmp, tmp))
                .Returns(tmp)
                .Verifiable();

            var _service = new GroupService(_mock.Object);

            //When
            var actual = _service.AddGroupMaterialReference(tmp, tmp);

            //Then
            Assert.AreEqual(tmp, actual);
            _mock.Verify(mock => mock.AddGroupMaterialReference(tmp, tmp), Times.Once);
        }

        [Test]
        public void RemoveGroupMaterialReference()
        {
            //Given
            _mock.Setup(mock => mock.RemoveGroupMaterialReference(tmp, tmp))
                .Returns(tmp)
                .Verifiable();

            var _service = new GroupService(_mock.Object);

            //When
            var actual = _service.RemoveGroupMaterialReference(tmp, tmp);

            //Then
            Assert.AreEqual(tmp, actual);
            _mock.Verify(mock => mock.RemoveGroupMaterialReference(tmp, tmp), Times.Once);
        }
    }
}