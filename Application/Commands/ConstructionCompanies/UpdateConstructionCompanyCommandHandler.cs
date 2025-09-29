using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.ConstructionCompanies
{
    public class UpdateConstructionCompanyCommandHandler : IRequestHandler<UpdateConstructionCompanyCommand, ConstructionCompany>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateConstructionCompanyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ConstructionCompany> Handle(UpdateConstructionCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.ConstructionCompanies.GetByIdAsync(request.Id);
            if (company == null)
                throw new Exception("Kompanija nije pronađena");

            var existingCompany = await _unitOfWork.ConstructionCompanies.GetCompanyByName(request.ConstructionCompanyToUpdateDto.Name);
            if (existingCompany != null && existingCompany.Id != company.Id)
                throw new Exception("Ime kompanije je već zauzeto");

            _mapper.Map(request.ConstructionCompanyToUpdateDto, company);
            _unitOfWork.ConstructionCompanies.Update(company);
            await _unitOfWork.Save();

            return company;
        }
    }
}