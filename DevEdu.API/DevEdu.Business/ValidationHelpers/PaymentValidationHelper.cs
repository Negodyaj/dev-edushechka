using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

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
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(payment), paymentId));
            return payment;
        }
        public List<PaymentDto> GetPaymentsByUserIdAndThrowIfNotFound(int userId)
        {
            var payments = _paymentRepository.GetPaymentsByUser(userId);
            if(payments == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundByUserId, nameof(payments), userId));
            return payments;
        }
        public List<PaymentDto> SelectPaymentsBySeveralIdAndThrowIfNotFound(List<int> ids)
        {
            var payments = _paymentRepository.SelectPaymentsBySeveralId(ids);
            if (payments == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFound));
            return payments;
        }
    }
}