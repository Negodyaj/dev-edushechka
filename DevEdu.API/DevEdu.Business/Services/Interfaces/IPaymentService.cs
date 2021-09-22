using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface IPaymentService
    {
        Task<int> AddPaymentAsync(PaymentDto dto);
        Task DeletePaymentAsync(int id);
        Task<PaymentDto> GetPaymentAsync(int id);
        Task<List<PaymentDto>> GetPaymentsByUserIdAsync(int userId);
        Task UpdatePaymentAsync(int id, PaymentDto dto);
        Task<List<int>> AddPaymentsAsync(List<PaymentDto> payments);
        Task<List<PaymentDto>> SelectPaymentsBySeveralIdAsync(List<int> ids);
    }
}