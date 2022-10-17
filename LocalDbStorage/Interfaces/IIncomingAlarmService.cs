using LocalDbStorage.Dto;
using LocalDbStorage.Repositories.Models;

namespace LocalDbStorage.Interfaces;

public interface IIncomingAlarmService
{
    /// <summary>
    /// Получить все аварии
    /// </summary>
    /// <returns></returns>
    Task<List<IncomingAlarmDto>> GetAllAlarms();

    /// <summary>
    /// Изменить запись в БД
    /// </summary>
    /// <returns></returns>
    Task UpdateAlarm(IncomingAlarmDto incomingAlarm);

    /// <summary>
    /// Возвращает все записи по Id рабочей станции за определенный период
    /// </summary>
    /// <param name="idWorkStation"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    Task<List<IncomingAlarmDto>> GetScopeAlarms(int idWorkStation, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Возвращает среднее количество аварий за сутки
    /// </summary>
    /// <param name="idWorkStation"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    Task<Dictionary<DateTime, double>> GetAverageAlarmsByDates(int idWorkStation, DateTime startDate, DateTime endDate);

    Task<List<List<IncomingAlarmDto>>> GetAlarmsPerDate(int idWorkStation, DateTime startDate, DateTime endDate);

    Task<Dictionary<DateTime, List<IncomingAlarmDto>>> GetCountInHour(int idWorkStation, DateTime startDate,
        DateTime endDate);
}