namespace LocalDbStorage.Interfaces
{
    public interface IRestClient
    {
        Task<T[]> GetAll<T>(string uri);
        Task<T?> GetOne<T>(string uri);
        Task Post<T>(string uri, T identificator);
        Task Put<T>(string uri, T identificator);
        Task Delete(string uri);
    }
}