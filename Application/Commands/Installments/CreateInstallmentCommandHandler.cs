using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Installments
{
    public class CreateInstallmentCommandHandler : IRequestHandler<CreateInstallmentCommand, Installment>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateInstallmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Installment> Handle(CreateInstallmentCommand request, CancellationToken cancellationToken)
        {
            var installment = _mapper.Map<Installment>(request.InstallmentToAddDto);
            installment.Id = Guid.NewGuid().ToString();

            await _unitOfWork.Installments.AddAsync(installment);
            await _unitOfWork.Save();

            return installment;
        }
    }
}