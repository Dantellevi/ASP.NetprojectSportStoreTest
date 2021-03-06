﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private IOrderProcessor orderProcessor;
        public CartController(IProductRepository _repo, IOrderProcessor proc)
        {
            repository = _repo;
            orderProcessor = proc;
        }


        public ViewResult Index (Cart cart,string returnUrl)
        {
            return View(new CartIndexModel
            {
                
                ReturnUrl=returnUrl,
                Cart=cart
            });
        }

       public RedirectToRouteResult AddToCart(Cart cart,int productid,string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productid);

            if(product!=null)
            {
                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart,int productid, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productid);
            if (product != null)
            {
               cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }


        public ViewResult Checkout()
        {
            return View(new ShippingDetails());

        }


        [HttpPost]
        public ViewResult Checkout(Cart cart,ShippingDetails shippingDetails)
        {
            if(cart.Lines.Count()==0)
            {
                ModelState.AddModelError("", "Ваша карта пуста!!");
            }
            if(ModelState.IsValid)
            {
                orderProcessor.Processor(cart, shippingDetails);
                cart.Clear();
                return View("Completed");

            }
            else
            {
                return View(shippingDetails);
            }
        }

    }
}