namespace Admin;

public partial class AppShell : Shell
{
    public AppShell(string? status = null, AppShellViewModel? viewModel = null)
    {
        try
        {
            InitializeComponent();
            BindingContext = viewModel;

            Routing.RegisterRoute(nameof(OnboardVendorView), typeof(OnboardVendorView));
            Routing.RegisterRoute(nameof(EditVendorView), typeof(EditVendorView));
            Routing.RegisterRoute(
                nameof(VerifyEmailForPasswordResetView),
                typeof(VerifyEmailForPasswordResetView)
            );
            Routing.RegisterRoute(nameof(ForgotPasswordView), typeof(ForgotPasswordView));
            Routing.RegisterRoute(
                nameof(AuthenticateEmailForLoginView),
                typeof(AuthenticateEmailForLoginView)
            );

            if (status == "Login")
            {
                CurrentItem = Login;
            }
            else
            {
                CurrentItem = SyncWithServer;
            }
            
        }
        catch (Exception ex)
        {
        //    App.MainPage.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    // This method is called when the ShellNavigating event is raised.
    protected override void OnNavigating(ShellNavigatingEventArgs args)
    {
        try
        {
            base.OnNavigating(args);

            if (args.Source == ShellNavigationSource.ShellItemChanged)
            {
                List<Page>? navigationStack = Navigation.NavigationStack?.ToList();

                if (navigationStack?.Count > 0)
                {
                    foreach (Page page in navigationStack)
                        if (page != null)
                        {
                            Navigation.RemovePage(page);
                        }
                }
            }
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
