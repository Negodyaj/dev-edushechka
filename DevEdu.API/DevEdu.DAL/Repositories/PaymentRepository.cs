using Dapper;
using DevEdu.Core;
using DevEdu.DAL.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        private const string _paymentInsertProcedure = "dbo.Payment_Insert";
        private const string _paymentDeleteProcedure = "dbo.Payment_Delete";
        private const string _paymentSelectByIdProcedure = "dbo.Payment_SelectById";
        private const string _paymentSelectAllByUserIdProcedure = "dbo.Payment_SelectAllByUserId";
        private const string _paymentUpdateProcedure = "dbo.Payment_Update";
        private const string _paymentsBulkInsertProcedure = "dbo.Payment_BulkInsert";
        private const string _paymentsSelectBySeveralIdProcedure = "dbo.Payment_SelectBySeveralId";
        private const string _paymentType = "dbo.PaymentType";
        private const string _idType = "dbo.IdType";

        public PaymentRepository(IOptions<DatabaseSettings> options) : base(options)
        {
        }

        public async Task<int> AddPaymentAsync(PaymentDto paymentDto)
        {
            return await _connection.QuerySingleAsync<int>(
                _paymentInsertProcedure,
                new
                {
                    userId = paymentDto.User.Id,
                    paymentDto.Date,
                    paymentDto.Sum,
                    paymentDto.IsPaid
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeletePaymentAsync(int id)
        {
            await _connection.ExecuteAsync(
                 _paymentDeleteProcedure,
                 new { id },
                 commandType: CommandType.StoredProcedure
             );
        }

        public async Task<PaymentDto> GetPaymentAsync(int id)
        {
            PaymentDto result = default;

            return (await _connection
                .QueryAsync<PaymentDto, UserDto, PaymentDto>(
                _paymentSelectByIdProcedure,
                (payment, user) =>
                {
                    if (result == null)
                    {
                        result = payment;
                        result.User = user;
                    }
                    return result;
                },
                new { id },
                splitOn: "Id",
                    commandType: CommandType.StoredProcedure
            ))
            .FirstOrDefault();
        }

        public async Task<List<PaymentDto>> GetPaymentsByUserAsync(int userId)
        {
            return (await _connection
                .QueryAsync<PaymentDto, UserDto, PaymentDto>(
                    _paymentSelectAllByUserIdProcedure,
                    (payment, user) =>
                    {
                        payment.User = user;
                        return payment;
                    },
                    new { userId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure))
                .ToList();
        }

        public async Task UpdatePaymentAsync(PaymentDto paymentDto)
        {
            await _connection.ExecuteAsync(
                  _paymentUpdateProcedure,
                  new
                  {
                      paymentDto.Id,
                      paymentDto.Date,
                      paymentDto.Sum,
                      paymentDto.IsPaid
                  },
                  commandType: CommandType.StoredProcedure
              );
        }

        public async Task<List<int>> AddPaymentsAsync(List<PaymentDto> payments)
        {
            var table = new DataTable();
            table.Columns.Add("Date");
            table.Columns.Add("Sum");
            table.Columns.Add("UserId");
            table.Columns.Add("IsPaid");

            foreach (var bill in payments)
            {
                table.Rows.Add(bill.Date, bill.Sum, bill.User.Id, bill.IsPaid);
            }
            var response = (await _connection
                .QueryAsync<int>(
               _paymentsBulkInsertProcedure,
               new { tblPayment = table.AsTableValuedParameter(_paymentType) },
               commandType: CommandType.StoredProcedure
               ))
               .ToList();

            return response;
        }

        public async Task<List<PaymentDto>> SelectPaymentsBySeveralIdAsync(List<int> ids)
        {
            var table = new DataTable();
            table.Columns.Add("Id");

            foreach (var i in ids)
            {
                table.Rows.Add(i);
            }
            var response = (await _connection
                .QueryAsync<PaymentDto, UserDto, PaymentDto>(
              _paymentsSelectBySeveralIdProcedure,
               (paymentDto, userDto) =>
               {
                   paymentDto.User = userDto;
                   return paymentDto;
               },
               new { @tblIds = table.AsTableValuedParameter(_idType) },
               splitOn: "Id",
               commandType: CommandType.StoredProcedure
                ))
                .ToList();

            return response;
        }
    }
}