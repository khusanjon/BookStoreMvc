using BookStore.Domain.Entities;

namespace BookStore.WebUI.Models
{
    public class PurchaseIndexViewModel
    {
        public Purchase Purchase { get; set; }
        public string ReturnUrl { get; set; }
    }
}