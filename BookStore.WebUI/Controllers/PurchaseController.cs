using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class PurchaseController : Controller
    {

        private IBookRepository repository;
        public PurchaseController(IBookRepository repo)
        {
            repository = repo;
        }
        public ViewResult Index(Purchase purchase, string returnUrl)
        {
            return View(new PurchaseIndexViewModel
            {
                Purchase = purchase,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToPurchase(Purchase purchase, int bookId, string returnUrl)
        {
            Book book = repository.Books
                .FirstOrDefault(g => g.BookId == bookId);

            if (book != null)
            {
                purchase.AddItem(book, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromPurchase(Purchase purchase, int bookId, string returnUrl)
        {
            Book book = repository.Books
                .FirstOrDefault(g => g.BookId == bookId);

            if (book != null)
            {
                purchase.RemoveLine(book);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Purchase purchase)
        {
            return PartialView(purchase);
        }
    }
}