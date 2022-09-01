using LocalDbStorage.Repositories.Base;
using LocalDbStorage.Repositories.Models;


namespace LocalDbStorage.Dto;

/// <summary>
/// Входящие аварии
/// </summary>
public class IncomingAlarmDto
{
    /// <summary>
    /// Id рабочей станции
    /// </summary>
    public int IdWorkStation { get; set; }

    /// <summary>
    /// Имя тега
    /// </summary>
    public string TagName { get; set; }

    /// <summary>
    /// Дата
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Количество
    /// </summary>
    public int? Count { get; set; }

    public List<IncomingAlarm> IncomingAlarms { get; set; }

}