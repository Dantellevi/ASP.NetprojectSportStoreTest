using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;



namespace SportsStore.WebUI.Controllers
{

    [Authorize]
    public class AdminController : Controller
    {
        IProductRepository repository;

        public AdminController(IProductRepository _repo)
        {
            repository = _repo;
        }

        
        public ViewResult Index()
        {
            return View(repository.Products);
        }

        public ViewResult Edit(int productId)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            return View(product);

        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase image=null)
        {
            if(ModelState.IsValid)
            {
                if(image!=null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);

                }


                repository.SaveProduct(product);
                TempData["message"] = string.Format("{0} Объект сохранен", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                //ошибка в значениях данных
                return View(product);

            }
        }



        public ViewResult Create()
        {
            return View("Edit", new Product());
        }


        public ActionResult Delete(int productID)
        {
            Product deleteProduct = repository.DeleteProduct(productID);
            if(deleteProduct!=null)
            {
                TempData["message"] = string.Format("{0} элемент удален!!");

            }
            return RedirectToAction("Index");

        }
    }
}