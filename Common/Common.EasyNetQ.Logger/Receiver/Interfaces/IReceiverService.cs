namespace Common.EasyNetQ.Logger.Receiver.Interfaces
{
    public interface IReceiverService
    {
        Task Processing(LogDTO log);
    }
}
