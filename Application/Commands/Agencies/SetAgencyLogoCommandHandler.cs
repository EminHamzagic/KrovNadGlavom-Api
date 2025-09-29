using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Agencies
{
    public class SetAgencyLogoCommandHandler : IRequestHandler<SetAgencyLogoCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;

        public SetAgencyLogoCommandHandler(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
        }
        
        public async Task<string> Handle(SetAgencyLogoCommand request, CancellationToken cancellationToken)
        {
            var agency = await _unitOfWork.Agencies.GetByIdAsync(request.Dto.Id);
            if (agency == null)
                throw new Exception("Agencija nije pronađena");

            if (agency.LogoUrl != null)
            {
                await _cloudinaryService.DeleteImageAsync(agency.LogoUrl);
            }

            var imageUrl = await _cloudinaryService.UploadImageAsync(request.Dto.File, "KrovNadGlavom");
            agency.LogoUrl = imageUrl;
            _unitOfWork.Agencies.Update(agency);
            await _unitOfWork.Save();

            return imageUrl;
        }
    }
}