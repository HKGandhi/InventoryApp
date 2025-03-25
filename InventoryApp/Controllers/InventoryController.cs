using InventoryBAL.DTO;
using InventoryBAL.Interface;
using InventoryDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace InventoryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        // GET: api/inventory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invertory>>> GetAllInventories()
        {
            var inventories = await _inventoryRepository.GetAllInventoriesAsync();
            return Ok(inventories);
        }

        // GET: api/inventory/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Invertory>> GetInventoryById(int id)
        {
            var inventory = await _inventoryRepository.GetInventoryByIdAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
            return Ok(inventory);
        }

        // POST: api/inventory
        [HttpPost]
        public async Task<ActionResult<Invertory>> CreateInventory([FromBody] Invertory inventory)
        {
            var createdInventory = await _inventoryRepository.AddInventoryAsync(inventory);
            return CreatedAtAction(nameof(GetInventoryById), new { id = createdInventory.Id }, createdInventory);
        }

        // PUT: api/inventory/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventory(int id, [FromBody] Invertory inventory)
        {
            if (id != inventory.Id)
            {
                return BadRequest();
            }

            var updatedInventory = await _inventoryRepository.UpdateInventoryAsync(inventory);
            return Ok(updatedInventory);
        }

        // DELETE: api/inventory/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            var deleted = await _inventoryRepository.DeleteInventoryAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }



        // CSV Upload 

        [HttpPost("UploadInventoryCSV")]
        public async Task<APIResponse> UploadInventoryCSV(IFormFile file)
        {
          
           return await _inventoryRepository.ProcessCSVFileAsync(file);
          
        }
    }
}
