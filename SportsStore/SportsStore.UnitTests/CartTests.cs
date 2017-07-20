using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using SportsStore.Domain.Entities;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;
using SportsStore.WebUI.Models;



namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {

            //организация - создание нскольких тестовых товаров

            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            //организация-создание новой корзины
            Cart target = new Cart();

            //действие

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            CartLine[] results = target.Lines.ToArray();


            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product, p2);


        }



        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //организация - создание нскольких тестовых товаров

            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            //организация-создание новой корзины
            Cart target = new Cart();

            //действие

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);


            CartLine[] results = target.Lines.OrderBy(c=>c.Product.ProductID).ToArray();



            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);
        }


        [TestMethod]
        public void Calculate_Cart_Total()
        {
            //организация - создание нскольких тестовых товаров

            Product p1 = new Product { ProductID = 1, Name = "P1",price=100M };
            Product p2 = new Product { ProductID = 2, Name = "P2",price=50M };

            //организация-создание новой корзины
            Cart target = new Cart();

            //действие

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);


            decimal results = target.ComputerTotalValue();



            Assert.AreEqual(results, 450M);
            
        }



        [TestMethod]
        public void Can_Clear_Contents()
        {
            //организация - создание нскольких тестовых товаров

            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            //организация-создание новой корзины
            Cart target = new Cart();

            //действие

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.Clear();


           



            Assert.AreEqual(target.Lines.Count(), 0);
          
        }

        //=======================================================================

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //организация -создание имитированного хранилища

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1, Name="P1",Category="Apples" },
            }.AsQueryable());

            //организация -создание экземпляра Cart
            Cart cart = new Cart();

            //организация- создание контроллера
            CartController target = new CartController(mock.Object,null);

            //действия- добавление товара в корзину

            target.AddToCart(cart, 1, null);

            //Утверждение- 
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }


        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            //организация -создание имитированного хранилища

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1, Name="P1",Category="Apples" },
            }.AsQueryable());

            //организация -создание экземпляра Cart
            Cart cart = new Cart();

            //организация- создание контроллера
            CartController target = new CartController(mock.Object, null);

            //действия- добавление товара в корзину

            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");


            //Утверждение- 
            Assert.AreEqual(result.RouteValues["action"],"Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");

        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            //организация- создание экземпляра cart
            Cart cart = new Cart();

            //Организация-создание контроллера
            CartController target = new CartController(null, null);

            //действие- вызов метода действия Index
            CartIndexModel result = (CartIndexModel)target.Index(cart, "myUrl").ViewData.Model;

            //утверждение
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");

        }

        [TestMethod]
        public void Cannot_Checkout_Emty_Cart()
        {
            //Организация-создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();


            //Организация- создание пустой корзины
            Cart cart = new Cart();

            //Организация -создание деталей о доставке
            ShippingDetails shipingDetails = new ShippingDetails();

            //Организация--создание экземпляра контроллера
            CartController target = new CartController(null, mock.Object);


            //Действие
            ViewResult result = target.Checkout(cart, shipingDetails);

            //утверждение-проверка что заказ не был передан обработчику
            mock.Verify(m => m.Processor(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            //Утверждение-проверка, что метод вернул стандартное представление
            Assert.AreEqual("", result.ViewName);

            //Утверждение-проверка что метод вернул стандартное представление
            Assert.AreEqual("", result.ViewName);

            //Утверждение - проверка, что представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);

        }


        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            //Организация-создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();


            //Организация- создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
           
           

            //Организация--создание экземпляра контроллера
            CartController target = new CartController(null, mock.Object);


            //Действие-попытка перехода к оплате
            ViewResult result = target.Checkout(cart, new ShippingDetails());

            //утверждение-проверка, что заказ не был передан обработчику
            mock.Verify(m => m.Processor(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            //Утверждение-проверка, что метод вернул стандартное представление
            Assert.AreEqual("", result.ViewName);
            
            //Утверждение - проверка, что представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }


        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            //Организация-создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();


            //Организация- создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);



            //Организация--создание экземпляра контроллера
            CartController target = new CartController(null, mock.Object);


            //Действие-попытка перехода к оплате
            ViewResult result = target.Checkout(cart, new ShippingDetails());

            //утверждение-проверка, что заказ  был передан обработчику
            mock.Verify(m => m.Processor(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());
            

            //Утверждение-проверка что метод вернул  представление Completed
            Assert.AreEqual("Completed", result.ViewName);

            //Утверждение - проверка, что представлению передана неверная модель
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }



            



    }
}
