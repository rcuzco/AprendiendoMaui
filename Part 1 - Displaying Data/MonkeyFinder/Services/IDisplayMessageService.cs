namespace MonkeyFinder.Services
{
    public interface IDisplayMessageService
    {
        Task DisplayAlert(string title, string message, string buttonText);
    }
}