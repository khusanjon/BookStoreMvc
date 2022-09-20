using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
using BookStore.WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BookStore.UnitTests
{
    [TestClass]
    public class PurchaseTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Организация - создание нескольких тестовых Книг
            Book book1 = new Book { BookId = 1, Name = "Книга1" };
            Book book2 = new Book { BookId = 2, Name = "Книга2" };

            // Организация - создание корзины
            Purchase purchase = new Purchase();

            // Действие
            purchase.AddItem(book1, 1);
            purchase.AddItem(book2, 1);
            List<PurchaseLine> results = purchase.Lines.ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Book, book1);
            Assert.AreEqual(results[1].Book, book2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Организация - создание нескольких тестовых Книг
            Book book1 = new Book { BookId = 1, Name = "Книга1" };
            Book book2 = new Book { BookId = 2, Name = "Книга2" };

            // Организация - создание корзины
            Purchase purchase = new Purchase();

            // Действие
            purchase.AddItem(book1, 1);
            purchase.AddItem(book2, 1);
            purchase.AddItem(book1, 5);
            List<PurchaseLine> results = purchase.Lines.OrderBy(c => c.Book.BookId).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпляров добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // Организация - создание нескольких тестовых игр
            Book book1 = new Book { BookId = 1, Name = "Книга1" };
            Book book2 = new Book { BookId = 2, Name = "Книга2" };
            Book book3 = new Book { BookId = 3, Name = "Книга3" };

            // Организация - создание корзины
            Purchase purchase = new Purchase();

            // Организация - добавление нескольких игр в корзину
            purchase.AddItem(book1, 1);
            purchase.AddItem(book2, 4);
            purchase.AddItem(book3, 4);
            purchase.AddItem(book2, 1);

            // Действие
            purchase.RemoveLine(book2);

            // Утверждение
            Assert.AreEqual(purchase.Lines.Where(c => c.Book == book2).Count(), 0);
            Assert.AreEqual(purchase.Lines.Count(), 2);
        }
        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Организация - создание нескольких тестовых игр
            Book book1 = new Book { BookId = 1, Name = "Книга1", Price = 100 };
            Book book2 = new Book { BookId = 2, Name = "Книга2", Price = 55 };

            // Организация - создание корзины
            Purchase purchase = new Purchase();

            // Действие
            purchase.AddItem(book1, 1);
            purchase.AddItem(book2, 1);
            purchase.AddItem(book1, 5);
            decimal result = purchase.ComputeTotalValue();

            // Утверждение
            Assert.AreEqual(result, 655);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Организация - создание нескольких тестовых игр
            Book book1 = new Book { BookId = 1, Name = "Книга1", Price = 100 };
            Book book2 = new Book { BookId = 2, Name = "Книга2", Price = 55 };

            // Организация - создание корзины
            Purchase purchase = new Purchase();

            // Действие
            purchase.AddItem(book1, 1);
            purchase.AddItem(book2, 1);
            purchase.AddItem(book1, 5);
            purchase.Clear();

            // Утверждение
            Assert.AreEqual(purchase.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Purchase()
        {
            // Организация - создание имитированного хранилища
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
            new Book {BookId = 1, Name = "Книга1", Category = "Кат1"},
        }.AsQueryable());

            // Организация - создание корзины
            Purchase purchase = new Purchase();

            // Организация - создание контроллера
            PurchaseController controller = new PurchaseController(mock.Object);

            // Действие - добавить книгу в корзину
            controller.AddToPurchase(purchase, 1, null);

            // Утверждение
            Assert.AreEqual(purchase.Lines.Count(), 1);
            Assert.AreEqual(purchase.Lines.ToList()[0].Book.BookId, 1);
        }

        /// <summary>
        /// После добавления книги в корзину, должно быть перенаправление на страницу корзины
        /// </summary>
        [TestMethod]
        public void Adding_Book_To_Purchase_Goes_To_Purchase_Screen()
        {
            // Организация - создание имитированного хранилища
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
            new Book {BookId = 1, Name = "Книга1", Category = "Кат1"},
        }.AsQueryable());

            // Организация - создание корзины
            Purchase purchase = new Purchase();

            // Организация - создание контроллера
            PurchaseController controller = new PurchaseController(mock.Object);

            // Действие - добавить книгу в корзину
            RedirectToRouteResult result = controller.AddToPurchase(purchase, 2, "myUrl");

            // Утверждение
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        // Проверяем URL
        [TestMethod]
        public void Can_View_Purchase_Contents()
        {
            // Организация - создание корзины
            Purchase purchase = new Purchase();

            // Организация - создание контроллера
            PurchaseController target = new PurchaseController(null);

            // Действие - вызов метода действия Index()
            PurchaseIndexViewModel result
                = (PurchaseIndexViewModel)target.Index(purchase, "myUrl").ViewData.Model;

            // Утверждение
            Assert.AreSame(result.Purchase, purchase);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Purchase()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация - создание пустой корзины
            Purchase purchase = new Purchase();

            // Организация - создание деталей о доставке
            ShippingDetails shippingDetails = new ShippingDetails();

            // Организация - создание контроллера
                PurchaseController controller = new PurchaseController(null, mock.Object);

            // Действие
            ViewResult result = controller.Checkout(purchase, shippingDetails);

            // Утверждение — проверка, что заказ не был передан обработчику 
            mock.Verify(m => m.ProcessOrder(It.IsAny<Purchase>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение — проверка, что метод вернул стандартное представление 
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Purchase purchase = new Purchase();
            purchase.AddItem(new Book(), 1);

            // Организация — создание контроллера
            PurchaseController controller = new PurchaseController(null, mock.Object);

            // Организация — добавление ошибки в модель
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(purchase, new ShippingDetails());

            // Утверждение - проверка, что заказ не передается обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Purchase>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение - проверка, что метод вернул стандартное представление
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
public void Can_Checkout_And_Submit_Order()
{
    // Организация - создание имитированного обработчика заказов
    Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

    // Организация — создание корзины с элементом
    Purchase purchase = new Purchase();
    purchase.AddItem(new Book(), 1);

    // Организация — создание контроллера
    PurchaseController controller = new PurchaseController(null, mock.Object);

    // Действие - попытка перехода к оплате
    ViewResult result = controller.Checkout(purchase, new ShippingDetails());

    // Утверждение - проверка, что заказ передан обработчику
    mock.Verify(m => m.ProcessOrder(It.IsAny<Purchase>(), It.IsAny<ShippingDetails>()),
        Times.Once());

    // Утверждение - проверка, что метод возвращает представление 
    Assert.AreEqual("Completed", result.ViewName);

    // Утверждение - проверка, что представлению передается допустимая модель
    Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
}
    }
}
