using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using Player.Views.OnBoarding;

namespace Player
{
    public class AppShell : Shell
    {
        public AppShell(string status)
        {
            FlyoutBehavior = FlyoutBehavior.Disabled;
            SetNavBarIsVisible(this, false);
            SetTabBarForegroundColor(this, Color.FromArgb("#EF2F50"));
            SetTabBarTitleColor(this, Color.FromArgb("#EF2F50"));

            TabBar loginTab = new TabBar
            {
                Items =
                {
                    new ShellContent
                    {
                        Route = nameof(LoginView),
                        ContentTemplate = new DataTemplate(typeof(LoginView)),
                    },
                },
            };

            TabBar syncDataTab = new TabBar
            {
                Items =
                {
                    new ShellContent
                    {
                        Route = nameof(LoadDataView),
                        ContentTemplate = new DataTemplate(typeof(LoadDataView)),
                    },
                },
            };

            TabBar emailAuthenticationTab = new TabBar
            {
                Items =
                {
                    new ShellContent
                    {
                        Route = nameof(AuthenticateEmailForLoginView),
                        ContentTemplate = new DataTemplate(typeof(AuthenticateEmailForLoginView)),
                    },
                },
            };

            TabBar LoggedInBar = new TabBar
            {
                Route = "LoggedInBar",
                Items =
                {
                    new ShellContent
                    {
                        Title = "Home",
                        Icon = "home",
                        Route = nameof(DashboardView),
                        ContentTemplate = new DataTemplate(typeof(DashboardView)),
                    },
                    new ShellContent
                    {
                        Title = "Play",
                        Icon = "play",
                        Route = nameof(AvailableGamesView),
                        ContentTemplate = new DataTemplate(typeof(AvailableGamesView)),
                    },
                    new ShellContent
                    {
                        Title = "Connect",
                        Icon = "connect",
                        Route = nameof(ConnectView),
                        ContentTemplate = new DataTemplate(typeof(ConnectView)),
                    },
                    new ShellContent
                    {
                        Title = "Bookings",
                        Icon = "my_booking",
                        Route = nameof(MyBookingsView),
                        ContentTemplate = new DataTemplate(typeof(MyBookingsView)),
                    },
                    new ShellContent
                    {
                        Title = "Settings",
                        Icon = "settings",
                        Route = nameof(AccountView),
                        ContentTemplate = new DataTemplate(typeof(AccountView)),
                    },
                },
            };

            switch (status)
            {
                case "Login":
                    Items.Add(loginTab);
                    Items.Add(emailAuthenticationTab);
                    Items.Add(syncDataTab);
                    Items.Add(LoggedInBar);

                    break;
                case "LoggedIn":
                    Items.Add(syncDataTab);
                    Items.Add(LoggedInBar);
                    Items.Add(loginTab);
                    Items.Add(emailAuthenticationTab);
                    break;
                case "EmailAuthentication":
                    Items.Add(emailAuthenticationTab);
                    Items.Add(syncDataTab);
                    Items.Add(LoggedInBar);
                    Items.Add(loginTab);
                    break;
            }

            // account
            Routing.RegisterRoute(nameof(RegisterView), typeof(RegisterView));
            Routing.RegisterRoute(nameof(ForgotPasswordView), typeof(ForgotPasswordView));
            Routing.RegisterRoute(nameof(ChangePasswordView), typeof(ChangePasswordView));
            Routing.RegisterRoute(nameof(EditProfileView), typeof(EditProfileView));
            Routing.RegisterRoute(nameof(EditInterestsView), typeof(EditInterestsView));
            Routing.RegisterRoute(nameof(NewProfileView), typeof(NewProfileView));
            Routing.RegisterRoute(nameof(NewAgeAndInterestView), typeof(NewAgeAndInterestView));
            Routing.RegisterRoute(
                nameof(VerifyEmailForPasswordResetView),
                typeof(VerifyEmailForPasswordResetView)
            );
            Routing.RegisterRoute(
                nameof(ConfirmEmailToRegisterItView),
                typeof(ConfirmEmailToRegisterItView)
            );
            Routing.RegisterRoute(nameof(ChangeMobileNumberView), typeof(ChangeMobileNumberView));
            Routing.RegisterRoute(
                nameof(VerifyRegisteredMobileNumberView),
                typeof(VerifyRegisteredMobileNumberView)
            );
            Routing.RegisterRoute(nameof(NewProfileView), typeof(NewProfileView));

            //onboarding
            Routing.RegisterRoute(nameof(OnBoardPage1), typeof(OnBoardPage1));
            Routing.RegisterRoute(nameof(OnBoardPage2), typeof(OnBoardPage2));
            Routing.RegisterRoute(nameof(OnBoardPage3), typeof(OnBoardPage3));

            //book
            Routing.RegisterRoute(nameof(VenuesListView), typeof(VenuesListView));
            Routing.RegisterRoute(nameof(BookingTimingView), typeof(BookingTimingView));
            Routing.RegisterRoute(nameof(VenueDetailsView), typeof(VenueDetailsView));
            Routing.RegisterRoute(nameof(PaymentSuccessView), typeof(PaymentSuccessView));
            Routing.RegisterRoute(nameof(PaymentView), typeof(PaymentView));
            Routing.RegisterRoute(nameof(TeamView), typeof(TeamView));

            //connect
            Routing.RegisterRoute(
                nameof(ReviewPlannedBookingPopup),
                typeof(ReviewPlannedBookingPopup)
            );
            Routing.RegisterRoute(nameof(JoinAGameView), typeof(JoinAGameView));
            Routing.RegisterRoute(
                nameof(ReviewGameBeforeApplyingView),
                typeof(ReviewGameBeforeApplyingView)
            );
            Routing.RegisterRoute(nameof(JoinRequestsView), typeof(JoinRequestsView));
            Routing.RegisterRoute(nameof(ChatView), typeof(ChatView));
            Routing.RegisterRoute(nameof(PlannedBookingsListView), typeof(PlannedBookingsListView));
            Routing.RegisterRoute(nameof(HostingSuccessView), typeof(HostingSuccessView));
            Routing.RegisterRoute(nameof(PlanBookingView), typeof(PlanBookingView));
            Routing.RegisterRoute(
                nameof(ReviewGameBeforeJoiningPopup),
                typeof(ReviewGameBeforeJoiningPopup)
            );
            Routing.RegisterRoute(nameof(JoiningSuccessView), typeof(JoiningSuccessView));

            //others
            Routing.RegisterRoute(nameof(NoInternetView), typeof(NoInternetView));
            Routing.RegisterRoute(nameof(LearnView), typeof(LearnView));
            Routing.RegisterRoute(nameof(CoachView), typeof(CoachView));
            Routing.RegisterRoute(nameof(FeedbackEmailView), typeof(FeedbackEmailView));
            Routing.RegisterRoute(nameof(NotificationView), typeof(NotificationView));

            CheckConnectivity();
#if ANDROID
            // set status bar theme on startup.
            SetStatusBarColor();
#endif

            // set status bar theme on theme change.
            App.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
        }

