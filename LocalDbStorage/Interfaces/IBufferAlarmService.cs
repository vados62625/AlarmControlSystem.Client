using LocalDbStorage.Repositories.Models;

namespace LocalDbStorage.Interfaces;

public interface IBufferAlarmService
{
    /// <summary>
    /// Получить все сигнализации
    /// </summary>
    /// <returns></returns>
    Task<List<BufferAlarm>> GetAllAlarms();

    /// <summary>
    /// Возвращает все записи по Id рабочей станции за определенный период
    /// </summary>
    /// <param name="idWorkStation"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    Task<List<BufferAlarm>> GetScopeAlarms(int idWorkStation, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Изменить запись в БД
    /// </summary>
    /// <returns></returns>
    Task UpdateAlarm(BufferAlarm activeAlarm);
}