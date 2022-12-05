using LocalDbStorage.Repositories.Base;

namespace LocalDbStorage.Repositories.Models;

/// <summary>
/// Рабочая станция
/// </summary>
public class WorkStation : EntityBase
{
    /// <summary>
    /// Id Дочернего общества
    /// </summary>
    public int BranchId { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Метка удаления записи
    /// </summary>
    public bool Del { get; set; }

    public virtual Branch Branch { get; set; } = null!;

}