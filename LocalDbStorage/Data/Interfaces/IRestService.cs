using LocalDbStorage.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalDbStorage.Data.Interfaces
{
    public interface IRestService
    {
        Task<Branch[]> GetAllBranch();
        Task<WorkStation[]> GetAllWorkStation();
        Task<ActiveAlarm[]> GetAllActiveAlarm();
        Task<IncomingAlarm[]> GetAllIncomingAlarm();
        Task<SuppressedAlarm[]> GetAllSuppressedAlarm();
        Task<Tag[]> GetAllTag();
        Task AddTag(Tag tag);
        Task UpdateTag(Tag tag, int id);
        Task DeleteTag(int id);
    }
}