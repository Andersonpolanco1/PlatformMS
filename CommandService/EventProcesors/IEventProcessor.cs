namespace CommandService.EventProcesors
{
    public interface IEventProcessor

    {
        void ProcessEvent(string message);
    }
}
