using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.User;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserToReturnDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserToReturnDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            if (user == null)
                throw new Exception("Korisnik nije pronađen");

            _mapper.Map(request.UserToUpdateDto, user);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.Save();

            return _mapper.Map<UserToReturnDto>(user);
        }
    }
}