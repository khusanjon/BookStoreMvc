using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Models;

namespace BookStore.WebUI.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        private IBookRepository repository;
        // PageSize указывает, что на одной странице должны отображаться 5 товаров
        public int pageSize = 5;
        public string cat;
        public BookController(IBookRepository repo)
        {
            repository = repo;
        }
        public ViewResult List(string category, int page = 1)
        {
            BooksListViewModel model = new BooksListViewModel
            {
                Books = repository.Books
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(book => book.BookId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                repository.Books.Count() : 
                repository.Books.Where(book => book.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }

        public FileContentResult GetImage(int bookId)
        {
            Book book = repository.Books
                .FirstOrDefault(b => b.BookId == bookId);

            if (book != null)
            {
                return File(book.ImageData, book.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}