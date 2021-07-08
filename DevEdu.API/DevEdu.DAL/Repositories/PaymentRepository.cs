
using Dapper;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DevEdu.DAL.Repositories
{
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        private const string _paymentAddProcedure = "dbo.Payment_Insert";
        private const string _paymentDeleteProcedure = "dbo.Payment_Delete";
        private const string _paymentSelectByIdProcedure = "dbo.Payment_SelectById";
        private const string _paymentAllByUserIdProcedure = "dbo.Payment_Insert";
        private const string _paymentUpdateProcedure = "dbo.Payment_Update";

        public PaymentRepository() { }

        public int AddPayment(PaymentDto paymentDto)
        {
            return _connection.QuerySingle<int>(
                _paymentAddProcedure,
                new
                {
                    paymentDto.Data,
                    paymentDto.Summ,
                    paymentDto.UserId,
                    paymentDto.IsPaid,
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeletePayment(int id)
        {
            _connection.Execute(
                _paymentDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public PaymentDto GetPayment(int id)
        {
            return _connection.QuerySingleOrDefault<PaymentDto>(
                _paymentSelectByIdProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<PaymentDto> GetPaymentsByUser(int userId)
        {
            return _connection
                .Query<PaymentDto>(
                    _paymentAllByUserIdProcedure,
                    new { userId },
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public void UpdatePayment(PaymentDto paymentDto)
        {
            _connection.Execute(
                _paymentUpdateProcedure,
                new
                {
                    paymentDto.Data,
                    paymentDto.Summ,
                    paymentDto.UserId,
                    paymentDto.IsPaid
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
