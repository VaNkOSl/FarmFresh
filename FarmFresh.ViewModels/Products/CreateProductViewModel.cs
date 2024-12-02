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

        [Required]
        public DateTime HarvestDate { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        public byte[] Photo { get; set; } = new byte[0];

        [Required]
        [DataType(DataType.Upload)]
        public IFormFile UploadedPhoto { get; set; }
    }

}
