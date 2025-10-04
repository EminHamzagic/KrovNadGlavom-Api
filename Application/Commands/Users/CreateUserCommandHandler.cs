using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
		private readonly ISecurePasswordHasher _securePasswordHasher;
        private readonly IEmailService _emailService;
		private readonly ITokenService _tokenService;
		private readonly INotificationService _notificationService;

		public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ISecurePasswordHasher securePasswordHasher, IEmailService emailService, ITokenService tokenService, INotificationService notificationService)
        {
            _emailService = emailService;
			_tokenService = tokenService;
			_notificationService = notificationService;
			_mapper = mapper;
			_securePasswordHasher = securePasswordHasher;
			_unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _unitOfWork.Users.GetUserByEmail(request.UserToAddDto.Email);
            if(existingUser != null)
                throw new Exception("Postoji korisnik sa unetim email-om");
                
            var user = _mapper.Map<User>(request.UserToAddDto);
            user.Id = Guid.NewGuid().ToString();
            user.PasswordHash = _securePasswordHasher.Hash(request.UserToAddDto.Password);
            user.IsVerified = false;
            if (user.Role == "Manager")
            {
                user.IsAllowed = false;
                await _notificationService.SendNotificationsForManagerRegister(user);
            }
            else
                user.IsAllowed = true;

            await _unitOfWork.Users.AddAsync(user);

            var token = _tokenService.GenerateEmailVerificationToken(user.Id, user.Email);
            var link = $"{request.Origin}/verify-email?token={token}";

            await _emailService.SendEmailAsync(user.Email, "Verifikacija email-a", "Verifikacija email-a", _emailService.GetEmailVerificationHtmlBody(link));
            await _unitOfWork.Save();

            return user.Id;
        }
    }
}