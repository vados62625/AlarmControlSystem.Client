using LocalDbStorage.Dto;
using LocalDbStorage.Repositories.Models;

namespace LocalDbStorage.Interfaces;

public interface IBufferAlarmService
{
    /// <summary>
    /// Получить все активные сигнализации
    /// </summary>
    /// <returns></returns>
    Task<List<ActiveAlarmDto>> GetAllActiveAlarms();

    /// <summary>
    /// Получить все входящие сигнализации
    /// </summary>
    /// <returns></returns>
    Task<List<IncomingAlarmDto>> GetAllIncomingAlarms();

    /// <summary>
    /// Получить все подавленные сигнализации
    /// </summary>
    /// <returns></returns>
    Task<List<SuppressedAlarmDto>> GetAllSuppressedAlarms();

    /// <summary>
    /// Получить все сигнализации
    /// </summary>
    /// <returns></returns>
    Task<List<BufferAlarm>> GetAllBufferAlarms();

    /// <summary>
    /// Изменить запись в БД
    /// </summary>
    /// <returns></returns>
    Task UpdateBufferAlarm(BufferAlarm bufferAlarm);
}