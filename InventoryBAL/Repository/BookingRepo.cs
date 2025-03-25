using InventoryBAL.DTO;
using InventoryBAL.Interface;
using InventoryDAL;
using InventoryDAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryBAL.Repository
{
    public class BookingRepo : IBooking
    {


        private readonly DBContext _context;

        public BookingRepo(DBContext context)
        {
            _context = context;
        }

        private async Task<bool> CanBookItemAsync(int memberId)
        {
            int maxBookings = Constant.MAX_Booking;
            var currentBookings = await _context.Bookings.CountAsync(b => b.MemberId == memberId);
            return currentBookings < maxBookings;
        }

        // Check if the inventory item is available (i.e., quantity > 0)
        private async Task<bool> IsInventoryAvailableAsync(int inventoryId)
        {
            var inventory = await _context.Invertories.FirstOrDefaultAsync(i => i.Id == inventoryId);
            return inventory != null && inventory.RemainingCOunt > 0;
        }

      

        // Cancel a booking and restore inventory
        public async Task<bool> CancelBookingAsync(int bookingReference)
        {
            var booking = await _context.Bookings
                                         .FirstOrDefaultAsync(b => b.Id == bookingReference);

            if (booking == null) return false;

            var inventory = await _context.Invertories.FirstOrDefaultAsync(i => i.Id == booking.InventoryId);
            inventory.RemainingCOunt ++;

            var member = await _context.Members.FirstOrDefaultAsync(i => i.Id == booking.MemberId);
            member.BookingCount --;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }

       public async Task<APIResponse> BookItemAsync(int memberId, int inventoryId)
        {
            Random random = new Random();
            var memberCanBook = await CanBookItemAsync(memberId);
            var inventoryIsAvailable = await IsInventoryAvailableAsync(inventoryId);

            if (!memberCanBook || !inventoryIsAvailable)
            {
                return new APIResponse { IsSuccess = false, Message = "Cannot book item. Either member has max bookings or inventory is unavailable." };
            }

            // Create the booking
            var booking = new Booking
            {
                MemberId = memberId,
                InventoryId = inventoryId,
                DateTimeBooked = DateTime.UtcNow,
                Id = random.Next(1, 100)  // Generate a unique booking reference
            };

            var inventory = await _context.Invertories.FirstOrDefaultAsync(i => i.Id == inventoryId);
            inventory.RemainingCOunt--;

            var member = await _context.Members.FirstOrDefaultAsync(i => i.Id == memberId);
            member.BookingCount++;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return new APIResponse { IsSuccess = true, Message = "Booking Id" + booking.Id };
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings.ToListAsync();
        }
    }
}
