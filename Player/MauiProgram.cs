using Dome.Player.Services.Implementations;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Mopups.Hosting;
using Player.Views.OnBoarding;
using Stripe;
using Syncfusion.Maui.Toolkit.Hosting;

namespace Player;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSharpnadoTabs(loggerEnable: false)
            .ConfigureSyncfusionToolkit()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMarkup()
            .ConfigurePages()
            .ConfigureServices()
            .ConfigureHandlers()
            .ConfigureMopups()
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
                fonts.AddFont("Inter-ExtraBold.ttf", "InterExtraBold");
                // didn't find the italic font online
                // fonts.AddFont("Inter-Italic.ttf", "InterItalic");
            })
            .ConfigureLifecycleEvents(lifecycle =>
            {
#if IOS || MACCATALYST
                lifecycle.AddiOS(ios =>
                {
                    ios.FinishedLaunching((app, data) => HandleAppLink(app.UserActivity));

                    ios.ContinueUserActivity(
                        (app, userActivity, handler) => HandleAppLink(userActivity)
                    );

                    if (
                        OperatingSystem.IsIOSVersionAtLeast(13)
                        || OperatingSystem.IsMacCatalystVersionAtLeast(13)
                    )
                    {
                        ios.SceneWillConnect(
                            (scene, sceneSession, sceneConnectionOptions) =>
                                HandleAppLink(
                                    sceneConnectionOptions
                                        .UserActivities.ToArray()
                                        .FirstOrDefault(a =>
                                            a.ActivityType
                                            == Foundation.NSUserActivityType.BrowsingWeb
                                        )
                                )
                        );

                        ios.SceneContinueUserActivity(
                            (scene, userActivity) => HandleAppLink(userActivity)
                        );
                    }
                });
#elif ANDROID
                lifecycle.AddAndroid(android =>
                {
                    android.OnCreate(
                        (activity, bundle) =>
                        {
                            var action = activity.Intent?.Action;
                            var data = activity.Intent?.Data?.ToString();

                            if (action == Android.Content.Intent.ActionView && data is not null)
                            {
                                activity.Finish();
                                System.Threading.Tasks.Task.Run(() => HandleAppLink(data));
                            }
                        }
                    );
                });
#endif
            });

        builder.Services.AddSingleton<IPopupNavigation>(MopupService.Instance);

        string environment = AppConstants.Environment;
        if (environment == "development")
        {
            builder.UseStripe(
                "pk_test_51PfQOVRrYwdBl6oLko5EXiROUUl9pyN6BMsgOrBIQlrERFjCosZRiGe8UbTftsS9O4COOh9bD4mZqEwD0ybyuSA2003oMNlOXK"
            );
        }
        else if (environment == "production")
        {
            builder.UseStripe(
                "pk_live_51PfQOVRrYwdBl6oLybUu7WeyeINQNaweDRMPKcOOguXjXVGog35oXYW7TGejhZpDZ3gaXx02hVGrEh19cdF68iUn00VHjORoRG"

            );
        }

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

#if IOS || MACCATALYST
    static bool HandleAppLink(Foundation.NSUserActivity? userActivity)
    {
        if (
            userActivity is not null
            && userActivity.ActivityType == Foundation.NSUserActivityType.BrowsingWeb
            && userActivity.WebPageUrl is not null
        )
        {
            HandleAppLink(userActivity.WebPageUrl.ToString());
            return true;
        }
        return false;
    }
