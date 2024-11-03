using AlHantoushi.Api.Dtos;
using AlHantoushi.Api.RequestHelpers;
using AlHantoushi.Core.Entities;
using AlHantoushi.Core.Interfaces;
using AlHantoushi.Infrastructure.Services.EmailService;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AlHantoushi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactRequestController(IMapper mapper , IGenericRepository<ContactRequest> repo , EmailServices services) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> CreateContactRequest(ContactRequestToCreateDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse<object>(400, null, "Invalid data"));

            var contactRequest = mapper.Map<ContactRequest>(dto);
            repo.Add(contactRequest);

            if (await repo.SaveAllAsync())
            {
                return Ok(new ApiResponse<ContactRequestToCreateDto>(200, dto, "Contact request created successfully"));
            }

            return BadRequest(new ApiResponse<object>(400, null, "Failed to create contact request"));
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<ContactRequestToReturnDto>>> GetAllContactRequests()
        {
            var contactRequests = await repo.ListAllAsync();

            if (contactRequests == null || !contactRequests.Any())
                return NotFound(new ApiResponse<object>(404, null, "No contact requests found"));

            var contactRequestDtos = mapper.Map<IReadOnlyList<ContactRequestToReturnDto>>(contactRequests);
            return Ok(contactRequestDtos);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<ContactRequestToReturnDto>> GetContactRequestById(int id)
        {
            var contactRequest = await repo.GetByIdAsync(id);

            if (contactRequest == null)
                return NotFound(new ApiResponse<object>(404, null, "Contact request not found"));

            var contactRequestDto = mapper.Map<ContactRequestToReturnDto>(contactRequest);
            return Ok(contactRequestDto);
        }

        [HttpPost("notify-ContactRequest/{Id}")]
        public async Task<IActionResult> NotifyContactRequest(int Id)
        {
            if (Id <= 0)
                return NotFound(new ApiResponse<object>(400, null, "Consultation not found."));

            var contact = await repo.GetByIdAsync(Id);

            if (contact == null || contact.Email == null || !EmailTemplateHelper.IsValidEmail(contact.Email))
                return NotFound(new ApiResponse<object>(404, null, "Applicant not found"));

            var emailMessage = new Message(
                new[] { contact.Email },
                "Test Title",
                "Test Content"
            );

            try
            {
                services?.SendSingleEmailAsync(emailMessage);
                return Ok(new ApiResponse<object>(200, null, "Email sent successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(500, null, $"Error sending email: {ex.Message}"));
            }
        }
    }
}
