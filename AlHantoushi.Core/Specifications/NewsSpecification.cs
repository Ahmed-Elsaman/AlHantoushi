using AlHantoushi.Core.Entities;

namespace AlHantoushi.Core.Specifications;

public class NewsSpecification : BaseSpecification<AlHantoushiNews>
{
    public NewsSpecification(NewsParam specParams) : base(x =>
   (specParams.Language == "ar"
            ? x.TitleAr.ToLower().Contains(specParams.Search)
            : x.TitleEn.ToLower().Contains(specParams.Search))
   &&( specParams.Status <= 0 || x.Status == specParams.Status)
    && (!specParams.GetDateRange().StartDate.HasValue && !specParams.GetDateRange().EndDate.HasValue) || (x.CreatedOn >= specParams.GetDateRange().StartDate && x.CreatedOn <= specParams.GetDateRange().EndDate))
    {
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        switch (specParams.Sort)
        {
            case "Asc":
                AddOrderBy(x => x.CreatedOn);
                break;
            case "Desc":
                AddOrderByDescending(x => x.CreatedOn);
                break;
            default:
                AddOrderByDescending(x => x.CreatedOn);
                break;
        }
    }
    public NewsSpecification()
    {
        AddOrderByDescending(x => x.CreatedOn);
        ApplyPaging(0, 1);
    }
    public NewsSpecification(int id) : base(x => x.Id == id)
    {

    }
    public NewsSpecification(int status , bool x) : base(x => x.Status == status)
    {

    }
}
