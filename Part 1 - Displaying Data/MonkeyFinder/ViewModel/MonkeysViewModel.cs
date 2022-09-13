using MonkeyFinder.Services;
using System.Reflection;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    public ObservableCollection<Monkey> Monkeys { get; } = new();

    private readonly IMonkeyService _monkeyService;
    private readonly IDisplayMessageService _displayMessageService;

    public MonkeysViewModel(IMonkeyService monkeyService, IDisplayMessageService displayMessageService)
    {
        Title = "The list of monkeys!!";
        _monkeyService = monkeyService;
        _displayMessageService = displayMessageService;
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
            await _displayMessageService.DisplayAlert("Error!", $"Unable to get data: {ex.Message}", "OK"); //esto deberíamos llevarlo a una interfaz y cuando necesitemos hacer cosas, usamos la interfaz
        }
        finally
        {
            IsBusy = false;
        }
    }
}
