using System.Web.Mvc;
using BookStore.Domain.Entities;

namespace BookStore.WebUI.Infrastructure.Binders
{
    public class PurchaseModelBinder : IModelBinder
    {
        private const string sessionKey = "Purchase";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            // Получить объект Purchase из сеанса
            Purchase purchase = null;
            if (controllerContext.HttpContext.Session != null)
            {
                purchase = (Purchase)controllerContext.HttpContext.Session[sessionKey];
            }

            // Создать объект Purchase если он не обнаружен в сеансе
            if (purchase == null)
            {
                purchase = new Purchase();
                if (controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = purchase;
                }
            }

            // Возвратить объект Purchase
            return purchase;
        }
    }
}