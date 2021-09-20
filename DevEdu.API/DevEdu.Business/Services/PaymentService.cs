using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<PaymentDto> GetPaymentAsync(int id) => await _paymentValidationHelper.GetPaymentByIdAndThrowIfNotFoundAsync(id);

        public async Task<List<PaymentDto>> GetPaymentsByUserIdAsync(int userId)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(userId);
            return await _paymentValidationHelper.GetPaymentsByUserIdAndThrowIfNotFoundAsync(userId);
        }

        public async Task<int> AddPaymentAsync(PaymentDto dto) => await _paymentRepository.AddPaymentAsync(dto);

        public async Task DeletePaymentAsync(int id)
        {
            await _paymentValidationHelper.GetPaymentByIdAndThrowIfNotFoundAsync(id);
            await _paymentRepository.DeletePaymentAsync(id);
        }

        public async Task UpdatePaymentAsync(int id, PaymentDto dto)
        {
            var paymentInDb = await _paymentValidationHelper.GetPaymentByIdAndThrowIfNotFoundAsync(id);
            if (dto == null)
                throw new EntityNotFoundException(ServiceMessages.EntityNotFound);

            if (paymentInDb.IsDeleted)
                throw new EntityNotFoundException(ServiceMessages.PaymentDeleted);

            dto.User = new UserDto { Id = paymentInDb.User.Id };
            dto.Id = id;
            await _paymentRepository.UpdatePaymentAsync(dto);
        }

        public async Task<List<int>> AddPaymentsAsync(List<PaymentDto> payments)
        {
            return await _paymentRepository.AddPaymentsAsync(payments);
        }

        public async Task<List<PaymentDto>> SelectPaymentsBySeveralIdAsync(List<int> ids)
        {
            var list = await _paymentValidationHelper.SelectPaymentsBySeveralIdAndThrowIfNotFoundAsync(ids);
            return list;
        }
    }
}