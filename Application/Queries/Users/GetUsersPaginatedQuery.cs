using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.User;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Users
{
    public record GetUsersPaginatedQuery(QueryStringParameters parameters) : IRequest<PaginatedResponse<UserToReturnDto>>;
}