using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.ViewModels.Order
{
    public class OrderDetailsViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }
        public string FarmerName { get; set; }
        public  string Origin { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }


    }
}
