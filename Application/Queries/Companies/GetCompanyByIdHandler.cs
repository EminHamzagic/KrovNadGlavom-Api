using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.ConstructionCompany;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.ConstructionCompanies
{
    public class GetCompanyByIdHandler : IRequestHandler<GetCompanyByIdQuery, CompanyToReturnDto>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public GetCompanyByIdHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<CompanyToReturnDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            // Dohvati kompaniju po Id-u
            var company = await _unitofWork.ConstructionCompanies.GetCompanyById(request.Id);

            if (company == null)
                throw new Exception("Kompanija nije pronađena");

            // Mapiraj na DTO
            var companyToReturn = _mapper.Map<CompanyToReturnDto>(company);

            // Ovde možeš dodati dodatne count/aggregate polja ako budu trebala, npr:
            // companyToReturn.NumberOfProjects = await _unitOfWork.Projects.GetCompanyProjectCount(company.Id);

            return companyToReturn;
        }
    }
}
