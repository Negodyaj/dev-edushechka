using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.Tests.TestDataHelpers;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class PaymentServiceTests
    {
        private Mock<IPaymentRepository> _paymentRepoMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IPaymentValidationHelper> _paymentValidationHelperMock;
        private PaymentService _sut;

        [SetUp]
        public void Setup()
        {
            _paymentRepoMock = new Mock<IPaymentRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _paymentValidationHelperMock = new Mock<IPaymentValidationHelper>();
            _sut = new PaymentService(_paymentRepoMock.Object,
                                       new PaymentValidationHelper(_paymentRepoMock.Object),
                                       new UserValidationHelper(_userRepositoryMock.Object));

        }
        [Test]
        public void GetPayment_ById_ValidPaymentRequestId_GotPayment()
        {
            //Given
            var paymentId = 2;
            _paymentRepoMock.Setup(x => x.GetPaymentAsync(paymentId)).Returns(new PaymentDto() { Id = paymentId });
            //When
            _sut.GetPaymentAsync(paymentId);
            //Then
            _paymentRepoMock.Verify(x => x.GetPaymentAsync(paymentId), Times.Once);
        }
        [Test]
        public void GetPayment_ById_NotValidPaymentRequestId_EntityNotFoundExceptionThrown()
        {
            //Given
            var paymentId = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "payment", paymentId);
            _paymentRepoMock.Setup(x => x.GetPaymentAsync(paymentId));
            _paymentValidationHelperMock.Setup(x => x.GetPaymentByIdAndThrowIfNotFoundAsync(paymentId));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.GetPaymentAsync(paymentId));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.GetPaymentAsync(paymentId), Times.Once);
        }
        [Test]
        public void GetPaymentsByUserId_ValidUserId_PaymentsGotten()
        {
            //Given
            var userId = 2;
            var paymentsInDB = PaymentData.GetPeyments();
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId)).Returns(new UserDto() { Id = userId });
            _paymentRepoMock.Setup(x => x.GetPaymentsByUserAsync(userId)).Returns(paymentsInDB);
            //When
            _sut.GetPaymentsByUserIdAsync(userId);
            //Then
            _paymentRepoMock.Verify(x => x.GetPaymentsByUserAsync(userId), Times.Once);
        }
        [Test]
        public void GetPaymentsByUserId_NotValidUserId_EntityNotFoundExceptionThrown()
        {
            //Given
            var userId = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "user", userId);
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.GetPaymentsByUserIdAsync(userId));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.GetPaymentsByUserAsync(userId), Times.Never);
        }
        [Test]
        public void GetPaymentsByUserId_ValidUserIdEntityInDataBaseIsAbssent_EntityNotFoundExceptionThrown()
        {
            //Given
            var userId = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundByUserId, "payments", userId);
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId)).Returns(new UserDto() { Id = userId });
            _paymentRepoMock.Setup(x => x.GetPaymentsByUserAsync(userId));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.GetPaymentsByUserIdAsync(userId));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.GetPaymentsByUserAsync(userId), Times.Once);
        }
        [Test]
        public void AddPayment_WithSimpleDto_PaymentWasAdded()
        {
            //Given
            var payment = PaymentData.GetPayment();
            //When
            _sut.AddPaymentAsync(payment);
            //Then
            _paymentRepoMock.Verify(x => x.AddPaymentAsync(payment), Times.Once);
        }
        [Test]
        public void DeletePayment_ValidPaymentId_PaymentDeleted()
        {
            //Given
            var id = 2;
            _paymentRepoMock.Setup(x => x.GetPaymentAsync(id)).Returns(new PaymentDto() { Id = id });
            //When
            _sut.DeletePaymentAsync(id);
            //Then
            _paymentRepoMock.Verify(x => x.DeletePaymentAsync(id), Times.Once);
        }
        [Test]
        public void DeletePayment_NotValidPaymentId_PaymentDeleted()
        {
            //Given
            var id = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "payment", id);
            _paymentRepoMock.Setup(x => x.GetPaymentAsync(id));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.DeletePaymentAsync(id));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.DeletePaymentAsync(id), Times.Never);
        }
        [Test]
        public void UpdatePayment_ValidPaymentId_PaymentWasUpdated()
        {
            //Given
            var id = 2;
            var payment = PaymentData.GetPayment();
            _paymentRepoMock.Setup(x => x.GetPaymentAsync(id)).Returns(payment);
            //When
            _sut.UpdatePaymentAsync(id, payment);
            //Then
            _paymentRepoMock.Verify(x => x.UpdatePaymentAsync(payment), Times.Once);
        }
        [Test]
        public void UpdatePayment_NotValidPaymentId_EntityNotFoundExceptionThrown()
        {
            //Given
            var id = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "payment", id);
            var payment = PaymentData.GetPayment();
            _paymentRepoMock.Setup(x => x.GetPaymentAsync(id));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.UpdatePaymentAsync(id, payment));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.UpdatePaymentAsync(payment), Times.Never);
        }
        [Test]
        public void UpdatePayment_WithEmptyDto_EntityNotFoundExceptionThrown()
        {
            //Given
            var id = 4;
            var paymentInDb = PaymentData.GetPayment();
            var exp = ServiceMessages.EntityNotFound;
            PaymentDto payment = default;
            _paymentRepoMock.Setup(x => x.GetPaymentAsync(id)).Returns(paymentInDb);
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.UpdatePaymentAsync(id, payment));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.UpdatePaymentAsync(payment), Times.Never);
        }
        [Test]
        public void AddPayments_WithListPaymentDto_PaymentsWereAdded()
        {
            //Given
            var payments = PaymentData.GetPeyments();
            //When
            _sut.AddPaymentsAsync(payments);
            //Then
            _paymentRepoMock.Verify(x => x.AddPaymentsAsync(payments), Times.Once);
        }
        [Test]
        public void SelectPaymentsBySeveralId_WithListId_PaymentsGotten()
        {
            //Given
            var payments = PaymentData.GetPeyments();
            var ids = new List<int>() { 1, 2, 3 };

            _paymentRepoMock.Setup(x => x.SelectPaymentsBySeveralIdAsync(ids)).Returns(payments);
            //When
            _sut.SelectPaymentsBySeveralIdAsync(ids);
            //Then
            _paymentRepoMock.Verify(x => x.SelectPaymentsBySeveralIdAsync(ids), Times.Once);
        }
        [Test]
        public void SelectPaymentsBySeveralId_WithListId_SomeIdAreAbsentInDb_EntityNotFoundExceptionThrown()
        {
            //Given
            var payments = PaymentData.GetPeyments();
            var ids = new List<int>() { 1, 4, 3 };
            _paymentRepoMock.Setup(x => x.SelectPaymentsBySeveralIdAsync(ids)).Returns(payments);
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.SelectPaymentsBySeveralIdAsync(ids));
            //Then
            Assert.That(result.Message, Is.EqualTo(ServiceMessages.EntityNotFound));
            _paymentRepoMock.Verify(x => x.SelectPaymentsBySeveralIdAsync(ids), Times.Once);
        }
        [Test]
        public void UpdatePayment_PaymentIsDeleted_EntityNotFoundExceptionThrown()
        {
            //Given
            var id = 4;
            var paymentInDb = new PaymentDto()
            {
                Id = 4,
                Date = DateTime.Now,
                IsDeleted = true,
                User = new UserDto() { Id = 1 }
            };
            var exp = ServiceMessages.PaymentDeleted;
            PaymentDto payment = PaymentData.GetPayment();
            _paymentRepoMock.Setup(x => x.GetPaymentAsync(id)).Returns(paymentInDb);
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.UpdatePaymentAsync(id, payment));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.UpdatePaymentAsync(payment), Times.Never);
        }
    }
}