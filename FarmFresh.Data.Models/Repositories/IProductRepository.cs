﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Data.Models.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(Guid productId);
    }
}
