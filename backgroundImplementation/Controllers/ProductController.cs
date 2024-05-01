using backgroundImplementation.BackgroundService;
using backgroundImplementation.cashing_services;
using backgroundImplementation.Data;
using backgroundImplementation.Modeals;
using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backgroundImplementation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        //private static List<Product> products = new List<Product>();
        private readonly ILogger<ProductController> _logger;
        private readonly ApplicationDbcontext _dbcontext;
        private readonly Icashing_service  _icashing;

       
        public ProductController(ILogger<ProductController> logger,ApplicationDbcontext dbcontext,Icashing_service icashing)
        {
            _logger = logger;
            _dbcontext = dbcontext;
           _icashing=icashing;
        }

        
        [HttpPost]
        [Route("AddProduct")]
        public IActionResult AddProduct(Product product)
        {
          _dbcontext.products.Add(product);
            _dbcontext.SaveChanges();
            var productList = _dbcontext.products.ToList(); 
            var expireDate = DateTimeOffset.Now.AddMinutes(3);
            _icashing.SetData("product", productList, expireDate);
            var JobId = BackgroundJob.Enqueue<IsendService>(x => x.SendEmail());
            return CreatedAtAction(nameof(GetAllProduct), product);
        }

        [HttpGet]
        [Route("GetAllProduct")]
        public async Task< IActionResult>GetAllProduct()
        {
            var CashingResult = _icashing.GetData <IEnumerable<Product>>("product");
            if (CashingResult != null && CashingResult.Count()>0)
            {
                   return Ok(CashingResult);
            }
            var productList = await _dbcontext.products.ToListAsync();
            var ExpireDate=DateTimeOffset.Now.AddMinutes(3);
            _icashing.SetData("product", productList, ExpireDate);
            return Ok(productList);
        }
        [HttpDelete]
        [Route("DeleteProduct")]
        public IActionResult DeleteProduct(Guid ID)
        {
            var Deleteproduct = _dbcontext.products.FirstOrDefault(x => x.Id == ID);
            if (Deleteproduct != null)
            {
                _dbcontext.products.Remove(Deleteproduct);
                _icashing.RemoveData("product");
                _dbcontext.SaveChanges();
                BackgroundJob.Enqueue<IsendService>(x => x.DeleteItem());
                return NoContent();
            }
            else
            {
                return NotFound();
            }

        }

    }
}
