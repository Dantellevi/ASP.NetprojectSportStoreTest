﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Controllers
{   
    

    public class NavController : Controller
    {
        private IProductRepository repository;
        
        public NavController(IProductRepository _repository)
        {
            repository = _repository;

        } 
        // GET: Nav
        public PartialViewResult Menu(string category=null, bool horizontalLayout=false)
           
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = repository.Products.Select(x => x.Category).Distinct().OrderBy(x => x);

            string viewName = horizontalLayout ? "MenuHorizontal" : "Menu";
            return PartialView(viewName,categories);
        }
    }
}