using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;
		private readonly ISecurePasswordHasher _securePasswordHasher;

		public CreateUserCommandHandler(IUnitofWork unitofWork, IMapper mapper, ISecurePasswordHasher securePasswordHasher)
        {
            _mapper = mapper;
			_securePasswordHasher = securePasswordHasher;
			_unitofWork = unitofWork;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request.UserToAddDto);
            user.Id = Guid.NewGuid().ToString();
            user.PasswordHash = _securePasswordHasher.Hash(request.UserToAddDto.Password);

            _unitofWork.Users.AddAsync(user);
            await _unitofWork.Save();

            return user.Id;
        }
    }
}