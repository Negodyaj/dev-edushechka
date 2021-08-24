using System;
using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Models;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using DevEdu.API.Configuration.ExceptionResponses;

namespace DevEdu.API.Controllers
{
    [AuthorizeRoles(Role.Manager)]
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
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [Description("Get payment by id")]
        public PaymentOutputModel GetPayment(int id)
        {
            var dto = _paymentService.GetPayment(id);
            return _mapper.Map<PaymentOutputModel>(dto);
        }

        //  api/payment/user/1
        [AuthorizeRoles(Role.Student)]
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(List<PaymentOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [Description("Get all payments by user id")]
        public List<PaymentOutputModel> SelectAllPaymentsByUserId(int userId)
        {
            var list = _paymentService.GetPaymentsByUserId(userId);
            return _mapper.Map<List<PaymentOutputModel>>(list);
        }

        //  api/payment
        [HttpPost]
        [ProducesResponseType(typeof(PaymentOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [Description("Add one payment")]
        public ActionResult<PaymentOutputModel> AddPayment([FromBody] PaymentInputModel model)
        {
            var dto = _mapper.Map<PaymentDto>(model);
            var id = _paymentService.AddPayment(dto);
            dto.Id = id;
            var output = _mapper.Map<PaymentOutputModel>(dto);
            return Created(new Uri("", UriKind.Relative), output);
        }

        //  api/payment/5
        [HttpDelete("{id}")]
        [ProducesResponseType (StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [Description("Delete payment by id")]
        public ActionResult DeletePayment(int id)
        {
            _paymentService.DeletePayment(id);
            return NoContent();
        }

        //  api/payment/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PaymentOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
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
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
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