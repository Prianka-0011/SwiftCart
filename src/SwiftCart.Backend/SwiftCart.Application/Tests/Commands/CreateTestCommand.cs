using MediatR;
using SwiftCart.Application.Tests;
using SwiftCart.Domain.Entities;
using AutoMapper;
using SwiftCart.Application.Interfaces.Repositories;

namespace SwiftCart.Application.Tests.Commands
{
    public class CreateTestCommand : IRequest<TestDto>
    {
        public TestDto Test { get; set; } = null!;
        
        public class Handler : IRequestHandler<CreateTestCommand, TestDto>
        {
            private readonly ITestRepository _repo;
            private readonly IMapper _mapper;

            public Handler(ITestRepository repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }

            public async Task<TestDto> Handle(CreateTestCommand request, CancellationToken cancellationToken)
            {
                // Map DTO → Entity
                var entity = _mapper.Map<Test>(request.Test);
                
                // Save to database
                entity.Id = await _repo.CreateAsync(entity);

                // Map back to DTO
                return _mapper.Map<TestDto>(entity);
            }
        }
    }
}
