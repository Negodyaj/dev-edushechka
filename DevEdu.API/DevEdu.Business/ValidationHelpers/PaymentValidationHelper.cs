using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.ValidationHelpers
{
    public class PaymentValidationHelper : IPaymentValidationHelper
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentValidationHelper(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public void CheckPaymentExistence(int paymentId)
        {
            var payment = _paymentRepository.GetPayment(paymentId);
            if (payment == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(payment), paymentId));
        }
    }
}