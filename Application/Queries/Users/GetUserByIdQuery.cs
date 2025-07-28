using krov_nad_glavom_api.Data.DTO.User;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Users
{
    public record GetUserByIdQuery(string Id) : IRequest<UserToReturnDto>;
}