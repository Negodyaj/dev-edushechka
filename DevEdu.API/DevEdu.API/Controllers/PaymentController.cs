﻿using System.Collections.Generic;
using System.ComponentModel;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels.Payment;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public PaymentController(IMapper mapper, IPaymentService paymentService)
        {
            _mapper = mapper;
            _paymentService = paymentService;
        }
        //  api/payment/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PaymentOutputModel), StatusCodes.Status200OK)]
        [Description("Get payment by id")]
        public PaymentOutputModel GetPayment(int id)
        {
            var payment = _paymentService.GetPayment(id);
            return _mapper.Map<PaymentOutputModel>(payment);
        }

        //  api/payment/user/1
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(List<PaymentOutputModel>), StatusCodes.Status200OK)]
        [Description("Get all payments by user id")]
        public List<PaymentOutputModel> SelectAllPaymentsByUserId(int userId)
        {
            var payment = _paymentService.GetPaymentsByUserId(userId);
            return _mapper.Map<List<PaymentOutputModel>>(payment);
        }

        //  api/payment
        [HttpPost]
        [ProducesResponseType(typeof(PaymentOutputModel), StatusCodes.Status201Created)]
        [Description("Add one payment")]
        public PaymentOutputModel AddPayment([FromBody] PaymentInputModel model)
        {
            var dto = _mapper.Map<PaymentDto>(model);
            int id = _paymentService.AddPayment(dto);
            dto = _paymentService.GetPayment(id);
            
            return _mapper.Map<PaymentOutputModel>(dto);
        }

        //  api/payment/5
        [HttpDelete("{id}")]
        [ProducesResponseType (StatusCodes.Status204NoContent)]
        [Description("Delete payment by id")]
        public void DeletePayment(int id)
        {
            _paymentService.DeletePayment(id);
        }

        //  api/payment/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PaymentOutputModel), StatusCodes.Status200OK)]
        [Description("Update payment by id")]
        public PaymentOutputModel UpdatePayment(int id, [FromBody] PaymentUpdateInputModel model)
        {
            var dto = _mapper.Map<PaymentDto>(model);
            _paymentService.UpdatePayment(id, dto);
            dto = _paymentService.GetPayment(id);
            return _mapper.Map<PaymentOutputModel>(dto);
        }
        //  api/payment/bulk
        [HttpPost("bulk")]
        [ProducesResponseType(typeof(List<PaymentOutputModel>), StatusCodes.Status200OK)]
        [Description("Add payments")]
        public List<PaymentOutputModel> AddPayments([FromBody] List<PaymentInputModel> models)
        {
            var dto = _mapper.Map<List<PaymentDto>>(models);
            var listId = _paymentService.AddPayments(dto);
            dto = _paymentService.SelectPaymentsBySeveralId(listId);
            return _mapper.Map<List<PaymentOutputModel>>(dto);
         }
    }
}