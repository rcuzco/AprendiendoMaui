using Kotlin.Contracts;
using MonkeyFinder.Services;
using System.Reflection;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    public ObservableCollection<Monkey> Monkeys { get; } = new();

    private readonly IMonkeyService _monkeyService;
    private readonly IDisplayMessageService _displayMessageService;
    private readonly IConnectivity _connectivity;
    private readonly IGeolocation _geolocation;

    [ObservableProperty]
    bool isRefreshing;

    public MonkeysViewModel(IMonkeyService monkeyService, IDisplayMessageService displayMessageService, IConnectivity connectivity, IGeolocation geolocation)
    {
        Title = "The list of monkeys!!";
        _monkeyService = monkeyService;
        _displayMessageService = displayMessageService;
        _connectivity = connectivity;
        _geolocation = geolocation;
    }

    [RelayCommand]
    async Task GetClosestMonekyAsync()
    {
        if (IsBusy || !Monkeys.Any())
        {
            return;
        }

        try
        {
            var location = await _geolocation.GetLastKnownLocationAsync();
            if (location is null)
            {
                location = await _geolocation.GetLocationAsync(
                    new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
            }

            if (location is null) return;

            var first = Monkeys.OrderBy(m => location.CalculateDistance(m.Latitude, m.Longitude, DistanceUnits.Kilometers)).FirstOrDefault();

            if (first is null) return;

            await _displayMessageService.DisplayAlertAsync("Closest monkey", $"{first.Name} in {first.Location}", "Ok");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await _displayMessageService.DisplayAlertAsync("Error!", $"Unable to get closest moneky: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    async Task GoToDetailsAsync(Monkey monkey)
    {
        if (monkey is null) return;

        //esto de abajo tb debería estar en una interface q se pueda inyectar y usar desde cualquier sitio
        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}",animate:true, parameters:
            new Dictionary<string, object>
            {
                { "Monkey", monkey }
            });
    }

    [RelayCommand]
    async Task GetMonkeysAsync()
    {
        if (IsBusy) return;
        try
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await _displayMessageService.DisplayAlertAsync("Internet connectivity problem", "Check your internet connection", "OK");
            }

            IsBusy = true;
            var monkeys = await _monkeyService.GetMonkeysAsync();

            if (Monkeys.Any())
            {
                Monkeys.Clear();
            }

            foreach (var monkey in monkeys)
            {
                Monkeys.Add(monkey);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await _displayMessageService.DisplayAlertAsync("Error!", $"Unable to get data: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }
}
