using AlHantoushi.Api.Dtos;
using AlHantoushi.Api.RequestHelpers;
using AlHantoushi.Core.Entities;
using AlHantoushi.Core.Interfaces;
using AlHantoushi.Core.Specifications;
using AlHantoushi.Infrastructure.Services.EmailService;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AlHantoushi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationController(IMapper _mapper , IGenericRepository<Consultation> _consultationRepository, EmailServices services) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> CreateConsultation([FromBody] ConsultationCreateDto consultationDto)
        {
            if (consultationDto == null)
                return BadRequest(new ApiResponse<object>(400 , null , "Invalid consultation data."));

            var consultation = _mapper.Map<Consultation>(consultationDto);
            _consultationRepository.Add(consultation);

            if (await _consultationRepository.SaveAllAsync())
                return Ok(new ApiResponse<object>(200 , null, "Consultation created successfully"));

            return BadRequest(new ApiResponse<object>(400, null, "Invalid consultation data."));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllConsultations([FromQuery] ConsultationParam param)
        {
            var spec = new ConsultationSpecification(param);
            var consultations = await _consultationRepository.ListAsync(spec);
            var count = await _consultationRepository.CountAsync(spec);
            if (consultations is null)
                return BadRequest(new ApiResponse<object>(400, null, "Something Go Wrong"));

            var DataDto = _mapper.Map<IReadOnlyList<ConsultationToReturnDto>>(consultations);

            return Ok(new Pagination<ConsultationToReturnDto>(param.PageIndex, param.PageSize, count, DataDto));
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetConsultationById(int id)
        {
            if (id <= 0)
                return NotFound(new ApiResponse<object>(400, null, "Consultation not found."));

            var consultation = await _consultationRepository.GetByIdAsync(id);
            if (consultation == null)
                return NotFound(new ApiResponse<object>(400, null, "Consultation not found."));
            
            var data = _mapper.Map<ConsultationToReturnDto>(consultation);
            return Ok(new ApiResponse<ConsultationToReturnDto>(400, data, "Consultation not found."));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteConsultation(int id)
        {
            if (id <= 0)
                return NotFound(new ApiResponse<object>(400, null, "Consultation not found."));

            var consultation = await _consultationRepository.GetByIdAsync(id);
            if (consultation == null)
                return NotFound("Consultation not found.");

            var spec = new ConsultationSpecification(id);

            await _consultationRepository.Remove(id , spec);
            if (await _consultationRepository.SaveAllAsync())
                return Ok(new ApiResponse<object>(200 , null , "Consultation deleted successfully"));

            return BadRequest(new ApiResponse<object>(400, null, "Failed to delete consultation."));
        }

        [HttpPost("notify-consultation/{Id}")]
        public async Task<IActionResult> NotifyConsultation(int Id)
        {
            if (Id <= 0)
                return NotFound(new ApiResponse<object>(400, null, "Consultation not found."));

            var consultation = await _consultationRepository.GetByIdAsync(Id);

            if (consultation == null || consultation.Email == null || !EmailTemplateHelper.IsValidEmail(consultation.Email))
                return NotFound(new ApiResponse<object>(404, null, "Applicant not found"));

            var emailMessage = new Message(
                new[] { consultation.Email },
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

