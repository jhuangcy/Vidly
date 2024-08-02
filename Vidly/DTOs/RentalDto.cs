using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vidly2.DTOs
{
    public class RentalDto
    {
        public int CustomerId { get; set; }
        public List<int> MovieIds { get; set; }
    }
}