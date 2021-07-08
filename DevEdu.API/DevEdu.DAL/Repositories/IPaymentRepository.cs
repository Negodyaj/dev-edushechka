using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IPaymentRepository
    {
        int AddPayment(PaymentDto paymentDto);
        void DeletePayment(int id);
        PaymentDto GetPayment(int id);
        List<PaymentDto> GetPaymentsByUser(int userId);
        void UpdatePayment(PaymentDto paymentDto);
    }
}