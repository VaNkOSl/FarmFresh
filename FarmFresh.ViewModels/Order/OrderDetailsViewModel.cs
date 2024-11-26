using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmFresh.Data.Models.Enums;

namespace FarmFresh.ViewModels.Order
{
    public class OrderDetailsViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber {  get; set; }
        public string Email { get; set; }
        public string ProductName { get; set; }
        public DeliveryOption DeliveryOption { get; set; }
        public string OrderStatus { get; set; }
        public string ProductDescription { get; set; }
        public string FarmerName { get; set; }
        public  string Origin { get; set; }
        public decimal ProductPrice { get; set; }
        public Seasons Seasons { get; set; }
        public DateTime HarvestDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public byte[] Picture { get; set; }

    }
}
