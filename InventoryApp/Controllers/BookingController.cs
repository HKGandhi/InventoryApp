using InventoryBAL.DTO;
using InventoryBAL.Interface;
using InventoryBAL.Repository;
using InventoryDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
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
            var booking = await _bookingRepository.GetAllBookingsAsync();
            return Ok(booking);
        }


    }
}
