namespace AlHantoushi.Infrastructure.Services.EmailService.Interfaces
{
    public interface IEmailServices
    {
        Task SendSingleEmailAsync(Message message);
        Task SendBulkEmailAsync(Message messages, int batchSize);
    }
}
