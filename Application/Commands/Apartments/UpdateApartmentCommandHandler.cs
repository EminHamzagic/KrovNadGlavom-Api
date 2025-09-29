using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Apartments
{
    public class UpdateApartmentCommandHandler : IRequestHandler<UpdateApartmentCommand, Apartment>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UpdateApartmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Apartment> Handle(UpdateApartmentCommand request, CancellationToken cancellationToken)
        {
            var apartment = await _unitOfWork.Apartments.GetApartmentById(request.Id);
            if (apartment == null)
                throw new Exception("Stan nije pronađen");

            _mapper.Map(request.ApartmentToUpdateDto, apartment);
            _unitOfWork.Apartments.Update(apartment);

            await _unitOfWork.Save();

            return apartment;
        }
    }
}