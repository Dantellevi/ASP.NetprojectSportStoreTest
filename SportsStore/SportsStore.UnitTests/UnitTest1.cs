using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using SportsStore.Domain.Entities;
using System.Linq;
using Moq;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers;
using Ninject.Planning.Targets;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1, Name="P1" },
                new Product {ProductID=2, Name="P2" },
                new Product {ProductID=3, Name="P3" },
                new Product {ProductID=4, Name="P4" },
               new Product {ProductID=4, Name="P5" }



            });

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;
            //действия
            ProductListViewModel result=(ProductListViewModel)controller.List(null,2).Model;
            //утверждение
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        //_________________________________________________________________________________
        [TestMethod]

        public void Can_Page_Links()
        {
            //организация-определение вспомогательного метода HTML
            //это необходимо для применения расширяющего метода

            HtmlHelper meHelper = null;
            //организация-создание данных PagingInfo
            PageInfo paginginfo = new PageInfo
            {
                Currentpage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            //организация-настройка делегата с помощью лямда -выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            //действие
            MvcHtmlString result = meHelper.PageLings(paginginfo, pageUrlDelegate);
            //утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>" + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>" + @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());



        }

        //_________________________________________________________________________________

        [TestMethod]
        public void Can_Send_Pagination_View_model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1, Name="P1" },
                new Product {ProductID=2, Name="P2" },
                new Product {ProductID=3, Name="P3" },
                new Product {ProductID=4, Name="P4" },
               new Product {ProductID=4, Name="P5" }



            });

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;
            //действия

            ProductListViewModel result = (ProductListViewModel)controller.List(null,2).Model;
            //утверждение
            PageInfo pageInfo = result.PageInfo;
            Assert.AreEqual(pageInfo.Currentpage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1, Name="P1",Category="Cat1" },
                new Product {ProductID=2, Name="P2",Category="Cat2" },
                new Product {ProductID=3, Name="P3",Category="Cat1" },
                new Product {ProductID=4, Name="P4",Category="Cat2" },
               new Product {ProductID=4, Name="P5",Category="Cat3"}

              

        });

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;


           Product[] result = ((ProductListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();
            //утверждение

            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }


        [TestMethod]
        public void Can_Create_categories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1, Name="P1",Category="Apples" },
                new Product {ProductID=2, Name="P2",Category="Apples" },
                new Product {ProductID=3, Name="P3",Category="Plums" },
                new Product {ProductID=4, Name="P4",Category="Oranges" }
             
        });
            //организация -создание контроллера
            NavController controller = new NavController(mock.Object);
            //действие получение набора категорий
            string[] results = ((IEnumerable<string>)controller.Menu().Model).ToArray();



            //утверждение
            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Plums");


        }


        [TestMethod]
        public void Indicates_selected_Category()
        {
            //организация -создание имитированного хранилища
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1, Name="P1",Category="Apples" },
                new Product {ProductID=2, Name="P2",Category="Oranges" }
               

        });

            NavController controller = new NavController(mock.Object);

            //определение-выбранной категории

            string categorySelect = "Apples";

            //действие-
            string result = controller.Menu(categorySelect).ViewBag.SelectedCategory;

            //утверждение

            Assert.AreEqual(categorySelect, result);

        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            //организация -создание имитированного хранилища
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1, Name="P1",Category="Cat1" },
                new Product {ProductID=2, Name="P2",Category="Cat2" },
                new Product {ProductID=3, Name="P3",Category="Cat1" },
                new Product {ProductID=4, Name="P4",Category="Cat2" },
               new Product {ProductID=4, Name="P5",Category="Cat3"}



        });

            //оргранизация- создание контроллера и установка размера страницы 
            //равным трем элементам
            ProductController contr = new ProductController(mock.Object);
            contr.pageSize = 3;

            //действие-тестирование счетчиков товаров для различных категорий

            int res1 = ((ProductListViewModel)contr.List("Cat1").Model).PageInfo.TotalItems;
            int res2 = ((ProductListViewModel)contr.List("Cat2").Model).PageInfo.TotalItems;
            int res3 = ((ProductListViewModel)contr.List("Cat3").Model).PageInfo.TotalItems;
            int resAll= ((ProductListViewModel)contr.List(null).Model).PageInfo.TotalItems;
            //утверждение

            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);

        }




    }
}
