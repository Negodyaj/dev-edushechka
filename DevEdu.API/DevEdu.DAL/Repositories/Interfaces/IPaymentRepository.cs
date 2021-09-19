using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface IPaymentRepository
    {
        Task<int> AddPaymentAsync(PaymentDto paymentDto);
        Task DeletePaymentAsync(int id);
        Task<PaymentDto> GetPaymentAsync(int id);
        Task<List<PaymentDto>> GetPaymentsByUserAsync(int userId);
        Task UpdatePaymentAsync(PaymentDto paymentDto);
        Task<List<int>> AddPaymentsAsync(List<PaymentDto> payments);
        Task<List<PaymentDto>> SelectPaymentsBySeveralIdAsync(List<int> ids);
    }
}