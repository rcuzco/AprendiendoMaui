namespace MonkeyFinder;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		//registramos rutas
		Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
	}
}