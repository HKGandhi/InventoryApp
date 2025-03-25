﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDAL.Models
{
    public class Member
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int BookingCount { get; set; }

        public DateTime? JoiningDate { get; set; }
    }
}
