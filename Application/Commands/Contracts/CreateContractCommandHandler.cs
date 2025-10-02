using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Contracts
{
    public class CreateContractCommandHandler : IRequestHandler<CreateContractCommand, Contract>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
		private readonly INotificationService _notificationService;

		public CreateContractCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
			_notificationService = notificationService;
		}

        public async Task<Contract> Handle(CreateContractCommand request, CancellationToken cancellationToken)
        {
            var apartment = await _unitOfWork.Apartments.GetByIdAsync(request.ContractToAddDto.ApartmentId);
            if (apartment == null)
                throw new Exception("Stan nije pronađen");

            if(!apartment.IsAvailable)
                throw new Exception("Stan je već kupljen");

            var contract = _mapper.Map<Contract>(request.ContractToAddDto);
            contract.Id = Guid.NewGuid().ToString();

            await _unitOfWork.Contracts.AddAsync(contract);

            await _notificationService.SendNotificationsForContractCreate(contract);

            var firstInstallment = new Installment
            {
                Id = Guid.NewGuid().ToString(),
                ContractId = contract.Id,
                SequenceNumber = 1,
                Amount = contract.InstallmentAmount,
                DueDate = DateTime.Now.AddDays(30),
                IsConfirmed = false,
                CreatedAt = DateTime.Now
            };
            await _unitOfWork.Installments.AddAsync(firstInstallment);

            apartment.IsAvailable = false;
            _unitOfWork.Apartments.Update(apartment);
            await _unitOfWork.Save();

            return contract;
        }
    }
}