        private void Current_RequestedThemeChanged(object? sender, AppThemeChangedEventArgs e) =>
            SetStatusBarColor();

        /// <summary>
        /// On theme change this method is called to set the status bar color and Tab Bar Unselected Color according to the theme.
        /// </summary>
        private void SetStatusBarColor()
        {
            if (App.Current.RequestedTheme == AppTheme.Dark)
            {
                //this.Behaviors.Add(new StatusBarBehavior
                //{
                //    StatusBarColor = Color.FromArgb("#23262A"),
                //    StatusBarStyle = StatusBarStyle.LightContent
                //});

                CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb("#23262A"));
                CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(StatusBarStyle.LightContent);
                Shell.SetTabBarUnselectedColor(this, Colors.White);
            }
            else
            {
                //this.Behaviors.Add(new StatusBarBehavior
                //{
                //    StatusBarColor = Colors.White,
                //    StatusBarStyle = StatusBarStyle.DarkContent
                //});
                CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Colors.White);
                CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(StatusBarStyle.DarkContent);
                Shell.SetTabBarUnselectedColor(this, Colors.Black);
            }
        }

        /// <summary>
        /// this method clear all the pages from the navigation state when the user navigates to a new shell section (Tab). Thus when the user navigates to a any tab the user will always see the first page of the tab as the navigation stack is cleared.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            try
            {
                base.OnNavigated(args);
                if (args.Source == ShellNavigationSource.ShellSectionChanged)
                {
                    List<Page> navigationStack = Navigation.NavigationStack.ToList();

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

        // A method to ping a webserver after every second to check internet connectivity. If the user is not connected navigate the user to the NoInternetView and change the connectivityStatus to false. When interenet connectivity returns change the connectivityStatus to true and navigate the user to the last page he was on.
        private async void CheckConnectivity()
        {
            try
            {
                this.Dispatcher.StartTimer(
                    TimeSpan.FromSeconds(10),
                    () =>
                    {
                        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                        {
                            if (
                                Shell.Current.CurrentPage != null
                                && Shell.Current.CurrentPage.GetType() != typeof(NoInternetView)
                            )
                            {
                                Shell.Current.Navigation.PushAsync(new NoInternetView());
                            }
                        }
                        return true;
                    }
                );
            }
            catch (Exception ex)
            {
                //  Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            string? currentPageRoute = Shell.Current.CurrentState.Location.ToString();

            if (currentPageRoute.Contains(nameof(PaymentView))
                || currentPageRoute.Contains(nameof(PaymentSuccessView))
                || currentPageRoute.Contains(nameof(BookingTimingView)))
            {
                return true;
            }

            Dispatcher.Dispatch(async () =>
            {
                try
                {
                    if (
                        currentPageRoute == $"//LoggedInBar/{nameof(DashboardView)}"
                        || currentPageRoute == $"//LoggedInBar/{nameof(AvailableGamesView)}"
                        || currentPageRoute == $"//LoggedInBar/{nameof(ConnectView)}"
                        || currentPageRoute == $"//LoggedInBar/{nameof(MyBookingsView)}"
                        || currentPageRoute == $"//LoggedInBar/{nameof(AccountView)}"
                    )
                    {
                        await Shell.Current.GoToAsync("///LoggedInBar/DashboardView");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("..");
                    }
                }
                catch (Exception) { }
            });

            return true;
        }
    }
}
