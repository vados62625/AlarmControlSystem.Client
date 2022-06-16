using GPNA.AlarmControlSystem.Data.AlarmControlSystem;

namespace GPNA.AlarmControlSystem.Data
{
    public interface IUiRestService
    {
        List<Tag> GetAllTag();

        Task GetBranch();
    }
}