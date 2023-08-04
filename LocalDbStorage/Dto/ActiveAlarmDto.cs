namespace LocalDbStorage.Dto;

/// <summary>
/// Активные сигнализации
/// </summary>
public partial class ActiveAlarmDto : DtoBase
{

    /// <summary>
    /// Длительность
    /// </summary>
    public TimeSpan? Duration { get; set; }
}