using AlHantoushi.Api.Dtos;
using AlHantoushi.Api.RequestHelpers;
using AlHantoushi.Core.Entities;
using AlHantoushi.Core.Interfaces;
using AlHantoushi.Core.Specifications;
using AlHantoushi.Infrastructure.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AlHantoushi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScrollBarController(IMapper mapper , IGenericRepository<ScrollBar> repo) : ControllerBase
{
    [HttpPost("CreateScrollBar")]
    public async Task<IActionResult> CreateScrollBar(ScrollBarToCreateDto bar)
    {
        if (bar == null)
        {
            return BadRequest(new ApiResponse<object>(400, null, "Fail to create News"));
        }
        string imageUrl = string.Empty;
        if (bar.ImgUrl != null)
        {
            imageUrl = await bar.ImgUrl.UploadFile("ScrollBar-Images");
        }

        var result = mapper.Map<ScrollBar>(bar);
        result.ImgUrl = imageUrl;

        repo.Add(result);
        if (await repo.SaveAllAsync())
        {
            return Ok(new ApiResponse<ScrollBarToCreateDto>(200, bar, "Created Successfully"));
        }
        return BadRequest(new ApiResponse<object>(400, null, "Something Go Wrong"));
    }

    [HttpGet("GetScrollBarData")]
    public async Task<ActionResult<IReadOnlyList<NewsToUpdateDto>>> GetScrollBarData()
    {   
       
        var news = await repo.ListAllAsync();

        if (news is null)
            return BadRequest(new ApiResponse<object>(400, null, "Something Go Wrong"));

        var scrollBarDto = mapper.Map<IReadOnlyList<ScrollBarToReturnDto>>(news);

        return Ok(new ApiResponse<IReadOnlyList<ScrollBarToReturnDto>>(200 , scrollBarDto , "Successfully Returned"));
    }

    [HttpPut("UpdateScrollBar/{id:int}")]
    public async Task<ActionResult> UpdateScrollBar(int id, ScrollBarToUpdateDto bar)
    {
        if (bar.Id != id || !repo.Exists(id))
            return BadRequest(new ApiResponse<object>(400, null, "Cannot update this product"));

        string imageUrl = string.Empty;
        if (bar.ImgUrl != null)
        {
            imageUrl = await bar.ImgUrl.UploadFile("ScrollBar-Images");
        }

        var data = mapper.Map<ScrollBar>(bar);
        repo.Update(data);

        if (await repo.SaveAllAsync())
        {
            return Ok(new ApiResponse<object>(200, null, "Successfully Update"));
        }

        return BadRequest(new ApiResponse<object>(400, null, "Problem updating the product"));
    }

    [HttpDelete("DeleteScrollBar/{id:int}")]
    public async Task<ActionResult> DeleteScrollBar(int id)
    {
        var spec = new ScrollBarSpecification(id);

        if (await repo.Remove(id, spec) == 1 && await repo.SaveAllAsync())
        {
            return Ok(new ApiResponse<object>(200, null, "Deleted Successfully"));
        }

        return BadRequest(new ApiResponse<object>(400, null, "Error Data Not found"));
    }
}
