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
        public ViewResult Index(string returnUrl)
        {
            return View(new PurchaseIndexViewModel
            {
                Purchase = GetPurchase(),
                ReturnUrl = returnUrl
            });
        }

        private IBookRepository repository;
        public PurchaseController(IBookRepository repo)
        {
            repository = repo;
        }

        public RedirectToRouteResult AddToPurchase(int bookId, string returnUrl)
        {
            Book book = repository.Books
                .FirstOrDefault(g => g.BookId == bookId);

            if (book != null)
            {
                GetPurchase().AddItem(book, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromPurchase(int bookId, string returnUrl)
        {
            Book book = repository.Books
                .FirstOrDefault(g => g.BookId == bookId);

            if (book != null)
            {
                GetPurchase().RemoveLine(book);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public Purchase GetPurchase()
        {
            Purchase purchase = (Purchase)Session["Purchase"];
            if (purchase == null)
            {
                purchase = new Purchase();
                Session["Purchase"] = purchase;
            }
            return purchase;
        }
    }
}