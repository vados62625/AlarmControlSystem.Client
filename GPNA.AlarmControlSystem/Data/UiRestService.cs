using GPNA.AlarmControlSystem.Data.AlarmControlSystem;

namespace GPNA.AlarmControlSystem.Data
{
    public class UiRestService : IUiRestService
    {
        private readonly AlarmControlSystemContext _context;
        public UiRestService(AlarmControlSystemContext context)
        {
            _context = context;
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
    }
}
