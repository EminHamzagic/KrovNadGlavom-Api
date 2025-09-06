namespace krov_nad_glavom_api.Data.DTO.Agency
{
    public class AgencyToReturnDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PIB { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int NumberOfBuildings { get; set; }        
        public int NumberOfApartments { get; set; }        
    }
}