using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IPaymentService
    {
        int AddPayment(PaymentDto dto);
        void DeletePayment(int id);
        PaymentDto GetPayment(int id);
        List<PaymentDto> GetPaymentsByUserId(int userId);
        void UpdatePayment(int id, PaymentDto dto);
        void AddPayments(List<PaymentDto> payments);
        List<PaymentDto> SelectPaymentsBySeveralId(List<int> ids);
    }
}