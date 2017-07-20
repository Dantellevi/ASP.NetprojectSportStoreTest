using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers;
using Ninject.Planning.Targets;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Infrastructure.Concrete;
using System.Linq;

using Moq;



namespace SportsStore.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {

            //Организация- создание объекта Product  с данными изображения
            Product prod = new Product
            {
                ProductID = 2,
                Name = "Test",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            //Организация- создание имитированного хранилища
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID=1,Name="P1" },
                prod,
                new Product {ProductID=3,Name="P3" }

            }.AsQueryable());

            //Организация- создание контроллера 
            ProductController target = new ProductController(mock.Object);

            //Действие-вызов метода действия GetImage()
            ActionResult result = target.GetImage(2);

            //Утверждение
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(prod.ImageMimeType, ((FileResult)result).ContentType);


        }

        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            //Организация-создание  имитированного хранилища
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID=1,Name="P1" },
                new Product {ProductID=2,Name="P2" }

            }.AsQueryable());

            //Организация- создание контроллера 
            ProductController target = new ProductController(mock.Object);

            //Действие-вызов метода действия GetImage()
            ActionResult result = target.GetImage(100);

            //Утверждение
            Assert.IsNull(result);



        }




    }
}
