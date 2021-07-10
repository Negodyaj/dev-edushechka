using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Servicies
{
    public interface IPaymentService
    {
        int AddPayment(PaymentDto dto);
        void DeletePayment(int id);
        PaymentDto GetPayment(int id);
        List<PaymentDto> GetPaymentByUserId(int userId);
        void UpdatePayment(int id, PaymentDto dto);
    }
}