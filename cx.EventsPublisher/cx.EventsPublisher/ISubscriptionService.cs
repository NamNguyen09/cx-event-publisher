using System.Collections.Generic;

namespace cx.EventsPublisher
{
    public interface ISubscriptionService
    {
        IEnumerable<IHandleEvent<TEvent>> GetSubscriptions<TEvent>();
    }
}
