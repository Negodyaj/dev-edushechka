using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IPaymentValidationHelper
    {
        PaymentDto GetPaymentByIdAndThrowIfNotFound(int paymentId);
        List<PaymentDto> GetPaymentsByUserIdAndThrowIfNotFound(int userId);
        List<PaymentDto> SelectPaymentsBySeveralIdAndThrowIfNotFound(List<int> ids);
    }
}