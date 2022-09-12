using System.Collections.Generic;
using System.Linq;

namespace BookStore.Domain.Entities
{
    public class Purchase // Модуль покупки
    {
        private List<PurchaseLine> lineCollection = new List<PurchaseLine>();

        public void AddItem(Book book, int quantity) // добавления элемента в корзину
        {
            PurchaseLine line = lineCollection
                .Where(b => b.Book.BookId == book.BookId)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new PurchaseLine
                {
                    Book = book,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Book book) // удаления элемента из корзины
        {
            lineCollection.RemoveAll(l => l.Book.BookId == book.BookId);
        }

        public decimal ComputeTotalValue() // вычисления общей стоимости элементов в корзине
        {
            return lineCollection.Sum(e => e.Book.Price * e.Quantity);

        }
        public void Clear() // очистки корзины путем удаления всех элементов
        {
            lineCollection.Clear();
        }

        public IEnumerable<PurchaseLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class PurchaseLine // Определен в том же самом файле и представляет товар, выбранный пользователем, а также приобретаемое его количество
    {
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }  


}
