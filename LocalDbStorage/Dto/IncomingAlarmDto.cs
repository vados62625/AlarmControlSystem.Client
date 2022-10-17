using LocalDbStorage.Repositories.Base;
using LocalDbStorage.Repositories.Models;


namespace LocalDbStorage.Dto;

/// <summary>
/// Входящие аварии
/// </summary>
public class IncomingAlarmDto : DtoBase
{
    /// <summary>
    /// Дата
    /// </summary>
    public DateTime EndDate { get; set; }
    
    /// <summary>
    /// Тест
    /// </summary>
    public bool Test { get; set; }
}