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
using Moq;


namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            //Организация-создание имитированного поставщика аутентификации
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "secret")).Returns(true);

            //Организация- создание модели представления
            LoginViewModel model = new LoginViewModel
            {
                UserName = "admin",
                Password = "secret"
            };

            //Организация- создание контроллера
            AccountController target = new AccountController(mock.Object);


            //Действиек- аутентификация с использованием правильных учетных данных

            ActionResult result = target.Login(model, "/MyURL");

            //Утверждение

            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyURL", ((RedirectResult)result).Url);



        }




        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentails()
        {
            //Организация-создание имитированного поставщика аутентификации
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("badUser", "badPass")).Returns(false);

            //Организация- создание модели представления
            LoginViewModel model = new LoginViewModel
            {
                UserName = "badUser",
                Password = "badPass"
            };

            //Организация- создание контроллера
            AccountController target = new AccountController(mock.Object);


            //Действиек- аутентификация с использованием правильных учетных данных

            ActionResult result = target.Login(model, "/MyURL");

            //Утверждение

            Assert.IsInstanceOfType(result, typeof(ViewResult));
           Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
