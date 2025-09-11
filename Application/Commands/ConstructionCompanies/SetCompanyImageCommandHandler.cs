using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.ConstructionCompanies
{
    public class SetCompanyImageCommandHandler : IRequestHandler<SetCompanyImageCommand, string>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ICloudinaryService _cloudinaryService;

        public SetCompanyImageCommandHandler(IUnitofWork unitofWork, ICloudinaryService cloudinaryService)
        {
            _unitofWork = unitofWork;
            _cloudinaryService = cloudinaryService;
        }
        
        public async Task<string> Handle(SetCompanyImageCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitofWork.ConstructionCompanies.GetByIdAsync(request.Dto.Id);
            if (company == null)
                throw new Exception("Kompanija nije pronađena");

            if (company.LogoUrl != null)
            {
                await _cloudinaryService.DeleteImageAsync(company.LogoUrl);
            }

            var imageUrl = await _cloudinaryService.UploadImageAsync(request.Dto.File, "KrovNadGlavom");
            company.LogoUrl = imageUrl;
            await _unitofWork.Save();

            return imageUrl;
        }
    }
}