using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class SetUserProfileImageCommandHandler : IRequestHandler<SetUserProfileImageCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;

        public SetUserProfileImageCommandHandler(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
        }
        
        public async Task<string> Handle(SetUserProfileImageCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.InstallmentProofToSendDto.Id);
            if (user == null)
                throw new Exception("Korisnik nije pronađen");

            if (user.ImageUrl != null)
            {
                await _cloudinaryService.DeleteImageAsync(user.ImageUrl);
            }

            var imageUrl = await _cloudinaryService.UploadImageAsync(request.InstallmentProofToSendDto.File, "KrovNadGlavom");
            user.ImageUrl = imageUrl;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.Save();

            return imageUrl;
        }
    }
}