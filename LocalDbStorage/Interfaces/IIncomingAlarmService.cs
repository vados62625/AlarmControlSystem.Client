using LocalDbStorage.Repositories.Models;

namespace LocalDbStorage.Interfaces;

public interface IIncomingAlarmService
{
    /// <summary>
    /// Получить все аварии
    /// </summary>
    /// <returns></returns>
    Task<List<IncomingAlarm>> GetAllAlarms();

    /// <summary>
    /// Изменить запись в БД
    /// </summary>
    /// <returns></returns>
    Task UpdateAlarm(IncomingAlarm incomingAlarm, int id);

    /// <summary>
    /// Возвращает все записи по Id рабочей станции за определенный период
    /// </summary>
    /// <param name="idWorkStation"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    Task<List<IncomingAlarm>> GetScopeAlarms(int idWorkStation, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Возвращает среднее количество аварий за сутки
    /// </summary>
    /// <param name="idWorkStation"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    Task<Dictionary<DateTime, double>> GetAverageAlarmsByDates(int idWorkStation, DateTime startDate, DateTime endDate);

    Task<List<List<IncomingAlarm>>> GetAlarmsPerDate(int idWorkStation, DateTime startDate, DateTime endDate);

    Task<Dictionary<DateTime, List<IncomingAlarm>>> GetCountInHour(int idWorkStation, DateTime startDate,
        DateTime endDate);
}