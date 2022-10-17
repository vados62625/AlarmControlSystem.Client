using LocalDbStorage.Repositories.Models;
using ActiveAlarmDto = LocalDbStorage.Dto.ActiveAlarmDto;

namespace LocalDbStorage.Interfaces;

public interface IActiveAlarmService
{
    /// <summary>
    /// Получить все Активные аварии
    /// </summary>
    /// <returns></returns>
    Task<List<ActiveAlarmDto>> GetAllAlarms();

    /// <summary>
    /// Изменить запись в БД
    /// </summary>
    /// <returns></returns>
    Task UpdateAlarm(ActiveAlarmDto activeAlarm);

    /// <summary>
    /// Возвращает все записи по Id рабочей станции за определенный период
    /// </summary>
    /// <param name="idWorkStation"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    Task<List<ActiveAlarmDto>> GetScopeAlarms(int idWorkStation, DateTime startDate, DateTime endDate);

    Task<Dictionary<DateTime, int>> CountOfActiveAlarms(int idWorkStation, DateTime startDate, DateTime endDate);
}