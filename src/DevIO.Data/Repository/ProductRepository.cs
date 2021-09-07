﻿using DevIO.Business.Models;
using DevIO.Data.Context;
using DevIO.Business.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MyDbContext db) : base(db) { }
        public async Task<IEnumerable<Product>> GetProducstBySupplier(Guid supplierId)
        {
            return await Search(p => p.SupplierId == supplierId);
        }

        public async Task<IEnumerable<Product>> GetProductsSuppliers()
        {

       

            var ProductsSuppliers = await Db.Products.AsNoTracking()
                .Include(f => f.Supplier)
                //.Include(f => f.Supplier.Address)
                .OrderBy(p => p.Name)
                .ToArrayAsync();
            return ProductsSuppliers;
        }

        public async Task<Product> GetProductSupplier(Guid id)
        {
            return await Db.Products.AsNoTracking()
                .Include(f => f.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
