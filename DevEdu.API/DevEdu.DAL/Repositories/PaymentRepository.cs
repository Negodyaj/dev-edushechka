
using Dapper;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevEdu.DAL.Enums;


namespace DevEdu.DAL.Repositories
{
    public class PaymentRepository : BaseRepository, IPaymentRepository     
    {
        private const string _paymentAddProcedure = "dbo.Payment_Insert";
        private const string _paymentDeleteProcedure = "dbo.Payment_Delete";
        private const string _paymentSelectByIdProcedure = "dbo.Payment_SelectById";
        private const string _paymentAllByUserIdProcedure = "dbo.Payment_SelectAllByUserId";
        private const string _paymentUpdateProcedure = "dbo.Payment_Update";

        public PaymentRepository() { }

        public int AddPayment(PaymentDto paymentDto)
        {
            return _connection.QuerySingle<int>(
                _paymentAddProcedure,
                new
                {
                    userId=paymentDto.User.Id,
                    paymentDto.Date,
                    paymentDto.Sum,
                    paymentDto.IsPaid
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
            PaymentDto result = default;
            return _connection
                .Query<PaymentDto, UserDto, PaymentDto>(
                _paymentSelectByIdProcedure,
                (payment, user) =>
                {
                    if (result == null)
                    {

                        result = payment;
                        result.User = user;
                    }
                    return result;
                }, new { id},
                splitOn:"Name",
                    commandType: CommandType.StoredProcedure
            )
            .FirstOrDefault();
        }

        public List<PaymentDto> GetPaymentsByUser(int userId)
        {
            var paymentDictionary = new Dictionary<int, PaymentDto>();

            return _connection
                .Query<PaymentDto, UserDto, PaymentDto>(
                    _paymentAllByUserIdProcedure,
                    (payment, user) =>
                    {
                        PaymentDto result;

                        if (!paymentDictionary.TryGetValue(payment.Id, out result))
                        {
                            result = payment;
                            result.User = user;
                            paymentDictionary.Add(payment.Id, result);
                        }

                        return result;
                    },
                    new { userId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .Distinct()
                .ToList();
        }

        public void UpdatePayment(PaymentDto paymentDto)
        {
            _connection.Execute(
                _paymentUpdateProcedure,
                new
                {
                    paymentDto.Date,
                    paymentDto.Sum,
                    paymentDto.User,
                    paymentDto.IsPaid
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
