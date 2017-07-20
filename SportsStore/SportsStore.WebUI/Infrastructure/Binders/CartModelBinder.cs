using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Entities;


namespace SportsStore.WebUI.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //получаем объект Cart  из сеанса
            Cart cart = null;

            if (controllerContext.HttpContext.Session != null)
            {
                cart = (Cart)controllerContext.HttpContext.Session[sessionKey];
            }
            //создать экземпляр Cart, если он не обнаружен в данных сеанса

            if(cart==null)
            {
                cart = new Cart();
                if(controllerContext.HttpContext.Session!=null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = cart;
                }


            }
            //возратить объект Cart
            return cart;
        }
    }
}