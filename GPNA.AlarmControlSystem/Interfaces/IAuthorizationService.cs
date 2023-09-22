namespace GPNA.AlarmControlSystem.Interfaces;

public interface IAuthorizationService
{
    public Task<string> GetAndSaveApiToken();

    public Task<bool> GetAuthorizationStatus();
}