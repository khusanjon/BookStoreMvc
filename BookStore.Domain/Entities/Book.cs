using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BookStore.Domain.Entities
{
    public class Book
    {
        [HiddenInput(DisplayValue = false)]
        public int BookId { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, введите название книги")]
        public string Name { get; set; }

        [Display(Name = "Автор")]
        [Required(ErrorMessage = "Пожалуйста, введите Автор для книги")]
        public string Author { get; set; }

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Пожалуйста, укажите категорию для книги")]
        public string Category { get; set; }

        [Display(Name = "Цена (сум)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение для цены")]
        public decimal Price { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }
}
