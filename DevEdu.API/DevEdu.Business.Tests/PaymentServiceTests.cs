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
        public async Task GetPayment_ById_ValidPaymentRequestId_GotPaymentAsync()
        {
            //Given
            var paymentId = 2;
            _paymentRepoMock.Setup(x => x.GetPaymentAsync(paymentId)).ReturnsAsync(new PaymentDto() { Id = paymentId });

            //When
            await _sut.GetPaymentAsync(paymentId);

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
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetPaymentAsync(paymentId));

            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.GetPaymentAsync(paymentId), Times.Once);
        }
        [Test]
        public async Task GetPaymentsByUserId_ValidUserId_PaymentsGottenAsync()
        {
            //Given
            var userId = 2;
            var paymentsInDB = PaymentData.GetPeyments();
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(new UserDto() { Id = userId });
            _paymentRepoMock.Setup(x => x.GetPaymentsByUserAsync(userId)).ReturnsAsync(paymentsInDB);

            //When
            await _sut.GetPaymentsByUserIdAsync(userId);

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
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetPaymentsByUserIdAsync(userId));

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
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(new UserDto() { Id = userId });
            _paymentRepoMock.Setup(x => x.GetPaymentsByUserAsync(userId));

            //When
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetPaymentsByUserIdAsync(userId));

            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.GetPaymentsByUserAsync(userId), Times.Once);
        }
        [Test]
        public async Task AddPayment_WithSimpleDto_PaymentWasAddedAsync()
        {
            //Given
            var payment = PaymentData.GetPayment();

            //When
            await _sut.AddPaymentAsync(payment);

            //Then
            _paymentRepoMock.Verify(x => x.AddPaymentAsync(payment), Times.Once);
        }
        [Test]
        public async Task DeletePayment_ValidPaymentId_PaymentDeletedAsync()
        {
            //Given
            var id = 2;
            _paymentRepoMock.Setup(x => x.GetPaymentAsync(id)).ReturnsAsync(new PaymentDto() { Id = id });

            //When
            await _sut.DeletePaymentAsync(id);

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
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.DeletePaymentAsync(id));

            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.DeletePaymentAsync(id), Times.Never);
        }
        [Test]
        public async Task UpdatePayment_ValidPaymentId_PaymentWasUpdatedAsync()
        {
            //Given
            var id = 2;
            var payment = PaymentData.GetPayment();
            _paymentRepoMock.Setup(x => x.GetPaymentAsync(id)).ReturnsAsync(payment);

            //When
            await _sut.UpdatePaymentAsync(id, payment);

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
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.UpdatePaymentAsync(id, payment));
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
            _paymentRepoMock.Setup(x => x.GetPaymentAsync(id)).ReturnsAsync(paymentInDb);

            //When
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.UpdatePaymentAsync(id, payment));

            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.UpdatePaymentAsync(payment), Times.Never);
        }
        [Test]
        public async Task AddPayments_WithListPaymentDto_PaymentsWereAddedAsync()
        {
            //Given
            var payments = PaymentData.GetPeyments();

            //When
            await _sut.AddPaymentsAsync(payments);

            //Then
            _paymentRepoMock.Verify(x => x.AddPaymentsAsync(payments), Times.Once);
        }
        [Test]
        public async Task SelectPaymentsBySeveralId_WithListId_PaymentsGottenAsync()
        {
            //Given
            var payments = PaymentData.GetPeyments();
            var ids = new List<int>() { 1, 2, 3 };

            _paymentRepoMock.Setup(x => x.SelectPaymentsBySeveralIdAsync(ids)).ReturnsAsync(payments);

            //When
            await _sut.SelectPaymentsBySeveralIdAsync(ids);

            //Then
            _paymentRepoMock.Verify(x => x.SelectPaymentsBySeveralIdAsync(ids), Times.Once);
        }
        [Test]
        public void SelectPaymentsBySeveralId_WithListId_SomeIdAreAbsentInDb_EntityNotFoundExceptionThrown()
        {
            //Given
            var payments = PaymentData.GetPeyments();
            var ids = new List<int>() { 1, 4, 3 };
            _paymentRepoMock.Setup(x => x.SelectPaymentsBySeveralIdAsync(ids)).ReturnsAsync(payments);
            //When
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.SelectPaymentsBySeveralIdAsync(ids));
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
            _paymentRepoMock.Setup(x => x.GetPaymentAsync(id)).ReturnsAsync(paymentInDb);
            //When
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.UpdatePaymentAsync(id, payment));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _paymentRepoMock.Verify(x => x.UpdatePaymentAsync(payment), Times.Never);
        }
    }
}