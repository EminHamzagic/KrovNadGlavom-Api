using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Apartments
{
    public class DeleteApartmentCommand : IRequest<string>
    {
        public string Id { get; set; }
        public DeleteApartmentCommand(string id)
        {
            Id = id;   
        }
    }
}