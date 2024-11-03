using AlHantoushi.Api.Dtos;
using AlHantoushi.Api.RequestHelpers;
using AlHantoushi.Core.Entities;
using AlHantoushi.Core.Interfaces;
using AlHantoushi.Core.Specifications;
using AlHantoushi.Infrastructure.Extensions;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using System.Globalization;

namespace AlHantoushi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewsController(IMapper mapper , IGenericRepository<AlHantoushiNews> repo) : ControllerBase
{
    [HttpPost("CreateNews")]
    public async Task<IActionResult> CreateNews(NewsToCreateDto news)
    {
        if (news == null)
        {
            return BadRequest(new ApiResponse<object>(400, null, "Fail to create News"));
        }
        string imageUrl = string.Empty;
        if (news.ImgUrl != null)
        {
            imageUrl = await news.ImgUrl.UploadFile("News-Images");
        }

        var result = mapper.Map<AlHantoushiNews>(news);
        result.ImgUrl = imageUrl;

        repo.Add(result);
        if (await repo.SaveAllAsync())
        {
            return Ok(new ApiResponse<NewsToCreateDto>(200, news, "Created Successfully"));
        }
        return BadRequest(new ApiResponse<object>(400, null, "Something Go Wrong"));
    }

    [HttpGet("GetNewsData")]
    public async Task<ActionResult<IReadOnlyList<Pagination<NewsToReturnDto>>>> GetNewsData([FromQuery] NewsParam param)
    {
        var language = HttpContext.Request.Headers["Accept-Language"].ToString();
        var culture = new CultureInfo("ar");
        if (!string.IsNullOrWhiteSpace(language) && (language == "ar" || language == "en"))
        {
            culture = new CultureInfo(language);
        }
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        param.Language = language;

        var spec = new NewsSpecification(param);
        var news = await repo.ListAsync(spec);
        var count = await repo.CountAsync(spec);

        if (news is null)
            return BadRequest(new ApiResponse<object>(400, null, "Something Go Wrong"));

        var newsDto = mapper.Map<IReadOnlyList<NewsToReturnDto>>(news);

        return Ok(new Pagination<NewsToReturnDto>(param.PageIndex, param.PageSize, count, newsDto));
    }

    [HttpGet("GetNewById/{id:int}")]
    public async Task<ActionResult<NewsToReturnDto>> GetNewData(int id)
    {
        if (id <= 0)
            return BadRequest(new ApiResponse<NewsToUpdateDto>(400, null, "Something Go Wrong"));

        var spec = new NewsSpecification(id);
        var news = await repo.GetEntityWithSpec(spec);

        if (news is null)
            return NotFound(new ApiResponse<NewsToReturnDto>(404, null, "Not Found"));

        var newsDto = mapper.Map<NewsToReturnDto>(news);

        return Ok(new ApiResponse<NewsToReturnDto>(200, newsDto, "Returned Successfully"));
    }

    [HttpPut("UpdateNews/{id:int}")]
    public async Task<ActionResult> UpdateNews(int id, NewsToUpdateDto news)
    {
        if (news.Id != id || !repo.Exists(id))
            return BadRequest(new ApiResponse<object>(400, null, "Cannot update this product"));

        string imageUrl = string.Empty;
        if (news.ImgUrl != null)
        {
            imageUrl = await news.ImgUrl.UploadFile("News-Images");
        }

        var data = mapper.Map<AlHantoushiNews>(news);
        repo.Update(data);

        if (await repo.SaveAllAsync())
        {
            return Ok(new ApiResponse<object>(200, null, "Successfully Update"));
        }

        return BadRequest(new ApiResponse<object>(400, null, "Problem updating the product"));
    }

    [HttpDelete("DeleteNews/{id:int}")]
    public async Task<ActionResult> DeleteNews(int id)
    {
        var spec = new NewsSpecification(id);

        if (await repo.Remove(id, spec) == 1 && await repo.SaveAllAsync())
        {
            return Ok(new ApiResponse<object>(200, null, "Deleted Successfully"));
        }

        return BadRequest(new ApiResponse<object>(400, null, "Error Data Not found"));
    }
}
