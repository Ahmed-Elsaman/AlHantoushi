using AlHantoushi.Core.Entities;

namespace AlHantoushi.Core.Specifications;

public class ConsultationSpecification : BaseSpecification<Consultation>
{
    public ConsultationSpecification(ConsultationParam specParams) : base(x =>
   (x.Title.ToLower().Contains(specParams.Search))
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
    public ConsultationSpecification()
    {
        AddOrderByDescending(x => x.CreatedOn);
        ApplyPaging(0, 1);
    }
    public ConsultationSpecification(int id) : base(x => x.Id == id)
    {

    }
}
