using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace DevEdu.Business.ValidationHelpers
{
    public class PaymentValidationHelper : IPaymentValidationHelper
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentValidationHelper(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public PaymentDto GetPaymentByIdAndThrowIfNotFound(int paymentId)
        {
            var payment = _paymentRepository.GetPayment(paymentId);
            if (payment == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(payment), paymentId));
            if (payment.IsDeleted)
                throw new EntityNotFoundException(ServiceMessages.PaymentDeletedMessage);
            return payment;
        }
        public List<PaymentDto> GetPaymentsByUserIdAndThrowIfNotFound(int userId)
        {
            var payments = _paymentRepository.GetPaymentsByUser(userId);
            if (payments == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundByUserIdMessage, nameof(payments), userId));
            return payments;
        }
        public List<PaymentDto> SelectPaymentsBySeveralIdAndThrowIfNotFound(List<int> ids)
        {
            var payments = _paymentRepository.SelectPaymentsBySeveralId(ids);
            CheckPaymentsExistence(payments, ids);
            if (payments == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage));
            return payments;
        }
        public void CheckPaymentsExistence(List<PaymentDto> payments, List<int> ids)
        {
            var arePaymentsInDataBase = ids.All(d => payments.Any(t => t.Id == d));

            if (!arePaymentsInDataBase)
            {
                throw new EntityNotFoundException(ServiceMessages.EntityNotFoundMessage);
            }
        }
    }
}