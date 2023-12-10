using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _service;

        public ProductController(IProductService productService)
        {
            _service = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [Authorize]
        public async Task<IActionResult> ProductIndex()
        {
            var products = await _service.FindAllProducts();
            return View(products);
        }
         [Authorize]
        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var response = await _service.CreateProduct(model);

            if (response != null)
                return RedirectToAction(nameof(ProductIndex));

            return View(model);
        }

        public async Task<IActionResult> ProductUpdate(int id)
        {
            var product = await _service.FindProductByID(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ProductUpdate(ProductModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var response = await _service.UpdateProduct(model);

            if (response != null)
                return RedirectToAction(nameof(ProductIndex));

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> ProductDelete(int id)
        {
            var product = await _service.FindProductByID(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> ProductDelete(ProductModel model)
        {
            var response = await _service.DeleteProductByID(model.Id);

            if (!response)
                return View(model);

            return RedirectToAction(nameof(ProductIndex));
        }
    }
}
