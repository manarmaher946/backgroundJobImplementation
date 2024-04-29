using backgroundImplementation.BackgroundService;
using backgroundImplementation.Modeals;
using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backgroundImplementation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private static List<Product> products = new List<Product>();
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("AddProduct")]
        public IActionResult AddProduct(Product product)
        {
            products.Add(product);
            var JobId = BackgroundJob.Enqueue<IsendService>(x=>x.SendEmail());
            return CreatedAtAction(nameof(GetAllProduct), product);
        }

        [HttpGet]
        [Route("GetAllProduct")]
        public IActionResult GetAllProduct()
        {
            var productList = products.ToList();

            return Ok(productList);
        }
        [HttpDelete]
        [Route("DeleteProduct")]
        public IActionResult DeleteProduct(Guid ID)
        {
            var Deleteproduct = products.FirstOrDefault(x => x.Id == ID);
            if (Deleteproduct != null)
            {
            products.Remove(Deleteproduct);
            BackgroundJob.Enqueue<IsendService>(x => x.DeleteItem());
            }
            return NoContent();

        }

    }
}
