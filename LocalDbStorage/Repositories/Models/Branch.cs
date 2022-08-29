using LocalDbStorage.Repositories.Base;

namespace LocalDbStorage.Repositories.Models;

/// <summary>
/// Кусты
/// </summary>
public class Branch : EntityBase
{
    public Branch()
    {
        WorkStations = new HashSet<WorkStation>();
    }

    /// <summary>
    /// Имя
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Метка удаления записи
    /// </summary>
    public bool Del { get; set; }

    public virtual ICollection<WorkStation> WorkStations { get; set; }
}