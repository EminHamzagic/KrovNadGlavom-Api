using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.AgencyRequests
{
    public class DeleteAgencyRequestCommandHandler : IRequestHandler<DeleteAgencyRequestCommand, AgencyRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAgencyRequestCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AgencyRequest> Handle(DeleteAgencyRequestCommand request, CancellationToken cancellationToken)
        {
            var agencyRequest = await _unitOfWork.AgencyRequests.GetByIdAsync(request.Id);
            if (agencyRequest == null)
                throw new Exception("Zahtev nije pronađen");

            agencyRequest.IsDeleted = true;
            _unitOfWork.AgencyRequests.Update(agencyRequest);
            await _unitOfWork.Save();

            return agencyRequest;
        }
    }
}