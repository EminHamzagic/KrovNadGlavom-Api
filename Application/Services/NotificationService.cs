using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.AgencyRequest;
using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> SendNotificationsForAgencyRequestUpdate(AgencyRequestToUpdateDto agencyRequestToUpdateDto, AgencyRequest agencyRequest)
        {
            var agencyUser = await _unitOfWork.Users.GetUserByAgencyId(agencyRequest.AgencyId);
            if (agencyRequestToUpdateDto.Status == "Approved")
            {
                var users = await _unitOfWork.UserAgencyFollows.GetAgencyFollowers(agencyRequest.AgencyId);
                if (users != null)
                {
                    var notifications = new List<Notification>();
                    foreach (var user in users)
                    {
                        notifications.Add(new Notification
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = user.Id,
                            Label = NotificationsLabelEnum.Novo,
                            Title = "Novi stanovi u ponudi",
                            Message = "Agencija koju pratite je uzela novu zgradu! Idite na stranicu <a href='/apartments' class='text-primary underline' target='_blank'>Stanovi</a> da bi ste pregledali nove stanove u ponudi.",
                            CreatedAt = DateTime.Now
                        });
                    }
                    notifications.Add(new Notification
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = agencyUser.Id,
                        Label = NotificationsLabelEnum.Zahtev,
                        Title = "Zahtev za zgradu prihvaćen",
                        Message = $"Kompanija je prihvatila vaš zahtev za <a href='/buildings/{agencyRequest.BuildingId}' class='text-primary underline' target='_blank'>zgradu</a>",
                        CreatedAt = DateTime.Now
                    });

                    await _unitOfWork.Notifications.AddRangeAsync(notifications);
                }


            }
            else if (agencyRequestToUpdateDto.Status == "Rejected")
            {
                await _unitOfWork.Notifications.AddAsync(new Notification
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = agencyUser.Id,
                    Label = NotificationsLabelEnum.Zahtev,
                    Title = "Zahtev za zgradu odbijen",
                    Message = $"Kompanija je odbila vaš zahtev za <a href='/buildings/{agencyRequest.BuildingId}' class='text-primary underline' target='_blank'>zgradu</a>. Idi na stranicu <a href='/requests' class='text-primary underline' target='_blank'>zahtevi</a> da bi video razlog odbijanja.",
                    CreatedAt = DateTime.Now
                });
            }

            await _unitOfWork.Save();

            return true;
        }

        public async Task<bool> SendNotificationsForAgencyRequestCreate(AgencyRequest agencyRequest)
        {
            var building = await _unitOfWork.Buildings.GetByIdAsync(agencyRequest.BuildingId);
            var comapnyUser = await _unitOfWork.Users.GetUserByCompanyId(building.CompanyId);

            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                UserId = comapnyUser.Id,
                Label = NotificationsLabelEnum.Zahtev,
                Title = "Novi zahtev za zgradu",
                Message = $@"Dobili ste novi zahtev za <a href='/buildings/{building.Id}' class='text-primary underline' target='_blank'>zgradu</a> od strance 
                            <a href='/agency/{agencyRequest.AgencyId}' class='text-primary underline' target='_blank'>agencije</a>. 
                            <span class='mt-5'>Idi na <a href='/requests' class='text-primary underline' target='_blank'>zahteve</a></span>",
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.Save();

            return true;
        }

        public async Task<bool> SendNotificationsForBuildingEndExtend(Building building)
        {
            var apartments = await _unitOfWork.Apartments.GetApartmentsByBuildingId(building.Id);
            var apartmentIds = apartments.Select(a => a.Id).ToList();
            var contracts = await _unitOfWork.Contracts.GetContractsByApartmentIds(apartmentIds);
            var userIds = contracts.Select(c => c.UserId).ToList();

            var notifications = new List<Notification>();
            foreach (var userId in userIds)
            {
                notifications.Add(new Notification
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    Label = NotificationsLabelEnum.Obaveštenje,
                    Title = "Obaveštenje o produženju",
                    Message = $"Izgrada zgrade vašeg stana je produženo do {building.ExtendedUntil?.ToString("dd.MM.yyyy")}. Za svaki mesec produženja biće vam uplaćena određena suma od strane kompanije.",
                    CreatedAt = DateTime.Now
                });
            }

            await _unitOfWork.Notifications.AddRangeAsync(notifications);
            await _unitOfWork.Save();

            return true;
        }

        public async Task<bool> SendNotificationsForContractCreate(Contract contract)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(contract.UserId);
            var agencyUser = await _unitOfWork.Users.GetUserByAgencyId(contract.AgencyId);

            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                UserId = agencyUser.Id,
                Label = NotificationsLabelEnum.Kupovina,
                Title = "Kupovina stana",
                Message = $@"Korisnik {user.Name} {user.Lastname} je uspešno kupio <a href='/apartments/{contract.ApartmentId}' class='text-primary underline' target='_blank'>stan</a>.
                            Kliknite <a href='/contracts/{contract.Id}' class='text-primary underline' target='_blank'>ovde</a> da bi ste pregledali ugovor.",
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.Save();

            return true;
        }

        public async Task<bool> SendNotificationsForDiscountRequestCreate(DiscountRequest discountRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(discountRequest.UserId);
            var agencyUser = await _unitOfWork.Users.GetUserByAgencyId(discountRequest.AgencyId);

            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                UserId = agencyUser.Id,
                Label = NotificationsLabelEnum.Popust,
                Title = "Zahtev za popust",
                Message = $@"Korisnik {user.Name} {user.Lastname} je poslao zahtev za popust za <a href='/apartments/{discountRequest.ApartmentId}' class='text-primary underline' target='_blank'>stan</a>.
                            Kliknite <a href='/discount-requests' class='text-primary underline' target='_blank'>ovde</a> da bi ste pregledali zahteve za popust.",
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.Save();

            return true;
        }

        public async Task<bool> SendNotificationsForDiscountRequestForward(DiscountRequest discountRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(discountRequest.UserId);
            var companyUser = await _unitOfWork.Users.GetUserByCompanyId(discountRequest.ConstructionCompanyId);
            var agency = await _unitOfWork.Agencies.GetByIdAsync(discountRequest.AgencyId);

            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                UserId = companyUser.Id,
                Label = NotificationsLabelEnum.Popust,
                Title = "Zahtev za popust",
                Message = $@"Agencija {agency.Name} vam je prosledila zahtev za popust korisnika {user.Name} {user.Lastname} za <a href='/apartments/{discountRequest.ApartmentId}' class='text-primary underline' target='_blank'>stan</a>.
                            Kliknite <a href='/discount-requests' class='text-primary underline' target='_blank'>ovde</a> da bi ste pregledali zahteve za popust.",
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.Save();

            return true;
        }

        public async Task<bool> SendNotificationsForDiscountRequestUpdate(DiscountRequest discountRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(discountRequest.UserId);

            if (discountRequest.Status == "Approved")
            {

                var notification = new Notification
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    Label = NotificationsLabelEnum.Popust,
                    Title = "Zahtev za popust prihvaćen",
                    Message = $@"Vaš zahtev za popust za ovaj <a href='/apartments/{discountRequest.ApartmentId}' class='text-primary underline' target='_blank'>stan</a> je prihvaćen.",
                    CreatedAt = DateTime.Now
                };
                await _unitOfWork.Notifications.AddAsync(notification);
            }
            else if (discountRequest.Status == "Rejected")
            {
                var notification = new Notification
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    Label = NotificationsLabelEnum.Popust,
                    Title = "Zahtev za popust odbijen",
                    Message = $@"Vaš zahtev za popust za ovaj <a href='/apartments/{discountRequest.ApartmentId}' class='text-primary underline' target='_blank'>stan</a> je odbijen.
                                Kliknite <a href='/discount-requests' class='text-primary underline' target='_blank'>ovde</a> da bi ste videli razlog odbijanja.",
                    CreatedAt = DateTime.Now
                };
                await _unitOfWork.Notifications.AddAsync(notification);
            }

            await _unitOfWork.Save();

            return true;
        }

        public async Task<bool> SendNotificationsForInstallmentProof(Installment installment)
        {
            var contract = await _unitOfWork.Contracts.GetByIdAsync(installment.ContractId);
            var user = await _unitOfWork.Users.GetByIdAsync(contract.UserId);
            var agencyUser = await _unitOfWork.Users.GetUserByAgencyId(contract.AgencyId);

            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                UserId = agencyUser.Id,
                Label = NotificationsLabelEnum.Placanje,
                Title = "Novi dokaz uplate",
                Message = $@"Korisnik {user.Name} {user.Lastname} je dodao dokaz o {installment.SequenceNumber}. uplati za ovaj <a href='/contracts/{contract.Id}' class='text-primary underline' target='_blank'>ugovor</a>.",
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.Save();

            return true;
        }

        public async Task<bool> SendNotificationsForInstallmentConfirm(Installment installment)
        {
            var contract = await _unitOfWork.Contracts.GetByIdAsync(installment.ContractId);
            var user = await _unitOfWork.Users.GetByIdAsync(contract.UserId);
            var agency = await _unitOfWork.Agencies.GetByIdAsync(contract.AgencyId);

            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                Label = NotificationsLabelEnum.Placanje,
                Title = "Rata potvrđena",
                Message = $@"Agencija {agency.Name} je potvrdila uplatu za ratu pod rednim brojem {installment.SequenceNumber}. 
                            Kliknite <a href='/contracts/{contract.Id}' class='text-primary underline' target='_blank'>ovde</a> da vidite ugovor",
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.Save();

            return true;
        }

        public async Task<bool> SendNotificationsForInstallmentCreate(Installment installment)
        {
            var contract = await _unitOfWork.Contracts.GetByIdAsync(installment.ContractId);
            var user = await _unitOfWork.Users.GetByIdAsync(contract.UserId);

            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                Label = NotificationsLabelEnum.Placanje,
                Title = "Nova rata",
                Message = $@"Kreirana je nova rata na <a href='/contracts/{contract.Id}' class='text-primary underline' target='_blank'>ovom</a> ugovoru.
                            Rok uplate za novu ratu je {installment.DueDate:dd.MM.yyyy}",
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.Save();

            return true;
        }

        public async Task<bool> SendNotificationsForNewAgencyFollow(UserAgencyFollow userAgencyFollow)
        {
            var user = await _unitOfWork.Users.GetUserByAgencyId(userAgencyFollow.AgencyId);

            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                Label = NotificationsLabelEnum.Obaveštenje,
                Title = "Novi pratilac",
                Message = $@"Vaša agencija je dobila jednog novog pratioca",
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.Save();

            return true;
        }

        public async Task<bool> SendNotificationsForLateContractInstllment(Contract contract)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                UserId = contract.UserId,
                Label = NotificationsLabelEnum.Placanje,
                Title = "Kašnjenje uplate rate",
                Message = $@"Zakasnili ste sa plaćanjem poslednje rate na ovom <a href='/contracts/{contract.Id}' class='text-primary underline' target='_blank'>ugovoru</a>.
                             Ako zakasnite sa plaćanjem rate jos {4 - contract.LateCount} puta, ugovor će biti raskinut. Molimo vas obratite pažnju!",
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.Save();

            return true;
        }

        public async Task<bool> SendNotificationsForContractInvalidated(Contract contract)
        {
            var agencyUser = await _unitOfWork.Users.GetUserByAgencyId(contract.AgencyId);

            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = contract.UserId,
                    Label = NotificationsLabelEnum.Obaveštenje,
                    Title = "Raskidanje ugovora",
                    Message = $@"Poštovani, kako se zakasnili sa uplatom rate na ugovoru pod brojem {contract.Id.Substring(0, 6)} je od ovog trenutka zvanično raskinut.
                            Biće vam vraćena količina nova u uznosu od 90% ukupne uplaćene svote, gde ostalih 10% ide agenciji u vidu nadoknade.",
                    CreatedAt = DateTime.Now
                },
                new Notification
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = agencyUser.Id,
                    Label = NotificationsLabelEnum.Obaveštenje,
                    Title = "Ugovor raskinut",
                    Message = $@"Ugovor pod brojem {contract.Id.Substring(0, 6)} je od ovog trenutka raskinut. Razlog: kupac je 4. put kasnio sa uplatom mesečne rate.
                                <a href='/apartments/{contract.ApartmentId}' class='text-primary underline' target='_blank'>Stan</a> pod ovim ugovorom je ponovo u prodaji.",
                    CreatedAt = DateTime.Now
                }
			};

            await _unitOfWork.Notifications.AddRangeAsync(notifications);
            await _unitOfWork.Save();

            return true;
        }
    }
}