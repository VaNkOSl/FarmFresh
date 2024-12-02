using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.ViewModels.Products
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? Description { get; set; }
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; } // Add this for editing
        public string FarmerName { get; set; }
        public Guid FarmerId { get; set; } // Add this for editing
        public DateTime HarvestDate { get; set; } // Add this for editing
        public DateTime ExpirationDate { get; set; } // Add this for editing
        public List<string> Photos { get; set; } = new List<string>();
    }

}
