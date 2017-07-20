﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;



namespace SportsStore.Domain.Concrete
{
  public  class EFProductRepository : IProductRepository
    {
        private EFContext context = new EFContext();

        public IEnumerable<Product> Products
        {
            get
            {
                return context.Products;
            }
        }

        public Product DeleteProduct(int productID)
        {
            Product dbEntry = context.Products.Find(productID);
            if(dbEntry!=null)
            {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }

        public void SaveProduct(Product product)
        {
           if(product.ProductID==0)
            {
                context.Products.Add(product);

            }
            else
            {
                Product dbEntry = context.Products.Find(product.ProductID);
                if(dbEntry!=null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.description = product.description;
                    dbEntry.price = product.price;
                    dbEntry.Category = product.Category;
                    dbEntry.ImageData = product.ImageData;
                    dbEntry.ImageMimeType = product.ImageMimeType;


                }
            }
            context.SaveChanges();
        }
    }
}
