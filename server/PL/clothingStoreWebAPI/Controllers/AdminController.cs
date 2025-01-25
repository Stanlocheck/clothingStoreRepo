using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClothesInterfacesBLL;
using ClothDTOs;
using Microsoft.AspNetCore.Authorization;

namespace clothingStoreWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IAdminsBLL _adminBLL;

        public AdminController(IAdminsBLL adminsBLL){
            _adminBLL = adminsBLL;
        }

        [HttpGet]
        public async Task<ActionResult<List<AdminDTO>>> GetAll(){
            try{
                var admin = await _adminBLL.GetAll();
                return Ok(admin);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdminDTO>> GetById(Guid id){
            try{
                var admin = await _adminBLL.GetById(id);
                return Ok(admin);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddAdmin(AdminAddDTO addAdmin){
            try{
                await _adminBLL.AddAdmin(addAdmin);
                return Ok(addAdmin);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAdmin(AdminAddDTO updtAdmin, Guid id){
            try{
                await _adminBLL.UpdateAdmin(updtAdmin, id);
                return Ok(updtAdmin);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAdmin(Guid id){
            try{
                await _adminBLL.DeleteAdmin(id);
                return Ok();
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
    }
}
