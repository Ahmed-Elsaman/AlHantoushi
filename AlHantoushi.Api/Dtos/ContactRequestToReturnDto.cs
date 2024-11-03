namespace AlHantoushi.Api.Dtos;

public class ContactRequestToReturnDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Service { get; set; }
    public string PhoneNumber { get; set; }
    public string CountryCode { get; set; }
    public string Message { get; set; }
    public DateTime CreatedOn { get; set; }
}
