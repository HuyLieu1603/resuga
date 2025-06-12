using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace PD_Store.Views.Payment
{
    public class PaymentResult : PageModel
    {
        private readonly ILogger<PaymentResult> _logger;

        public PaymentResult(ILogger<PaymentResult> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}