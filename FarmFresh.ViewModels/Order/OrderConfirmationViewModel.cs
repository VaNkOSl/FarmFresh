using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmFresh.Data.Models;

namespace FarmFresh.ViewModels.Order
{
    public class OrderConfirmationViewModel
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public ICollection<OrderProduct> Products { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public byte[] Picture { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

    }
}
