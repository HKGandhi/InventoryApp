using InventoryBAL.DTO;
using InventoryBAL.Interface;
using InventoryDAL;
using InventoryDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryBAL.Repository
{
    public class InventoryRepository : IInventoryRepository
    {

        private readonly DBContext _context;

        public InventoryRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<Invertory> GetInventoryByIdAsync(int inventoryId)
        {
            return await _context.Invertories.FindAsync(inventoryId);
        }

        public async Task<IEnumerable<Invertory>> GetAllInventoriesAsync()
        {
            return await _context.Invertories.ToListAsync();
        }

        public async Task<Invertory> AddInventoryAsync(Invertory inventory)
        {
            var result = await _context.Invertories.AddAsync(inventory);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Invertory> UpdateInventoryAsync(Invertory inventory)
        {
            _context.Invertories.Update(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }

        public async Task<bool> DeleteInventoryAsync(int inventoryId)
        {
            var inventory = await _context.Invertories.FindAsync(inventoryId);
            if (inventory == null) return false;

            _context.Invertories.Remove(inventory);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<APIResponse> ProcessCSVFileAsync(IFormFile file)
        {
            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    // Optional: Skip the header if the file has a header row
                    csv.Read();
                    csv.ReadHeader();

                    // Read each record in the CSV file
                    while (csv.Read())
                    {
                        var inventory = new Invertory
                        {
                            Title = csv.GetField<string>("Title"),
                            ExpireDate = string.IsNullOrEmpty(csv.GetField<string>("ExpireDate")) ? DateTime.Now : Convert.ToDateTime(csv.GetField<string>("ExpireDate")),
                            Description = csv.GetField<string>("Description"),
                            RemainingCOunt = csv.GetField<int>("RemainingCOunt")
                        };


                        await _context.Invertories.AddAsync(inventory);
                    }


                    await _context.SaveChangesAsync();
                }
                return new APIResponse { IsSuccess = true, Message ="CSV File Data Inserted Successfully"};

            }
            catch (Exception ex)
            {
                return new APIResponse { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}

