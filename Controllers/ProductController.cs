using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PD_Store.Helper;
using PD_Store.Repositories.Product;
using PD_Store.ViewModels.Product;

namespace PD_Store.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _productService.GetListProduct();
            if (result.Status == Contants.StatusCodeSuccessed)
            {
                return View(result.Data);
            }
            else
            {
                // Handle error case
                ViewBag.ErrorMessage = result.Message;
                return View(new List<Models.Product.Products>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _productService.GetProductById(id);
            if (result.Status == Contants.StatusCodeSuccessed)
            {
                return View(result.Data);
            }
            else
            {
                // Handle error case
                ViewBag.ErrorMessage = result.Message;
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetListProduct()
        {
            return Ok(await _productService.GetListProduct());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductVM request)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProduct(request);
                if (result.Status == Contants.StatusCodeSuccessed)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = result.Message;
                }
            }
            return View(request);
        }

    }
}