using krov_nad_glavom_api.Data.DTO.AgencyRequest;
using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendNotificationsForAgencyRequestUpdate(AgencyRequestToUpdateDto agencyRequestToUpdateDto, AgencyRequest agencyRequest);
        Task<bool> SendNotificationsForAgencyRequestCreate(AgencyRequest agencyRequest);
        Task<bool> SendNotificationsForBuildingEndExtend(Building building);
        Task<bool> SendNotificationsForContractCreate(Contract contract);
        Task<bool> SendNotificationsForDiscountRequestCreate(DiscountRequest discountRequest);
        Task<bool> SendNotificationsForDiscountRequestForward(DiscountRequest discountRequest);
        Task<bool> SendNotificationsForDiscountRequestUpdate(DiscountRequest discountRequest);
        Task<bool> SendNotificationsForInstallmentProof(Installment installment);
        Task<bool> SendNotificationsForInstallmentConfirm(Installment installment);
        Task<bool> SendNotificationsForInstallmentCreate(Installment installment);
        Task<bool> SendNotificationsForNewAgencyFollow(UserAgencyFollow userAgencyFollow);
        Task<bool> SendNotificationsForLateContractInstllment(Contract contract);
        Task<bool> SendNotificationsForContractInvalidated(Contract contract);
        Task<bool> SendNotificationsForBuildingDelete(Building building);
        Task<bool> SendNotificationsForManagerRegister(User user);
    }
}