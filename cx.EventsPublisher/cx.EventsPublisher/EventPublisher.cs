using System;
using System.Linq;

namespace cx.EventsPublisher
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ISubscriptionService _subscriptionService;
        public EventPublisher(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        public void Publish<TEvent>(TEvent @event)
        {
            var subscriptions = _subscriptionService.GetSubscriptions<TEvent>();
            subscriptions.ToList().ForEach(x => PublishToEvent(x, @event));
        }

        private static void PublishToEvent<TEvent>(IHandleEvent<TEvent> x, TEvent @event)
        {
            try
            {
                x.Handle(@event);
            }
            catch { }
            finally
            {
                var instance = x as IDisposable;
                if (instance != null)
                {
                    instance.Dispose();
                }
            }
        }
    }
}
