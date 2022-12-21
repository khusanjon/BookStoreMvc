using BookStore.Domain.Entities;
using System.Collections.Generic;

namespace BookStore.Domain.Abstract
{
    public interface IBookRepository
    {
        // получать последовательность объектов Book
        // Класс, зависящий от интерфейса IBookRepository, может получать объекты Book
        IEnumerable<Book> Books { get; }
        void SaveBook(Book book);
        Book DeleteBook(int bookId);
    }
}
