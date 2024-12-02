using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.ViewModels.Products
{
    public class CreateProductViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public Guid FarmerId { get; set; }

        public string? Description { get; set; }

        public byte[] Photo { get; set; }

        public DateTime HarvestDate { get; set; }

        public DateTime ExpirationDate { get; set; }
    }


}
