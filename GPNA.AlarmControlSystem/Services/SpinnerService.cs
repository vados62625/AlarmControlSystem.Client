namespace GPNA.AlarmControlSystem.Services
{
    public interface ISpinnerService
    {
        public event Action OnShow;
        public event Action OnHide;
        public void Show();
        public void Hide();
        public Task<TResult> Load<TResult>(Func<Task<TResult>> func);
        public Task Load(Func<Task> func);
    }

    public class SpinnerService : ISpinnerService
    {
        public event Action OnShow = default!;
        public event Action OnHide = default!;

        public void Show() => OnShow?.Invoke();

        public void Hide() => OnHide?.Invoke();

        public async Task<TResult> Load<TResult>(Func<Task<TResult>> func)
        {
            try
            {
                Show();
                return await func();
            }
            finally
            {
                Hide();
            }
        }

        public async Task Load(Func<Task> func)
        {
            try
            {
                Show();
                await func();
            }
            finally 
            { 
                Hide(); 
            }
        }
    }
}
