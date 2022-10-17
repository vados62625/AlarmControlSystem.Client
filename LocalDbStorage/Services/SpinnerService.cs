namespace LocalDbStorage.Services
{

    public interface ISpinnerService
    {
        public event Action OnShow;
        public event Action OnHide;
        public void Show();
        public void Hide();
    }
    public class SpinnerService: ISpinnerService
    {
        public event Action OnShow = default!;
        public event Action OnHide = default!;

        public void Show()
        {
            OnShow?.Invoke();
        }

        public void Hide()
        {
            OnHide?.Invoke();
        }
	}
}
