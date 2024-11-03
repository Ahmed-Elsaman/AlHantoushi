using AlHantoushi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AlHantoushi.Infrastructure.Services.EmailService;

public class ConsultationService(StoreContext context)
{
    public async Task<List<string>> GetAllConsultationEmailsAsync()
    {
        return await context.Consultations
                             .Select(s => s.Email)
                             .ToListAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await context.Consultations.AnyAsync(s => s.Email == email);
    }
}
