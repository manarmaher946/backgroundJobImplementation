using backgroundImplementation.Data;
using backgroundImplementation.DistributedChashing;
using backgroundImplementation.Modeals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace backgroundImplementation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbcontext _dbcontext;
        private readonly IDistributed distributed;
    public CategoryController(IDistributed distributed,ApplicationDbcontext dbcontext)
        {
            _dbcontext=dbcontext;
            this.distributed = distributed;
        }

        [HttpGet]
        [Route("GetAllCategory")]
      public async Task<IActionResult> GetAllCategory()
        {
            var DataInCashing = distributed.GetData<IEnumerable<Category>>("Category");
            if (DataInCashing != null && DataInCashing.Count() >0)
            {
                return Ok(DataInCashing);
            }
            var datafromdatabase=await _dbcontext.categories.ToListAsync();
            var Expriredate=DateTime.Now.AddMinutes(3);
            distributed.SetData("Category", datafromdatabase, Expriredate);
            return Ok(datafromdatabase);
        }

        [HttpPost]
        [Route("AddCategory")]
        public async Task<IActionResult> AddCategory(Category category)
        {
            _dbcontext.categories.Add(category);
            _dbcontext.SaveChanges();
            var datafromdatabase = await _dbcontext.categories.ToListAsync();
            var Expriredate = DateTime.Now.AddMinutes(3);
            distributed.SetData("Category", datafromdatabase, Expriredate);
            return CreatedAtAction(nameof(GetAllCategory), category);
        }

    }
}
