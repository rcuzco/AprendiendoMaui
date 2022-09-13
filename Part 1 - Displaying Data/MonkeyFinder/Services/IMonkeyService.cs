namespace MonkeyFinder.Services
{
    public interface IMonkeyService
    {
        Task<IEnumerable<Monkey>> GetMonkeysAsync();
    }
}