namespace GPNA.AlarmControlSystem.Interfaces;

public interface IAuthorizationService
{
    public Task<bool> GetAuthorizationStatus();
}