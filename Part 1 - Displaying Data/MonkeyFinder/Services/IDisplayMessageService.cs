namespace MonkeyFinder.Services
{
    public interface IDisplayMessageService
    {
        Task DisplayAlertAsync(string title, string message, string buttonText);
    }
}