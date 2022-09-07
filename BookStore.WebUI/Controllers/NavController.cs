using BookStore.Domain.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IBookRepository repository;

        public NavController(IBookRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu()
        {
            IEnumerable<string> categories = repository.Books
                .Select(book => book.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(categories);
        }
    }
}