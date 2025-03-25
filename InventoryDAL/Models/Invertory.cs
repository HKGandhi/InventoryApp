using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDAL.Models
{
    public class Invertory
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int RemainingCOunt {  get; set; }

        public DateTime? ExpireDate { get; set; }
    }
}
