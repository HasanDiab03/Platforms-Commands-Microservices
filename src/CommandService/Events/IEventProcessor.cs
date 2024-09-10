namespace CommandService.Events
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}