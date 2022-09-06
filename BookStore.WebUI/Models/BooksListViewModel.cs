using BookStore.Domain.Entities;
using System.Collections.Generic;


namespace BookStore.WebUI.Models
{
    public class BooksListViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}