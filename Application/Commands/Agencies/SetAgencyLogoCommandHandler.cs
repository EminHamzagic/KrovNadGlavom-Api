using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Agencies
{
    public class SetAgencyLogoCommandHandler : IRequestHandler<SetAgencyLogoCommand, string>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ICloudinaryService _cloudinaryService;

        public SetAgencyLogoCommandHandler(IUnitofWork unitofWork, ICloudinaryService cloudinaryService)
        {
            _unitofWork = unitofWork;
            _cloudinaryService = cloudinaryService;
        }
        
        public async Task<string> Handle(SetAgencyLogoCommand request, CancellationToken cancellationToken)
        {
            var agency = await _unitofWork.Agencies.GetByIdAsync(request.Dto.Id);
            if (agency == null)
                throw new Exception("Agencija nije pronađena");

            if (agency.LogoUrl != null)
            {
                await _cloudinaryService.DeleteImageAsync(agency.LogoUrl);
            }

            var imageUrl = await _cloudinaryService.UploadImageAsync(request.Dto.File, "KrovNadGlavom");
            agency.LogoUrl = imageUrl;
            _unitofWork.Agencies.Update(agency);
            await _unitofWork.Save();

            return imageUrl;
        }
    }
}