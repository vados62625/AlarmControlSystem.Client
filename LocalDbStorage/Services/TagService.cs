using LocalDbStorage.Extentions;
using LocalDbStorage.Interfaces;
using LocalDbStorage.Repositories.Models;
using Microsoft.Extensions.Logging;

namespace LocalDbStorage.Services;

public class TagService : ITagService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TagService> _log;

    public TagService(IHttpClientFactory httpClientFactory, ILogger<TagService> log)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(TagService));
        _log = log;
    }

    /// <summary>
    /// Получить все Теги
    /// </summary>
    /// <returns></returns>
    public async Task<List<Tag>> GetAllTags()
    {
        var result = await _httpClient.Get<List<Tag>>("/api/Tag?itemsOnPage=0&page=1");
        return result;
    }

    /// <summary>
    /// Добавить тег
    /// </summary>
    /// <returns></returns>
    public async Task AddTag(Tag tag)
    {
        try
        {
            await _httpClient.Post<Tag>("/api/Tag", tag);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    /// <summary>
    /// Изменить тег
    /// </summary>
    /// <returns></returns>
    public async Task UpdateTag(Tag tag)
    {
        try
        {
            await _httpClient.Put<Tag>($"/api/Tag/{tag.Id}", tag);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    /// <summary>
    /// Удалить тег
    /// </summary>
    /// <returns></returns>
    public async Task DeleteTag(Tag tag)
    {
        try
        {
            await _httpClient.Delete<Tag>($"/api/Tag/{tag.Id}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}