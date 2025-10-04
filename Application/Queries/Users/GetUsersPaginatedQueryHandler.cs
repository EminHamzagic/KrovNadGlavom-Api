using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.User;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Users
{
    public class GetUsersPaginatedQueryHandler : IRequestHandler<GetUsersPaginatedQuery, PaginatedResponse<UserToReturnDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetUsersPaginatedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
		}

        public async Task<PaginatedResponse<UserToReturnDto>> Handle(GetUsersPaginatedQuery request, CancellationToken cancellationToken)
        {
            var (usersPage, totalCount, totalPages) = await _unitOfWork.Users.GetUsersPage(request.parameters);

            var usersToReturn = _mapper.Map<List<UserToReturnDto>>(usersPage);
            
            return new PaginatedResponse<UserToReturnDto>(
                usersToReturn,
                totalCount,
                request.parameters.PageNumber,
                request.parameters.PageSize,
                totalPages
            );
        }
    }
}