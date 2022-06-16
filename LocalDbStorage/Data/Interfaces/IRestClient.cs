using System.Threading.Tasks;

namespace GPNA.AlarmControlSystem.LocalDbStorage.Data.Interfaces
{
    public interface IRestClient
    {
        Task<T[]> GetAll<T>(string uri);
        Task<T?> GetOne<T>(string uri);
        Task Post<T>(string uri, T identificator);
    }
}