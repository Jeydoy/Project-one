using System;
using System.Collections.Generic;
using System.Text;

namespace CafeDomain.Model
{
    public class CartItem
    {
        public int ProductId { get; set; }  
        public string ProductName { get; set; }= null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }   
    }
}
