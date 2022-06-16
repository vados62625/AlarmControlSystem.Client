using GPNA.AlarmControlSystem.Data.AlarmControlSystem;
using GPNA.AlarmControlSystem.LocalDbStorage.Data.Interfaces;
using GPNA.AlarmControlSystem.LocalDbStorage.Data.Requests;

namespace GPNA.AlarmControlSystem.Data
{
    public class UiRestService : IUiRestService
    {
        private readonly AlarmControlSystemContext _context;
        private readonly IRestService _restService;

        public UiRestService(AlarmControlSystemContext context, IRestService restService)
        {
            _context = context;

            _restService = restService;

        }

        public List<Tag> GetAllTag()
        {
            try
            {
                var value = _context.Tags.ToList();
                return value;
            }
            catch (Exception e)
            {

            }
            return new List<Tag>();
        }

        public async Task GetBranch()
        {
            try
            {

                var test = await _restService.GetAllBranch();
            }
            catch (Exception e)
            {

            }
        }
    }
}