#endif

    static void HandleAppLink(string url)
    {
        if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri))
        {
            App.Current?.SendOnAppLinkRequestReceived(uri);
        }
    }

    public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
    {
        //Account Views
        builder.Services.AddTransient<LoginView, LoginViewModel>();
        builder.Services.AddTransient<NewProfileView, NewProfileViewModel>();
        builder.Services.AddTransient<NewAgeAndInterestView, NewAgeAndInterestViewModel>();
        builder.Services.AddSingleton<AccountView, AccountViewModel>();
        builder.Services.AddTransient<RegisterView, RegisterViewModel>();
        builder.Services.AddTransient<ForgotPasswordView, ForgotPasswordViewModel>();
        builder.Services.AddTransient<ChangePasswordView, ChangePasswordViewModel>();
        builder.Services.AddTransient<
            AuthenticateEmailForLoginView,
            AuthenticateEmailForLoginViewModel
        >();
        builder.Services.AddTransient<EditProfileView, EditProfileViewModel>();
        builder.Services.AddTransient<EditInterestsView, EditInterestsViewModel>();
        builder.Services.AddTransient<
            VerifyRegisteredMobileNumberView,
            VerifyRegisteredMobileNumberViewModel
        >();
        builder.Services.AddTransient<
            VerifyEmailForPasswordResetView,
            VerifyEmailForPasswordResetViewModel
        >();
        builder.Services.AddTransient<
            ConfirmEmailToRegisterItView,
            ConfirmEmailToRegisterItViewModel
        >();
        builder.Services.AddTransient<ChangeMobileNumberView, ChangeMobileNumberViewModel>();
        builder.Services.AddTransient<LoadDataView, LoadDataViewModel>();

        //connect views
        builder.Services.AddTransient<ConnectView, ConnectViewModel>();
        builder.Services.AddTransient<PlanBookingView, PlanBookingViewModel>();
        builder.Services.AddTransient<JoinAGameView, JoinAGameViewModel>();
        builder.Services.AddTransient<JoinRequestsView, JoinRequestsViewModel>();
        builder.Services.AddTransient<
            ReviewGameBeforeApplyingView,
            ReviewGameBeforeApplyingViewModel
        >();
        builder.Services.AddTransient<ChatView, ChatViewModel>();
        builder.Services.AddTransient<PlannedBookingsListView, PlannedBookingsListViewModel>();
        builder.Services.AddTransient<HostingSuccessView, HostingSuccessViewModel>();
        builder.Services.AddTransient<JoiningSuccessView>();

        //book views
        builder.Services.AddTransient<VenuesListView, VenuesListViewModel>();
        builder.Services.AddTransient<AvailableGamesView, AvailableGamesViewModel>();
        builder.Services.AddTransient<BookingTimingView, BookingTimingViewModel>();
        builder.Services.AddTransient<VenueDetailsView, VenueDetailsViewModel>();
        // as per the docs of Stripe.Maui the View  where payment sheet is opened should be registered as Scoped
        builder.Services.AddScoped<PaymentView, PaymentViewModel>();
        builder.Services.AddTransient<PaymentSuccessView, PaymentSuccessViewModel>();
        builder.Services.AddTransient<TeamView, TeamViewModel>();

        //Other views
        builder.Services.AddTransient<DashboardView, DashboardViewModel>();
        builder.Services.AddTransient<LearnView, LearnViewModel>();
        builder.Services.AddTransient<MyBookingsView, MyBookingsViewModel>();
        builder.Services.AddTransient<NoInternetView, NoInternetViewModel>();
        builder.Services.AddTransient<CoachView, CoachViewModel>();
        builder.Services.AddTransient<BaseView, BaseViewModel>();
        builder.Services.AddTransient<AccountBaseView, AccountBaseViewModel>();
        builder.Services.AddTransient<FeedbackEmailView, FeedbackEmailViewModel>();
        builder.Services.AddTransient<OnBoardPage1>();
        builder.Services.AddTransient<OnBoardPage2>();
        builder.Services.AddTransient<OnBoardPage3>();
        builder.Services.AddTransient<NotificationView, NotificationViewModel>();

        return builder;
    }

    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {
        //App Services
        builder.Services.AddSingleton<INavigationService, MauiNavigationService>();
        builder.Services.AddTransient<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<IRealmService, RealmService>();
        builder.Services.AddTransient<IAppStaticData, AppStaticData>();
        builder.Services.AddTransient<ITwilioService, TwilioService>();
        builder.Services.AddTransient<IImageService, ImageService>();
        builder.Services.AddTransient<ILocationService, LocationService>();
        builder.Services.AddTransient<IFeedbackEmailService, FeedbackEmailService>();
        builder.Services.AddSingleton<ISecretsService, SecretsService>();
        builder.Services.AddTransient<IChatService, ChatService>();

        // Other Services
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IMediaPicker>(MediaPicker.Default);
        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
        builder.Services.AddSingleton<IGeocoding>(Geocoding.Default);
        builder.Services.AddSingleton<IBrowser>(Browser.Default);
        builder.Services.AddSingleton<ILauncher>(Launcher.Default);
        builder.Services.AddSingleton<IVersionTracking>(VersionTracking.Default);
        builder.Services.AddSingleton<IDeviceInfo>(DeviceInfo.Current);
        builder.Services.AddSingleton<IFileSystem>(FileSystem.Current);

        //Stripe
        builder.Services.AddTransient<IStripeService, StripeService>();
        builder.Services.AddTransient<IAddToCartService, AddToCartService>();

        return builder;
    }

    public static MauiAppBuilder ConfigureHandlers(this MauiAppBuilder builder)
    {
        return builder.ConfigureMauiHandlers(handlers =>
        {
#if ANDROID

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(
                "NoUnderline",
                (handler, view) =>
                {
                    handler.PlatformView.BackgroundTintList =
                        Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
                    handler.PlatformView.TextCursorDrawable.SetTint(Colors.Red.ToPlatform());
                }
            );

            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping(
                "NoUnderline",
                (handler, view) =>
                {
                    handler.PlatformView.BackgroundTintList =
                        Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
                    handler.PlatformView.TextCursorDrawable.SetTint(Colors.Red.ToPlatform());
                }
            );

            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(
                "NoUnderline",
                (handler, view) =>
                {
                    handler.PlatformView.BackgroundTintList =
                        Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
                }
            );

            Microsoft.Maui.Handlers.ButtonHandler.Mapper.AppendToMapping(
                "NoOrangeBorder",
                (handler, view) =>
                {
                    handler.PlatformView.StateListAnimator = null;
                    handler.PlatformView.Elevation = 0;
                    handler.PlatformView.Foreground = null;
                }
            );

            Microsoft.Maui.Handlers.ImageButtonHandler.Mapper.AppendToMapping(
                "NoOrangeBorder",
                (handler, view) =>
                {
                    handler.PlatformView.StateListAnimator = null;
                    handler.PlatformView.Elevation = 0;
                    handler.PlatformView.Foreground = null;
                    handler.PlatformView.SetPadding(0, 0, 0, 0);
                }
            );

#endif

#if IOS
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(
                "NoBorder",
                (handler, view) =>
                {
                    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
                }
            );

            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(
                "NoBorder",
                (handler, view) =>
                {
                    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
                }
            );

#endif
        });
    }
}
