using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.User;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Users
{
	public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserToReturnDto>
	{
        private readonly IUnitofWork _unitofWork;
		private readonly IMapper _mapper;

		public GetUserByIdQueryHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
			_mapper = mapper;
		}
		public async Task<UserToReturnDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
		{
			var user = await _unitofWork.Users.GetByIdAsync(request.Id);
			if (user == null)
				throw new Exception("Korisnik nije pronađen");

			var userToReturn = _mapper.Map<User, UserToReturnDto>(user);
			userToReturn.Reservation = await _unitofWork.Reservations.GetReservationByUserId(user.Id);

			return userToReturn;
        }
	}
}