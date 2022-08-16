using LocalDbStorage.Repositories.Models;

namespace LocalDbStorage.Data.Interfaces;

public interface IDataService
{
    /// <summary>
    /// Возвращает все записи по Id рабочей станции за определенный период
    /// </summary>
    /// <param name="idWorkStation"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    Task<List<IncomingAlarm>> GetScopeIncomingAlarm(int idWorkStation, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Возвращает все записи по Id рабочей станции за определенный период
    /// </summary>
    /// <param name="idWorkStation"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    Task<List<ActiveAlarm>> GetScopeActiveAlarm(int idWorkStation, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Возвращает все записи по Id рабочей станции за определенный период
    /// </summary>
    /// <param name="idWorkStation"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    Task<List<SuppressedAlarm>> GetScopeSuppressedAlarm(int idWorkStation, DateTime startDate, DateTime endDate);
}