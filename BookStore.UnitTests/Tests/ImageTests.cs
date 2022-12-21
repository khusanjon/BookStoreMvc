using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using BookStore.Domain.Entities;
using BookStore.Domain.Abstract;
using BookStore.WebUI.Controllers;

namespace BookStore.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Организация - создание объекта Game с данными изображения
            Book book = new Book
            {
                BookId = 2,
                Name = "Книга2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            // Организация - создание имитированного хранилища
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
                new Book {BookId = 1, Name = "Книга1"},
                book,
                new Book {BookId = 3, Name = "Книга3"}
            }.AsQueryable());

            // Организация - создание контроллера
            BookController controller = new BookController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(2);

            // Утверждение
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(book.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Организация - создание имитированного хранилища
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
                new Book {BookId = 1, Name = "Игра1"},
                new Book {BookId = 2, Name = "Игра2"}
            }.AsQueryable());

            // Организация - создание контроллера
            BookController controller = new BookController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(10);

            // Утверждение
            Assert.IsNull(result);
        }
    }
}