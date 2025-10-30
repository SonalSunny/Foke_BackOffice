namespace FOKE.Services.Interface
{
    public interface IAuthenticationServiceRepository
    {
        Task<string> GenerateToken(string DevicePrimaryId, string deviceId, string civilId);

    }
}
