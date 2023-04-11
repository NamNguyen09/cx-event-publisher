using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace cx.EventsPublisher
{
    public class EventSubscriptions : ISubscriptionService
    {
        private static IUnityContainer _unityContainer;
        public static void RegisterEventSubscriptions(IEnumerable<Assembly> systemAssemblies, IUnityContainer unityContainer,
            bool isApp, string subcriptionTypeNamspace = "RelayLog.Business.Subscriptions")
        {
            if (unityContainer == null) return;
            _unityContainer = unityContainer;
            var allClassTypes = systemAssemblies.SelectMany(x => x.GetTypes()).Where(t => !t.IsInterface);
            var subcriptionTypes = allClassTypes.Where(t => t.Namespace != null && t.Namespace.Contains(subcriptionTypeNamspace) && t.IsPublic).ToList();
            foreach (var subcriptionType in subcriptionTypes)
            {
                var handleEventInterfaces = subcriptionType.GetInterfaces()
                        .Where(x => x.IsGenericType)
                        .Where(x => x.GetGenericTypeDefinition() == typeof(IHandleEvent<>))
                        .ToList();
                foreach (var handleEventInterface in handleEventInterfaces)
                {
                    var lifetimeManager = isApp ? (LifetimeManager)new WindowsServicePerThreadLifetimeManager() : new HttpContextLifetimeManager();
                    _unityContainer.RegisterType(handleEventInterface, subcriptionType,
                                                subcriptionType.FullName, lifetimeManager);
                }
            }
        }

        public IEnumerable<IHandleEvent<TEvent>> GetSubscriptions<TEvent>()
        {
            if (_unityContainer == null) return Enumerable.Empty<IHandleEvent<TEvent>>();

            var events = _unityContainer.ResolveAll(typeof(IHandleEvent<TEvent>));

            return events.Cast<IHandleEvent<TEvent>>();
        }
    }
}
