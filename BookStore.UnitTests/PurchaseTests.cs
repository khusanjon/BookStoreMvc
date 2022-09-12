using BookStore.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
