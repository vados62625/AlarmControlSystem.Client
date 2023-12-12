namespace GPNA.AlarmControlSystem.Models.Dto.TagTask;

public class UpdateTagTaskCommand
{
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// В архив
    /// </summary>
    public bool? IsArchived { get; set; }

    /// <summary>
    /// Добавить пользователей
    /// </summary>
    public int[]? UserIdArray { get; set; } = Array.Empty<int>();
}