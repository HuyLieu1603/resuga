using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PD_Store.Helper;

namespace PD_Store.Controllers
{
    public class PaymentController : Controller
    {


        private readonly IConfiguration _configuration;

        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IConfiguration configuration, ILogger<PaymentController> logger)
        {
            _logger = logger;
            _configuration = configuration;
        }
        [HttpPost]
        public IActionResult CreateVNPayUrl(decimal amount, string orderInfo)
        {
            try
            {
                var vnPayConfig = _configuration.GetSection("VnPay");
                string vnp_ReturnUrl = vnPayConfig["ReturnUrl"];
                string vnp_Url = vnPayConfig["Url"];
                string vnp_TmnCode = vnPayConfig["TmnCode"];
                string vnp_HashSecret = vnPayConfig["HashSecret"];

                var vnpay = new VnPayLibrary();

                vnpay.AddRequestData("vnp_Version", "2.1.0");
                vnpay.AddRequestData("vnp_Command", "pay");
                vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                vnpay.AddRequestData("vnp_Amount", ((long)amount * 100).ToString());
                vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", "VND");
                var ipAddress = HttpContext.Connection?.RemoteIpAddress?.ToString();
                if (string.IsNullOrEmpty(ipAddress))
                    ipAddress = "192.168.50.188"; // fallback local
                vnpay.AddRequestData("vnp_IpAddr", ipAddress);
                vnpay.AddRequestData("vnp_Locale", "vn");
                vnpay.AddRequestData("vnp_OrderInfo", orderInfo);
                vnpay.AddRequestData("vnp_OrderType", "other");
                vnpay.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
                vnpay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());

                string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
                return Redirect(paymentUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating VNPay URL");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public IActionResult Callback()
        {
            var vnpay = new VnPayHelper();
            var queryCollection = Request.Query;
            string hashSecret = _configuration["VnPay:HashSecret"];

            bool isValid = vnpay.ValidateSignature(queryCollection, hashSecret);

            string responseCode = queryCollection["vnp_ResponseCode"];
            string orderId = queryCollection["vnp_TxnRef"];
            string amount = queryCollection["vnp_Amount"];

            ViewBag.IsSuccess = isValid && responseCode == "00";
            ViewBag.OrderId = orderId;
            ViewBag.Amount = long.Parse(amount) / 100;
            ViewBag.Message = responseCode == "00" ? "Giao dịch thành công" : "Giao dịch thất bại";

            return View("PaymentResult");
        }
    }
}