namespace cx.EventsPublisher
{
    public interface IHandleEvent<TEvent>
    {
        void Handle(TEvent @event);
    }
}
