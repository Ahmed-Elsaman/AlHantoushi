using AlHantoushi.Core.Entities;

namespace AlHantoushi.Core.Specifications;

public class ScrollBarSpecification : BaseSpecification<ScrollBar>
{
    public ScrollBarSpecification(int id) : base(x => x.Id == id)
    {

    }
}
