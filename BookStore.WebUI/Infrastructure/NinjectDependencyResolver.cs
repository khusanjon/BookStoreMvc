using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Ninject;
using BookStore.Domain.Abstract;
using BookStore.Domain.Concrete;
using BookStore.WebUI.Infrastructure.Concrete;
using BookStore.WebUI.Infrastructure.Abstract;

namespace BookStore.WebUI.Infrastructure
{
    // Библиотеку Ninject для создания специального распознавателя зависимостей,
    // который MVC Framework будет применять при создании экземпляров объектов внутри приложения.
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            // Здесь размещон привязки            
            kernel.Bind<IBookRepository>().To<EFBookRepository>();

            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager
                    .AppSettings["Email.WriteAsFile"] ?? "false")
            };

            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", emailSettings);
            kernel.Bind<IAuthProvider>().To<FormAuthProvider>();
        }
    }
}