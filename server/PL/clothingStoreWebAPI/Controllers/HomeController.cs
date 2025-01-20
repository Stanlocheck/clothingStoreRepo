using ClothDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClothingStorePersistence;
using Microsoft.EntityFrameworkCore;

namespace clothingStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cloth>>> GetAll(){
            return await _context.Clothes.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Cloth>> PostCloth(Cloth cloth){
            _context.Clothes.Add(cloth);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
