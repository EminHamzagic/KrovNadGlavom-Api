using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.UserAgencyFollows
{
    public class CreateUserAgencyFollowCommandHandler : IRequestHandler<CreateUserAgencyFollowCommand, UserAgencyFollow>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateUserAgencyFollowCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserAgencyFollow> Handle(CreateUserAgencyFollowCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserAgencyFollowToAddDto.UserId);
            if (user == null)
                throw new Exception("Korisnik nije pronađen");

            var agency = await _unitOfWork.Agencies.GetByIdAsync(request.UserAgencyFollowToAddDto.AgencyId);
            if (agency == null)
                throw new Exception("Agencija nije pronađena");
                
            var userFollow = _mapper.Map<UserAgencyFollow>(request.UserAgencyFollowToAddDto);
            userFollow.Id = Guid.NewGuid().ToString();
            await _unitOfWork.UserAgencyFollows.AddAsync(userFollow);
            await _unitOfWork.Save();

            return userFollow;
        }
    }
}