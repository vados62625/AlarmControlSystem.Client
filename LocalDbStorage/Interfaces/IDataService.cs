using LocalDbStorage.Repositories.Models;

namespace LocalDbStorage.Interfaces;

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

    /// <summary>
    /// Получить все Дочерние общества
    /// </summary>
    /// <returns></returns>
    Task<List<Branch>> GetAllBranch();

    /// <summary>
    /// Получить все Рабочие станции
    /// </summary>
    /// <returns></returns>
    Task<List<WorkStation>> GetAllWorkStation();

    /// <summary>
    /// Получить все Активные аварии
    /// </summary>
    /// <returns></returns>
    Task<List<ActiveAlarm>> GetAllActiveAlarm();

    /// <summary>
    /// Получить все Входящие аварии
    /// </summary>
    /// <returns></returns>
    Task<List<IncomingAlarm>> GetAllIncomingAlarm();

    /// <summary>
    /// Получить все Подавленные аварии
    /// </summary>
    /// <returns></returns>
    Task<List<SuppressedAlarm>> GetAllSuppressedAlarm();

    /// <summary>
    /// Получить все Теги
    /// </summary>
    /// <returns></returns>
    Task<List<Tag>> GetAllTag();

    /// <summary>
    /// Добавить тег
    /// </summary>
    /// <returns></returns>
    Task AddTag(Tag tag);

    /// <summary>
    /// Изменить тег
    /// </summary>
    /// <returns></returns>
    Task UpdateTag(Tag tag, int id);

    /// <summary>
    /// Удалить тег
    /// </summary>
    /// <returns></returns>
    Task DeleteTag(Tag tag, int id);
}