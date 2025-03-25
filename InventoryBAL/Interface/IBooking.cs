using InventoryBAL.DTO;
using InventoryDAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryBAL.Interface
{
    public interface IBooking
    {
        Task<APIResponse> BookItemAsync(int memberId, int inventoryId);
        Task<bool> CancelBookingAsync(int Id);

        Task<IEnumerable<Booking>> GetAllBookingsAsync();


    }
}
