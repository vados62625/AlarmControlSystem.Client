using LocalDbStorage.Repositories.Models.Enum;

namespace LocalDbStorage.Dto;

/// <summary>
/// Активные сигнализации
/// </summary>
public partial class SuppressedAlarmDto : DtoBase
{

    /// <summary>
    /// Длительность
    /// </summary>
    public TimeSpan? Duration { get; set; }
}