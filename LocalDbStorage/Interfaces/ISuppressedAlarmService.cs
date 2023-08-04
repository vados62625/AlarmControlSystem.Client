using LocalDbStorage.Dto;

namespace LocalDbStorage.Interfaces;

public interface ISuppressedAlarmService
{
    /// <summary>
    /// Получить все Подавленные аварии
    /// </summary>
    /// <returns></returns>
    Task<List<SuppressedAlarmDto>> GetAllAlarms();

    /// <summary>
    /// Изменить запись в БД
    /// </summary>
    /// <returns></returns>
    Task UpdateAlarm(SuppressedAlarmDto suppressedAlarm);

    /// <summary>
    /// Возвращает все записи по Id рабочей станции за определенный период
    /// </summary>
    /// <param name="idWorkStation"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    Task<List<SuppressedAlarmDto>> GetScopeAlarms(int idWorkStation, DateTime startDate, DateTime endDate);

    Task<Dictionary<DateTime, int>> CountOfSuppressedAlarms(int idWorkStation, DateTime startDate, DateTime endDate);
}