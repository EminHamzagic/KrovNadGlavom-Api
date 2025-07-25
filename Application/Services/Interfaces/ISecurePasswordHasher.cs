namespace krov_nad_glavom_api.Application.Services.Interfaces
{
    public interface ISecurePasswordHasher
    {
        string Hash(string password);
        bool Verify(string hash, string password);
    }
}