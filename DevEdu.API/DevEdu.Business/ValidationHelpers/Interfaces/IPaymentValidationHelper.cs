using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IPaymentValidationHelper
    {
        PaymentDto GetPaymentByIdAndThrowIfNotFound(int paymentId);
    }
}