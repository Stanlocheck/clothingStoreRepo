using ClothDTOs;
using ClothesInterfacesBLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace clothingStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        private IBuyersBLL _buyerBLL;

        public BuyerController(IBuyersBLL buyersBLL){
            _buyerBLL = buyersBLL;
        }

        [HttpGet]
        public async Task<ActionResult<List<BuyerDTO>>> GetAll(){
            try{
                var buyer = await _buyerBLL.GetAll();
                return Ok(buyer);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BuyerDTO>> GetById(Guid id){
            try{
                var buyer = await _buyerBLL.GetById(id);
                return Ok(buyer);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddBuyer(BuyerAddDTO addBuyer){
            try{
                await _buyerBLL.AddBuyer(addBuyer);
                return Ok(addBuyer);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBuyer(BuyerAddDTO updtBuyer, Guid id){
            try{
                await _buyerBLL.UpdateBuyer(updtBuyer, id);
                return Ok(updtBuyer);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBuyer(Guid id){
            try{
                await _buyerBLL.DeleteBuyer(id);
                return Ok();
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
    }
}
