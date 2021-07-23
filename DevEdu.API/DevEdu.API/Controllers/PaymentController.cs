using System.Collections.Generic;
using System.ComponentModel;
using AutoMapper;
using DevEdu.API.Models.InputModels;
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
        [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
        [Description("Get payment by id")]
        public PaymentDto GetPayment(int id)
        {
            return _paymentService.GetPayment(id);
        }

        //  api/payment/user/1
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(List<PaymentDto>), StatusCodes.Status200OK)]
        [Description("Get all payments by user id")]
        public List<PaymentDto> SelectAllPaymentsByUserId(int userId)
        {
            return _paymentService.GetPaymentsByUserId(userId);
        }

        //  api/payment
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [Description("Add one payment")]
        public int AddPayment([FromBody] PaymentInputModel model)
        {
            var dto = _mapper.Map<PaymentDto>(model);
            return _paymentService.AddPayment(dto);
        }

        //  api/payment/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [Description("Delete payment by id")]
        public string DeletePayment(int id)
        {
            _paymentService.DeletePayment(id);
            return $"deleted payment id: {id}";
        }

        //  api/payment/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [Description("Update payment by id")]
        public string UpdatePayment(int id, [FromBody] PaymentInputModel model)
        {
            var dto = _mapper.Map<PaymentDto>(model);
            _paymentService.UpdatePayment(id, dto);
            return $"Text";
        }
        //  api/payment/bulk
        [HttpPost("bulk")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [Description("Add payments")]
        public string AddPayments([FromBody] List<PaymentInputModel> models)
        {
            var dto = _mapper.Map<List<PaymentDto>>(models);
            _paymentService.AddPayments(dto);
            return "Payments added";
        }
    }
}