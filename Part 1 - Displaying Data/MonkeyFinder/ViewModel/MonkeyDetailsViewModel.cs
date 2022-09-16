using MonkeyFinder.Services;
using System.Data.SqlTypes;

namespace MonkeyFinder.ViewModel;

[QueryProperty("Monkey", "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    private readonly IMap _map;
	private readonly IDisplayMessageService _displayMessageService;

	public MonkeyDetailsViewModel(IMap map, IDisplayMessageService displayMessageService)
	{
		_map = map;
		_displayMessageService = displayMessageService;
    }

	[ObservableProperty]
	Monkey monkey;

	[RelayCommand]
	async Task OpenMapAsync()
	{
		try
		{
			await _map.OpenAsync(Monkey.Latitude, Monkey.Longitude, new MapLaunchOptions
			{
				Name = Monkey.Name,
				NavigationMode = NavigationMode.None
			});
		}
		catch (Exception ex)
		{
            Debug.WriteLine(ex);
            await _displayMessageService.DisplayAlertAsync("Error!", $"Unable to open map: {ex.Message}", "OK");
        }
	}
	
}
