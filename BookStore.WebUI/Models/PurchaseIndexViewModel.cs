using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.WebUI.Models
{
    public class PurchaseIndexViewModel
    {
        public Purchase Purchase { get; set; }
        public string ReturnUrl { get; set; }
    }
}