using Microsoft.AspNetCore.Mvc;
using ClothesInterfacesBLL;
using ClothDTOs;
using Microsoft.AspNetCore.Authorization;

namespace clothingStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IClothBLL _clothBLL;

        public HomeController(IClothBLL clothBLL) {
            _clothBLL = clothBLL;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClothDTO>>> GetAll(){
            try{
                var cloth = await _clothBLL.GetAll();
                return Ok(cloth);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClothDTO>> GetById(Guid id){
            try{
                var cloth = await _clothBLL.GetById(id);
                return Ok(cloth);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult> AddCloth(ClothAddDTO addCloth){
            try{
                await _clothBLL.AddCloth(addCloth);
                return Ok(addCloth);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCloth(ClothAddDTO updtCloth, Guid id){
            try{
                await _clothBLL.UpdateCloth(updtCloth, id);
                return Ok(updtCloth);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCloth(Guid id){
            try{
                await _clothBLL.DeleteCloth(id);
                return Ok();
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
    }
}
