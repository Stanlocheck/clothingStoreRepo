using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClothesInterfacesBLL;
using ClothDTOs;
using Microsoft.AspNetCore.Authorization;

namespace clothingStoreWebAPI.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IAdminsBLL _adminBLL;
        public AdminController(IAdminsBLL adminsBLL){
            _adminBLL = adminsBLL;
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
