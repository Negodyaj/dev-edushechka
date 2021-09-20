using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public class PaymentValidationHelper : IPaymentValidationHelper
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentValidationHelper(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<PaymentDto> GetPaymentByIdAndThrowIfNotFoundAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetPaymentAsync(paymentId);
            if (payment == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(payment), paymentId));
            
            if (payment.IsDeleted)
                throw new EntityNotFoundException(ServiceMessages.PaymentDeleted);
            
            return payment;
        }

        public async Task<List<PaymentDto>> GetPaymentsByUserIdAndThrowIfNotFoundAsync(int userId)
        {
            var payments = await _paymentRepository.GetPaymentsByUserAsync(userId);
            if (payments == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundByUserId, nameof(payments), userId));
            
            return payments;
        }

        public async Task<List<PaymentDto>> SelectPaymentsBySeveralIdAndThrowIfNotFoundAsync(List<int> ids)
        {
            var payments = await _paymentRepository.SelectPaymentsBySeveralIdAsync(ids);
            CheckPaymentsExistence(payments, ids);
            if (payments == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFound));
           
            return payments;
        }

        public void CheckPaymentsExistence(List<PaymentDto> payments, List<int> ids)
        {
            var arePaymentsInDataBase = ids.All(d => payments.Any(t => t.Id == d));

            if (!arePaymentsInDataBase)
            {
                throw new EntityNotFoundException(ServiceMessages.EntityNotFound);
            }
        }
    }
}