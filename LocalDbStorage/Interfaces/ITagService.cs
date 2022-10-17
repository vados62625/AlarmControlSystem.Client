using LocalDbStorage.Repositories.Models;

namespace LocalDbStorage.Interfaces;

public interface ITagService
{
    /// <summary>
    /// Получить все Теги
    /// </summary>
    /// <returns></returns>
    Task<List<Tag>> GetAllTags();

    /// <summary>
    /// Добавить тег
    /// </summary>
    /// <returns></returns>
    Task AddTag(Tag tag);

    /// <summary>
    /// Изменить тег
    /// </summary>
    /// <returns></returns>
    Task UpdateTag(Tag tag);

    /// <summary>
    /// Удалить тег
    /// </summary>
    /// <returns></returns>
    Task DeleteTag(Tag tag);
}