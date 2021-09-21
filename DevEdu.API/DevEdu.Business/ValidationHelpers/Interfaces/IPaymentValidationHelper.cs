using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IPaymentValidationHelper
    {
        Task<PaymentDto> GetPaymentByIdAndThrowIfNotFoundAsync(int paymentId);
        Task<List<PaymentDto>> GetPaymentsByUserIdAndThrowIfNotFoundAsync(int userId);
        Task<List<PaymentDto>> SelectPaymentsBySeveralIdAndThrowIfNotFoundAsync(List<int> ids);
    }
}