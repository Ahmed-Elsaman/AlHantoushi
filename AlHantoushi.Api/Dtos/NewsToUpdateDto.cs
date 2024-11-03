namespace AlHantoushi.Api.Dtos;

public class NewsToUpdateDto
{
    public int Id { get; set; }
    public string TitleAr { get; set; }
    public string TitleEn { get; set; }
    public string DescriptionAr { get; set; }
    public string DescriptionEn { get; set; }
    public IFormFile ImgUrl { get; set; }
    public int Status { get; set; }
}
