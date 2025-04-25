using InventoryBAL.DTO;
using InventoryBAL.Interface;
using InventoryBAL.Repository;
using InventoryDAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBooking _bookingRepository;

        public BookingController(IBooking bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }


        [HttpPost("BookingByInventoryMemberId")]
        public async Task<APIResponse> BookingByInventoryMemberId(int inventoryId, int memberId)
        {

            return await _bookingRepository.BookItemAsync(memberId, inventoryId);

        }


        [HttpPost("CancelBookingById")]
        public async Task<bool> CancelBookingById(int bookingId)
        {

            return  await  _bookingRepository.CancelBookingAsync(bookingId);

        }


        // Get All Booking List
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookingList()
        {
            for(int i=0;i<=1000000;i++)
            {
                Console.WriteLine(i);
            }
            var booking = await _bookingRepository.GetAllBookingsAsync();
            return Ok(booking);
        }


    }
}
