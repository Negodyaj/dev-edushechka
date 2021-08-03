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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _paymentRepoMock.Setup(x => x.GetPayment(paymentId)).Returns(new PaymentDto() { Id = paymentId });
            //When
            _sut.GetPayment(paymentId);
            //Then
            _paymentRepoMock.Verify(x => x.GetPayment(paymentId), Times.Once);
        }
        [Test]
        public void GetPayment_ById_NotValidPaymentRequestId_EntityNotFoundExceptionThrown()
        {
            //Given
            var paymentId = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "payment", paymentId);
            _paymentRepoMock.Setup(x => x.GetPayment(paymentId));
            _paymentValidationHelperMock.Setup(x => x.GetPaymentByIdAndThrowIfNotFound(paymentId));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.GetPayment(paymentId));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.GetPayment(paymentId), Times.Once);
        }
        [Test]
        public void GetPaymentsByUserId_ValidUserId_PaymentsGotten()
        {
            //Given
            var userId = 2;
            var paymentsInDB = PaymentData.GetPeyments();
            _userRepositoryMock.Setup(x => x.SelectUserById(userId)).Returns(new UserDto() { Id = userId });
            _paymentRepoMock.Setup(x => x.GetPaymentsByUser(userId)).Returns(paymentsInDB);
            //When
            _sut.GetPaymentsByUserId(userId);
            //Then
            _paymentRepoMock.Verify(x => x.GetPaymentsByUser(userId), Times.Once);
        }
        [Test]
        public void GetPaymentsByUserId_NotValidUserId_EntityNotFoundExceptionThrown()
        {
            //Given
            var userId = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "user", userId);
            _userRepositoryMock.Setup(x => x.SelectUserById(userId));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.GetPaymentsByUserId(userId));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.GetPaymentsByUser(userId), Times.Never);
        }
        [Test]
        public void GetPaymentsByUserId_ValidUserIdEntityInDataBaseIsAbssent_EntityNotFoundExceptionThrown()
        {
            //Given
            var userId = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundByUserId, "payments", userId);
            _userRepositoryMock.Setup(x => x.SelectUserById(userId)).Returns(new UserDto() { Id = userId });
            _paymentRepoMock.Setup(x => x.GetPaymentsByUser(userId));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.GetPaymentsByUserId(userId));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.GetPaymentsByUser(userId), Times.Once);
        }
        [Test]
        public void AddPayment_WithSimpleDto_PaymentWasAdded()
        {
            //Given
            var payment = PaymentData.GetPayment();
            //When
            _sut.AddPayment(payment);
            //Then
            _paymentRepoMock.Verify(x => x.AddPayment(payment), Times.Once);
        }
        [Test]
        public void DeletePayment_ValidPaymentId_PaymentDeleted()
        {
            //Given
            var id = 2;
            _paymentRepoMock.Setup(x => x.GetPayment(id)).Returns(new PaymentDto() { Id = id });
            //When
            _sut.DeletePayment(id);
            //Then
            _paymentRepoMock.Verify(x => x.DeletePayment(id), Times.Once);
        }
        [Test]
        public void DeletePayment_NotValidPaymentId_PaymentDeleted()
        {
            //Given
            var id = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "payment", id);
            _paymentRepoMock.Setup(x => x.GetPayment(id));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.DeletePayment(id));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.DeletePayment(id), Times.Never);
        }
        [Test]
        public void UpdatePayment_ValidPaymentId_PaymentWasUpdated()
        {
            //Given
            var id = 2;
            var payment = PaymentData.GetPayment();
            _paymentRepoMock.Setup(x => x.GetPayment(id)).Returns(payment);
            //When
            _sut.UpdatePayment(id, payment);
            //Then
            _paymentRepoMock.Verify(x => x.UpdatePayment(payment), Times.Once);
        }
        [Test]
        public void UpdatePayment_NotValidPaymentId_EntityNotFoundExceptionThrown()
        {
            //Given
            var id = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "payment", id);
            var payment = PaymentData.GetPayment();
            _paymentRepoMock.Setup(x => x.GetPayment(id));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.UpdatePayment(id, payment));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.UpdatePayment(payment), Times.Never);
        }
        [Test]
        public void UpdatePayment_WithEmptyDto_EntityNotFoundExceptionThrown()
        {
            //Given
            var id = 4;
            var paymentInDb = PaymentData.GetPayment();
            var exp = ServiceMessages.EntityNotFound;
            PaymentDto payment = default;
            _paymentRepoMock.Setup(x => x.GetPayment(id)).Returns(paymentInDb);
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.UpdatePayment(id, payment));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.UpdatePayment(payment), Times.Never);
        }
        [Test]
        public void AddPayments_WithListPaymentDto_PaymentsWereAdded()
        {
            //Given
            var payments = PaymentData.GetPeyments();
            //When
            _sut.AddPayments(payments);
            //Then
            _paymentRepoMock.Verify(x => x.AddPayments(payments), Times.Once);
        }
        [Test]
        public void SelectPaymentsBySeveralId_WithListId_PaymentsGotten()
        {
            //Given
            var payments = PaymentData.GetPeyments();
            var ids = new List<int>() { 1, 2, 3 };
            
            _paymentRepoMock.Setup(x => x.SelectPaymentsBySeveralId(ids)).Returns(payments);
            //When
            _sut.SelectPaymentsBySeveralId(ids);
            //Then
            _paymentRepoMock.Verify(x => x.SelectPaymentsBySeveralId(ids), Times.Once);
        }
        [Test]
        public void SelectPaymentsBySeveralId_WithListId_SomeIdAreAbsentInDb_EntityNotFoundExceptionThrown()
        {
            //Given
            var payments = PaymentData.GetPeyments();
            var ids = new List<int>() { 1, 4, 3 };
            _paymentRepoMock.Setup(x => x.SelectPaymentsBySeveralId(ids)).Returns(payments);
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.SelectPaymentsBySeveralId(ids));
            //Then
            Assert.That(result.Message, Is.EqualTo(ServiceMessages.EntityNotFound));
            _paymentRepoMock.Verify(x => x.SelectPaymentsBySeveralId(ids), Times.Once);
        }
     }
}