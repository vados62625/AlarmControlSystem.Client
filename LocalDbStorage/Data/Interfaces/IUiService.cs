namespace LocalDbStorage.Data.Interfaces;

public interface IUiService
{
    Task<Dictionary<DateTime, double>> CountOfIncomingAlarms(int idWorkStation, DateTime startDate, DateTime endDate);
    Task<Dictionary<DateTime, int>> CountOfActiveAlarms(int idWorkStation, DateTime startDate, DateTime endDate);
    Task<Dictionary<DateTime, int>> CountOfSuppressedAlarms(int idWorkStation, DateTime startDate, DateTime endDate);
}