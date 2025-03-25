using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDAL.Models
{
    public class Booking
    {

        public int Id { get; set; }

        public int MemberId { get; set; }

        public int InventoryId { get; set; }

        public DateTime? DateTimeBooked { get; set; }
    }
}
