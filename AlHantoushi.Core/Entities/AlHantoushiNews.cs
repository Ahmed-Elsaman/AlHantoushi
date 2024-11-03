namespace AlHantoushi.Core.Entities;

public class AlHantoushiNews : BaseEntity
{
    public string TitleAr { get; set; }
    public string TitleEn { get; set; }
    public string DescriptionAr { get; set; }
    public string DescriptionEn { get; set; }
    public string ImgUrl { get; set; }
    public int Status { get; set; }
}
