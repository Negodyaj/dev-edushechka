using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentValidationHelper _paymentValidationHelper;
        private readonly IUserValidationHelper _userValidationHelper;

        public PaymentService(IPaymentRepository paymentRepository,
                              IPaymentValidationHelper paymentValidationHelper,
                              IUserValidationHelper userValidationHelper)
        {
            _paymentRepository = paymentRepository;
            _paymentValidationHelper = paymentValidationHelper;
            _userValidationHelper = userValidationHelper;
        }

        public PaymentDto GetPayment(int id) => _paymentValidationHelper.GetPaymentByIdAndThrowIfNotFound(id);

        public List<PaymentDto> GetPaymentsByUserId(int userId)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userId);
            return _paymentValidationHelper.GetPaymentsByUserIdAndThrowIfNotFound(userId);
        }

        public int AddPayment(PaymentDto dto) => _paymentRepository.AddPayment(dto);

        public void DeletePayment(int id)
        {
            _paymentValidationHelper.GetPaymentByIdAndThrowIfNotFound(id);
            _paymentRepository.DeletePayment(id);
        }

        public void UpdatePayment(int id, PaymentDto dto)
        {
            var paymentInDb = _paymentValidationHelper.GetPaymentByIdAndThrowIfNotFound(id);
            if (dto == null)
                throw new EntityNotFoundException(ServiceMessages.EntityNotFound);
            dto.User = new UserDto { Id = paymentInDb.User.Id };
            dto.Id = id;
            _paymentRepository.UpdatePayment(dto);
        }

        public List<int> AddPayments(List<PaymentDto> payments)
        {
            return _paymentRepository.AddPayments(payments);
        }

        public List<PaymentDto> SelectPaymentsBySeveralId(List<int> ids)
        {
            var list = _paymentValidationHelper.SelectPaymentsBySeveralIdAndThrowIfNotFound(ids);
            return list;
        }
    }
}