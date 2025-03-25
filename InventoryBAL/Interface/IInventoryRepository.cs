using InventoryBAL.DTO;
using InventoryDAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryBAL.Interface
{
    public interface IInventoryRepository
    {
        Task<Invertory> GetInventoryByIdAsync(int inventoryId);
        Task<IEnumerable<Invertory>> GetAllInventoriesAsync();
        Task<Invertory> AddInventoryAsync(Invertory inventory);
        Task<Invertory> UpdateInventoryAsync(Invertory inventory);
        Task<bool> DeleteInventoryAsync(int inventoryId);

        // Customize Method

        Task<APIResponse> ProcessCSVFileAsync(IFormFile file);
    }
}
