using GPNA.ACSAPI.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GPNA.AlarmControlSystem.LocalDbStorage.Data.Interfaces
{
    public interface IRestService
    {
        Task<Branch[]> GetAllBranch();
    }
}