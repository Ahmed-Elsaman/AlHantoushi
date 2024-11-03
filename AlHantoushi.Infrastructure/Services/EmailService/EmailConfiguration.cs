namespace AlHantoushi.Infrastructure.Services.EmailService
{
    public class EmailConfiguration
    {
        public string? from { get; set; } 
        public string? SmtpServer { get; set; } 
        public int Port { get; set; }
        public string? SmtpUsername { get; set; }
        public string? SmtpPassword { get; set; }
    }
}
