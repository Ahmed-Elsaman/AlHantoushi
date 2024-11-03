using AlHantoushi.Infrastructure.Data;

using System.Text.RegularExpressions;

namespace AlHantoushi.Api.RequestHelpers;

public static class EmailTemplateHelper
{
    public static string GetBodyContentByStatus(StoreContext context)
    {
        return $@"
                    <p>Dear ,</p>
                    <p>We are pleased to inform you that your application has been accepted. 
                    Congratulations on becoming a part of our team!</p>
                    <p>We look forward to working with you.</p>";
    }
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }

}
