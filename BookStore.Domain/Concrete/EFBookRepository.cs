using System.Collections.Generic;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;

namespace BookStore.Domain.Concrete
{   // Создание хранилища для объектов Book
    public class EFBookRepository : IBookRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Book> Books
        {
            get { return context.Books; } 
        }

        public void SaveBook(Book book)
        {
            if (book.BookId == 0)
                context.Books.Add(book);
            else
            {
                Book dbEntry = context.Books.Find(book.BookId);
                if (dbEntry != null)
                {
                    dbEntry.Name = book.Name;
                    dbEntry.Author = book.Author;
                    dbEntry.Price = book.Price;
                    dbEntry.Category = book.Category;
                    dbEntry.ImageData = book.ImageData;
                    dbEntry.ImageMimeType = book.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Book DeleteBook(int bookId)
        {
            Book dbEntry = context.Books.Find(bookId);
            if (dbEntry != null)
            {
                context.Books.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
