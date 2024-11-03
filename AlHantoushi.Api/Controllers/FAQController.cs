using AlHantoushi.Api.Dtos;
using AlHantoushi.Api.RequestHelpers;
using AlHantoushi.Core.Entities;
using AlHantoushi.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AlHantoushi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQController(IMapper _mapper , IGenericRepository<FAQ> _faqRepository) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> CreateFAQ([FromBody] FAQCreateDto faqDto)
        {
            if (faqDto == null)
                return BadRequest("Invalid FAQ data.");

            var faq = _mapper.Map<FAQ>(faqDto);
            _faqRepository.Add(faq);

            if (await _faqRepository.SaveAllAsync())
            {
                var returnDto = _mapper.Map<FAQReturnDto>(faq);
                return Ok(new ApiResponse<object>(200 , null , "Created Successfully"));
            }

            return BadRequest(new ApiResponse<object>(400, null, "Failed to create FAQ."));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllFAQs()
        {
            var faqs = await _faqRepository.ListAllAsync();
            var faqsToReturn = _mapper.Map<IEnumerable<FAQReturnDto>>(faqs);
            return Ok(new ApiResponse<IEnumerable<FAQReturnDto>>(200, faqsToReturn, "Successfully Returned"));
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetFAQById(int id)
        {
            var faq = await _faqRepository.GetByIdAsync(id);
            if (faq == null)
                return NotFound(new ApiResponse<object>(404, null, "FAQ not found."));

            var returnDto = _mapper.Map<FAQReturnDto>(faq);
            return Ok(new ApiResponse<FAQReturnDto>(200, returnDto, "Successfully Returned"));
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateFAQ(int id, [FromBody] FAQCreateDto faqDto)
        {
            if (id <= 0)
                return NotFound(new ApiResponse<object>(404, null, "FAQ not found."));
            var faq = await _faqRepository.GetByIdAsync(id);
            if (faq == null)
                return NotFound(new ApiResponse<object>(404, null, "FAQ not found."));

            _mapper.Map(faqDto, faq);
            _faqRepository.Update(faq);

            if (await _faqRepository.SaveAllAsync())
            {
                var returnDto = _mapper.Map<FAQReturnDto>(faq);
                return Ok(new ApiResponse<object>(200, null, "FAQ updated."));
            }

            return NotFound(new ApiResponse<object>(404, null, "FAQ not found."));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            if (id <= 0)
                return NotFound(new ApiResponse<object>(404, null, "FAQ not found."));

            var faq = await _faqRepository.GetByIdAsync(id);
            if (faq == null)
                return NotFound(new ApiResponse<object>(404, null, "FAQ not found."));

            _faqRepository.Remove(faq);
            if (await _faqRepository.SaveAllAsync())
                return Ok(new ApiResponse<object>(200, null, "FAQ Deleted."));

            return NotFound(new ApiResponse<object>(404, null, "FAQ not found."));
        }
    }
}

