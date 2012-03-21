using System;
using System.Collections.Generic;
using System.Web.Http.Services;
using WebApiContacts.Controllers;

namespace WebApiContacts.IoC
{
    public class MyResolver : IDependencyResolver
    {
        private static ContactRepository repository = new ContactRepository();

        private static Dictionary<Type, Func<object>> container = new Dictionary<Type, Func<object>>();

        static MyResolver()
        {
            container.Add(typeof (ContactRepository), () => repository);
            container.Add(typeof (ContactsController), () => new ContactsController(GetInstance<ContactRepository>()));
            container.Add(typeof (ImageController), () => new ImageController(GetInstance<ContactRepository>()));
        }

        private static T GetInstance<T>() where T : class
        {
            return container[typeof (T)]() as T;
        }

        public object GetService(Type serviceType)
        {
            if (container.ContainsKey(serviceType))
            {
                return container[serviceType]();
            }
            // return null if not handled
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            // return empty collection if not handled
            return new List<object>();
        }
    }
}