namespace cx.EventsPublisher
{
    public interface IEventPublisher
    {
        void Publish<T>(T @event);
    }
}
