using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class SetUserProfileImageCommandHandler : IRequestHandler<SetUserProfileImageCommand, string>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ICloudinaryService _cloudinaryService;

        public SetUserProfileImageCommandHandler(IUnitofWork unitofWork, ICloudinaryService cloudinaryService)
        {
            _unitofWork = unitofWork;
            _cloudinaryService = cloudinaryService;
        }
        
        public async Task<string> Handle(SetUserProfileImageCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitofWork.Users.GetByIdAsync(request.InstallmentProofToSendDto.Id);
            if (user == null)
                throw new Exception("Korisnik nije pronađen");

            if (user.ImageUrl != null)
            {
                await _cloudinaryService.DeleteImageAsync(user.ImageUrl);
            }

            var imageUrl = await _cloudinaryService.UploadImageAsync(request.InstallmentProofToSendDto.File, "KrovNadGlavom");
            user.ImageUrl = imageUrl;
            _unitofWork.Users.Update(user);
            await _unitofWork.Save();

            return imageUrl;
        }
    }
}