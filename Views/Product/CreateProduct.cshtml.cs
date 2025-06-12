using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace PD_Store.Views.Product
{
    public class CreateProduct : PageModel
    {
        private readonly ILogger<CreateProduct> _logger;

        public CreateProduct(ILogger<CreateProduct> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}