using System.Collections.Generic;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.Business.Servicies;
using DevEdu.DAL.Models;
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
        public PaymentDto GetPayment(int id)
        {
            return _paymentService.GetPayment(id);
        }

        //  api/payment/by-user/1
        [HttpGet("by-user/{userId}")]
        public List<PaymentDto> GetPaymentByUserId(int userId)
        {
            return _paymentService.GetPaymentByUserId(userId);
        }

        //  api/payment
        [HttpPost]
        public int AddPayment([FromBody] PaymentInputModel model)
        {
            var dto = _mapper.Map<PaymentDto>(model);
            return _paymentService.AddPayment(dto);
        }

        //  api/payment/5
        [HttpDelete("{id}")]
        public void DeletePayment(int id)
        {
            _paymentService.DeletePayment(id);
        }

        //  api/payment/5
        [HttpPut("{id}")]
        public string UpdatePayment(int id, [FromBody] PaymentInputModel model)
        {
            var dto = _mapper.Map<PaymentDto>(model);
            _paymentService.UpdatePayment(id, dto);
            return $"Text";
        }
    }
}

