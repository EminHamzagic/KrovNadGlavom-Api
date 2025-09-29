using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.ConstructionCompanies
{
    public class CreateConstructionCompanyCommandHandler : IRequestHandler<CreateConstructionCompanyCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CreateConstructionCompanyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
		}

        public async Task<string> Handle(CreateConstructionCompanyCommand request, CancellationToken cancellationToken)
        {
            var existingCompany = await _unitOfWork.ConstructionCompanies.GetCompanyByName(request.ConstructionCompanyToAddDto.Name);
            if(existingCompany != null)
                throw new Exception("Ime kompanije je već zauzeto");
                
            var company = _mapper.Map<ConstructionCompany>(request.ConstructionCompanyToAddDto);
            company.Id = Guid.NewGuid().ToString();

            await _unitOfWork.ConstructionCompanies.AddAsync(company);
            await _unitOfWork.Save();
            return company.Id;
        }
    }
}