using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.User;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Users.GetUserById
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
				throw new Exception("User not found");

			return _mapper.Map<User, UserToReturnDto>(user);
        }
	}
}