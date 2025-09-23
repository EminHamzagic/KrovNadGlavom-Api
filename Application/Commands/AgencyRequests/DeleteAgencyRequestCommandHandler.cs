using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.AgencyRequests
{
    public class DeleteAgencyRequestCommandHandler : IRequestHandler<DeleteAgencyRequestCommand, AgencyRequest>
    {
        private readonly IUnitofWork _unitofWork;

        public DeleteAgencyRequestCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<AgencyRequest> Handle(DeleteAgencyRequestCommand request, CancellationToken cancellationToken)
        {
            var agencyRequest = await _unitofWork.AgencyRequests.GetByIdAsync(request.Id);
            if (agencyRequest == null)
                throw new Exception("Zahtev nije pronađen");

            agencyRequest.IsDeleted = true;
            await _unitofWork.Save();

            return agencyRequest;
        }
    }
}