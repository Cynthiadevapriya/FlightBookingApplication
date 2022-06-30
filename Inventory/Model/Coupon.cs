using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Model
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double Amount { get; set; }
    }
}
