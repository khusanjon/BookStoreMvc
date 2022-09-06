using System.Data.Entity;
using BookStore.Domain.Entities;

namespace BookStore.Domain.Concrete
{   // Для подключения к базе данных через Entity Framework, Нужено посредник - контекст данных.
    // Контекст данных представляет собой класс, производный от класса DbContext.
    // Контекст данных содержит одно или несколько свойств типа DbSet<T>
    public class EFDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
    }
}
