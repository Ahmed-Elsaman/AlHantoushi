using AlHantoushi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AlHantoushi.Infrastructure.Services.EmailService;

public class ContactRequestsService(StoreContext context)
{
    public async Task<List<string>> GetAllContactRequestsEmailsAsync()
    {
        return await context.ContactRequests
                             .Select(s => s.Email)
                             .ToListAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await context.ContactRequests.AnyAsync(s => s.Email == email);
    }
}
