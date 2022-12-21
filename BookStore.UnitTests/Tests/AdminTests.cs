using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BookStore.UnitTests.Tests
{
    [TestClass]
    public class AdminTest1
    {
        [TestMethod]
        public void Index_Contains_All_Book()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Книга1"},
                new Book { BookId = 2, Name = "Книга2"},
                new Book { BookId = 3, Name = "Книга3"},
                new Book { BookId = 4, Name = "Книга4"},
                new Book { BookId = 5, Name = "Книга5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            List<Book> result = ((IEnumerable<Book>)controller.Index().
                ViewData.Model).ToList();

            // Утверждение
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Книга1", result[0].Name);
            Assert.AreEqual("Книга2", result[1].Name);
            Assert.AreEqual("Книга3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Book()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
    {
        new Book { BookId = 1, Name = "Книга1"},
        new Book { BookId = 2, Name = "Книга2"},
        new Book { BookId = 3, Name = "Книга3"},
        new Book { BookId = 4, Name = "Книга4"},
        new Book { BookId = 5, Name = "Книга5"}
    });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Book book1 = controller.Edit(1).ViewData.Model as Book;
            Book book2 = controller.Edit(2).ViewData.Model as Book;
            Book book3 = controller.Edit(3).ViewData.Model as Book;

            // Assert
            Assert.AreEqual(1, book1.BookId);
            Assert.AreEqual(2, book2.BookId);
            Assert.AreEqual(3, book3.BookId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Book()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
    {
        new Book { BookId = 1, Name = "Книга1"},
        new Book { BookId = 2, Name = "Книга2"},
        new Book { BookId = 3, Name = "Книга3"},
        new Book { BookId = 4, Name = "Книга4"},
        new Book { BookId = 5, Name = "Книга5"}
    });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Book result = controller.Edit(6).ViewData.Model as Book;

            // Assert
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IBookRepository> mock = new Mock<IBookRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Book book = new Book { Name = "Test" };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(book);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SaveBook(book));

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IBookRepository> mock = new Mock<IBookRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Book book = new Book { Name = "Test" };

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(book);

            // Утверждение - проверка того, что обращение к хранилищу НЕ производится 
            mock.Verify(m => m.SaveBook(It.IsAny<Book>()), Times.Never());

            // Утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Books()
        {
            // Организация - создание объекта Book
            Book book = new Book { BookId = 2, Name = "Игра2" };

            // Организация - создание имитированного хранилища данных
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Игра1"},
                new Book { BookId = 2, Name = "Игра2"},
                new Book { BookId = 3, Name = "Игра3"},
                new Book { BookId = 4, Name = "Игра4"},
                new Book { BookId = 5, Name = "Игра5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие - удаление игры
            controller.Delete(book.BookId);

            // Утверждение - проверка того, что метод удаления в хранилище
            // вызывается для корректного объекта Book
            mock.Verify(m => m.DeleteBook(book.BookId));
        }


    }
}
