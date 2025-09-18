using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Apartments
{
    public class UpdateApartmentCommandHandler : IRequestHandler<UpdateApartmentCommand, Apartment>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;
        public UpdateApartmentCommandHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitofWork = unitofWork;
        }

        public async Task<Apartment> Handle(UpdateApartmentCommand request, CancellationToken cancellationToken)
        {
            var apartment = await _unitofWork.Apartments.GetApartmentById(request.Id);
            if (apartment == null)
                throw new Exception("Stan nije pronađen");

            _mapper.Map(request.ApartmentToUpdateDto, apartment);

            await _unitofWork.Save();

            return apartment;
        }
    }
}