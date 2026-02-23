using Dome.Admin.Services.Implementations;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using Mopups.Services;

namespace Admin;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkitMarkup()
            .ConfigurePages()
            .ConfigureServices()
            .ConfigureMopups()
            .UseMauiCommunityToolkit(options =>
            {
                options.SetShouldEnableSnackbarOnWindows(true);
            })
            .ConfigureHandlers()
            .ConfigureEssentials(essentials =>
            {
                essentials.UseVersionTracking();
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Inter-Light.ttf", "InterLight");
                fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                fonts.AddFont("Inter-Medium.ttf", "InterMedium");
                fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
                fonts.AddFont("Inter-Bold.ttf", "InterBold");
            });

        builder.Services.AddSingleton<IPopupNavigation>(MopupService.Instance);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
    {
        //Account Views
        builder.Services.AddTransient<LoginView, LoginViewModel>();
        builder.Services.AddTransient<OnboardVendorView, OnboardVendorViewModel>();
        builder.Services.AddTransient<EditVendorView, EditVendorViewModel>();
        builder.Services.AddTransient<VendorYardView, VendorYardViewModel>();
        builder.Services.AddTransient<ChangePasswordView, ChangePasswordViewModel>();
        builder.Services.AddTransient<ForgotPasswordView, ForgotPasswordViewModel>();
        builder.Services.AddTransient<
            VerifyEmailForPasswordResetView,
            VerifyEmailForPasswordResetViewModel
        >();
        builder.Services.AddTransient<
            AuthenticateEmailForLoginView,
            AuthenticateEmailForLoginViewModel
        >();
        builder.Services.AddTransient<CoachView, CoachViewModel>();
        builder.Services.AddTransient<LearnView, LearnViewModel>();
        builder.Services.AddTransient<CouponView, CouponViewModel>();
        builder.Services.AddTransient<SyncDataView, SyncDataViewModel>();

        builder.Services.AddTransient<ChangeVendorPasswordViewModel>();
        return builder;
    }

    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {
        //App Services
        builder.Services.AddSingleton<INavigationService, MauiNavigationService>();
        builder.Services.AddSingleton<IRealmService, RealmService>();
        builder.Services.AddTransient<ISettingsService, SettingsService>();
        builder.Services.AddTransient<ITwilioService, TwilioService>();
        builder.Services.AddTransient<IImageService, ImageService>();
        builder.Services.AddSingleton<ISecretsService, SecretsService>();

        // Other Services
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IMediaPicker>(MediaPicker.Default);
        builder.Services.AddSingleton<IFileSystem>(FileSystem.Current);

        return builder;
    }

    public static MauiAppBuilder ConfigureHandlers(this MauiAppBuilder builder)
    {
        return builder.ConfigureMauiHandlers(handlers =>
        {
#if WINDOWS

            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping(
                "NoUnderline",
                (handler, view) =>
                {
                    handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                }
            );

            Microsoft.Maui.Controls.Handlers.Items.CollectionViewHandler.Mapper.AppendToMapping(
                "NoUnderline",
                (handler, view) =>
                {
                    handler.PlatformView.IsMultiSelectCheckBoxEnabled = false;
                }
            );

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(
                "NoUnderline",
                (handler, view) =>
                {
                    handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                }
            );
#endif
#if MACCATALYST
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(
                "NoUnderline",
                (handler, view) =>
                {
                    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
                }
            );
#endif
        });
    }
}
