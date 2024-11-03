using AlHantoushi.Api.Dtos;
using AlHantoushi.Core.Entities;

using AutoMapper;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AlHantoushiNews , NewsToReturnDto>();
        CreateMap<NewsToCreateDto, AlHantoushiNews>();
        CreateMap<NewsToUpdateDto, AlHantoushiNews>();
        CreateMap<ContactRequestToCreateDto, ContactRequest>();
        CreateMap<ContactRequest, ContactRequestToReturnDto>();
        CreateMap<ConsultationCreateDto, Consultation>();
        CreateMap<Consultation, ConsultationToReturnDto>();
        CreateMap<FAQCreateDto, FAQ>();
        CreateMap<FAQ, FAQReturnDto>();
    }
}