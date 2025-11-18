using AutoMapper;
using SwiftCart.Application.Tests;
using SwiftCart.Domain.Entities;


namespace EmotiaMart.Application.Mappings
{
    public class TestMappingProfile : Profile
    {
        public TestMappingProfile()
        {
            CreateMap<TestDto, Test>();
                // .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<Test, TestDto>();
        }
    }
}


