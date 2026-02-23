using CommunityToolkit.Maui.Alerts;
using Color = Microsoft.Maui.Graphics.Color;

namespace Dome.Shared.Helper
{
    public static class AlertHelper
    {
        public static async Task ShowSnackBar(string messsage)
        {
            try
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                SnackbarOptions snackbarOptions = new SnackbarOptions
                {
                    BackgroundColor = Colors.LightGray,
                    TextColor = Colors.Black,
                    ActionButtonTextColor = Color.FromArgb("#EF2F50"),
                    CornerRadius = 10,
                    CharacterSpacing = .5,
                };

                string text = messsage;
                string actionButtonText = "Dismiss";
                TimeSpan duration = TimeSpan.FromSeconds(5);
                ISnackbar snackBar = Snackbar.Make(
                    text,
                    null,
                    actionButtonText,
                    duration,
                    snackbarOptions
                );
                await snackBar.Show(cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
