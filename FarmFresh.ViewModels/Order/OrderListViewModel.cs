using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmFresh.Data.Models.Enums;

namespace FarmFresh.ViewModels.Order
{
    public class OrderListViewModel
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string ProductName { get; set; }
        public string OrderStatus { get; set; }
        public Decimal Price { get;set; }
        public int Quantity { get; set; }
        public byte[] Picture { get; set; }
    }
}
