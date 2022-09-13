using System.Net.Http;
using System.Net.Http.Json;

namespace MonkeyFinder.Services;

public class MonkeyService : IMonkeyService
{
    private readonly HttpClient _httpClient;    
    private IEnumerable<Monkey> _monkeyList = Enumerable.Empty<Monkey>();
    public MonkeyService(IHttpClientFactory httpClientFactory)
    {       
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<IEnumerable<Monkey>> GetMonkeysAsync()
    {
        if (_monkeyList?.Any() == true)
            return _monkeyList;

        var url = "https://montemagno.com/monkeys.json";
        var response = await _httpClient.GetAsync(url);
        
        if (response.IsSuccessStatusCode)
        {
            _monkeyList = await response.Content.ReadFromJsonAsync<IEnumerable<Monkey>>();
        }

        return _monkeyList;
    }
}
