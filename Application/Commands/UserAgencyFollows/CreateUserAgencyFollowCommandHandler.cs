using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.UserAgencyFollows
{
    public class CreateUserAgencyFollowCommandHandler : IRequestHandler<CreateUserAgencyFollowCommand, UserAgencyFollow>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;
        public CreateUserAgencyFollowCommandHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitofWork = unitofWork;
        }

        public async Task<UserAgencyFollow> Handle(CreateUserAgencyFollowCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitofWork.Users.GetByIdAsync(request.UserAgencyFollowToAddDto.UserId);
            if (user == null)
                throw new Exception("Korisnik nije pronađen");

            var agency = await _unitofWork.Agencies.GetByIdAsync(request.UserAgencyFollowToAddDto.AgencyId);
            if (agency == null)
                throw new Exception("Agencija nije pronađena");
                
            var userFollow = _mapper.Map<UserAgencyFollow>(request.UserAgencyFollowToAddDto);
            userFollow.Id = Guid.NewGuid().ToString();
            _unitofWork.UserAgencyFollows.AddAsync(userFollow);
            await _unitofWork.Save();

            return userFollow;
        }
    }
}