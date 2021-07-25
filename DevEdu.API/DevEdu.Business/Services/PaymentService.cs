using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public PaymentDto GetPayment(int id) => _paymentRepository.GetPayment(id);

        public List<PaymentDto> GetPaymentsByUserId(int userId) => _paymentRepository.GetPaymentsByUser(userId);

        public int AddPayment(PaymentDto dto) => _paymentRepository.AddPayment(dto);

        public void DeletePayment(int id) => _paymentRepository.DeletePayment(id);

        public void UpdatePayment(int id, PaymentDto dto)
        {
            var paymentInDb = _paymentRepository.GetPayment(id);
            if(paymentInDb.IsDeleted == true)  
            throw new EntityNotFoundException("This payment is deleted");
            dto.User = new UserDto { Id = paymentInDb.User.Id };
            
            dto.Id = id;
            _paymentRepository.UpdatePayment(dto);
        }
        public void AddPayments(List<PaymentDto> payments)
        {
            _paymentRepository.AddPayments(payments);
        }
    }
}