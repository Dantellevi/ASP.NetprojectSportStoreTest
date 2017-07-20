using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;


namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {

        private IProductRepository repository;
        public int pageSize =3;
          

        public ProductController(IProductRepository ProductRepository)
        {
            this.repository = ProductRepository;

        }

        public ViewResult List(string category, int page=1)
        {
            ProductListViewModel model = new ProductListViewModel {
             Products=repository.Products.Where(p=>category==null ||p.Category==category).OrderBy(p => p.ProductID).Skip((page - 1) * pageSize).Take(pageSize),
             PageInfo=new PageInfo
             {
                 Currentpage=page,
                 ItemsPerPage=pageSize,
                 TotalItems=category==null ?
                 repository.Products.Count():
                 repository.Products.Where(e=>e.Category==category).Count()
             },
             CurrentCategory=category

            };
            return View(model);

        }

        public FileContentResult GetImage(int productId)
        {
            Product prod = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if(prod!=null)
            {
                return File(prod.ImageData, prod.ImageMimeType);

            }
            else
            {
                return null;
            }


        }


       
    }
}