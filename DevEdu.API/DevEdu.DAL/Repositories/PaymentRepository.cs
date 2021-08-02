﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        private const string _paymentAddProcedure = "dbo.Payment_Insert";
        private const string _paymentDeleteProcedure = "dbo.Payment_Delete";
        private const string _paymentSelectByIdProcedure = "dbo.Payment_SelectById";
        private const string _paymentAllByUserIdProcedure = "dbo.Payment_SelectAllByUserId";
        private const string _paymentUpdateProcedure = "dbo.Payment_Update";
        private const string _addPaymentsProcedure = "[dbo].[Payment_BulkInsert]";
        private const string _selectPaymentsBySeveralId = "[dbo].[Payment_SelectBySeveralId]";
        private const string _paymentType = "[dbo].[PaymentType]";
        private const string _idType = "[dbo].[IdType]";

        public PaymentRepository() { }

        public int AddPayment(PaymentDto paymentDto)
        {
            return _connection.QuerySingle<int>(
                _paymentAddProcedure,
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
                },
                new { id },
                splitOn: "Id",
                    commandType: CommandType.StoredProcedure
            )
            .FirstOrDefault();
        }

        public List<PaymentDto> GetPaymentsByUser(int userId)
        {
            return _connection
                .Query<PaymentDto, UserDto, PaymentDto>(
                    _paymentAllByUserIdProcedure,
                    (payment, user) =>
                    {
                        payment.User = user;
                        return payment;
                    },
                    new { userId },
                    splitOn: "Id",
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
                    paymentDto.Id,
                    paymentDto.Date,
                    paymentDto.Sum,
                    paymentDto.IsPaid
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public List<int> AddPayments(List<PaymentDto> payments)
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
            var response = _connection.Query<int>(
               _addPaymentsProcedure,
               new { tblPayment = table.AsTableValuedParameter(_paymentType) },
               commandType: CommandType.StoredProcedure
               ).ToList();
            return response;
        }
        public List<PaymentDto> SelectPaymentsBySeveralId(List<int> ids)
        {
            var table = new DataTable();
            table.Columns.Add("Id");
            foreach (var i in ids)
            {
                table.Rows.Add(i);
            }
            var response = _connection.Query<PaymentDto, UserDto, PaymentDto>(
              _selectPaymentsBySeveralId,
               (paymentDto, userDto) =>
               {
                   paymentDto.User = userDto;
                   return paymentDto;
               },
               new { @tblIds = table.AsTableValuedParameter(_idType) },
               splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .ToList();

            return response;
        }
    }
}