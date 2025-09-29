using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.User;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Users
{
	public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserToReturnDto>
	{
        private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<UserToReturnDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
		{
			var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
			if (user == null)
				throw new Exception("Korisnik nije pronađen");

			var userToReturn = _mapper.Map<User, UserToReturnDto>(user);
			userToReturn.Reservation = await _unitOfWork.Reservations.GetReservationByUserId(user.Id);

			return userToReturn;
        }
	}
}