# Dome AI coding guide

## Big picture
- Solution is 3 .NET MAUI apps (Player, Vendor, Admin) plus shared library Dome.Shared; each app is a separate entrypoint and UI shell, sharing models/services via Dome.Shared. See [README.md](README.md) and [Dome.Shared](Dome.Shared).
- UI follows MVVM with CommunityToolkit.Mvvm; Views live under each app’s Views/ and ViewModels/ folders, and DI wiring happens in each app’s `MauiProgram.ConfigurePages/ConfigureServices`. See [Player/MauiProgram.cs](Player/MauiProgram.cs) and [Admin/MauiProgram.cs](Admin/MauiProgram.cs).
- Navigation uses MAUI Shell + `INavigationService` wrapper (`MauiNavigationService`) in the shared project. See [Dome.Shared/Services/Implementations/MauiNavigationService.cs](Dome.Shared/Services/Implementations/MauiNavigationService.cs).

## Key integrations & data flows
- Realm/MongoDB App Services is the data store and auth provider; access via `IRealmService` from ViewModels. See [Admin/ViewModels/BaseViewModel.cs](Admin/ViewModels/BaseViewModel.cs) and [Player/ViewModels](Player/ViewModels).
- Payments use Stripe: `StripeService` hits the server API and Player `PaymentView` is registered as Scoped (per Stripe.Maui guidance). See [Player/Services/Implementations/StripeService.cs](Player/Services/Implementations/StripeService.cs) and [Player/MauiProgram.cs](Player/MauiProgram.cs).
- Messaging uses a separate chat backend in `ChatService` with its own base URL. See [Player/Services/Implementations/ChatService.cs](Player/Services/Implementations/ChatService.cs).
- Secrets are centralized via `ISecretsService` and used by Twilio/SendGrid/AWS/Realm. See [Dome.Shared/Services/Interfaces/ISecretsService.cs](Dome.Shared/Services/Interfaces/ISecretsService.cs).

## Project-specific conventions
- Shell navigation only; don’t mix with NavigationPage/TabbedPage/FlyoutPage. This is reinforced in the MAUI agent rules at [./.github/agents/dotnet-maui.agent.md](.github/agents/dotnet-maui.agent.md).
- Avoid obsolete MAUI controls/patterns: no ListView/TableView/AndExpand, use `Background` instead of `BackgroundColor`, use handlers (not renderers). See [./.github/agents/dotnet-maui.agent.md](.github/agents/dotnet-maui.agent.md).
- Most views are built in C# (CommunityToolkit.Maui.Markup); keep that style when editing existing views. Example: [Player/Views/NoInternetView.cs](Player/Views/NoInternetView.cs).
- Environment is controlled via `AppConstants.Environment` (currently production) and affects Stripe key selection. See [Dome.Shared/Constants/AppConstants.cs](Dome.Shared/Constants/AppConstants.cs) and [Player/MauiProgram.cs](Player/MauiProgram.cs).

## Core user flows (Player app)
- Booking flow: Home Play tab → Venue list → Venue details → Booking timing → Payment → Success. See [Player/ViewModels/Book](Player/ViewModels/Book) and [Player/Views/Book](Player/Views/Book).
- Connect flow: connect views and chat (join requests, conversations). See [Player/ViewModels/Connect](Player/ViewModels/Connect) and [Player/Views/Connect](Player/Views/Connect).
- Login/registration flow: account onboarding and verification views. See [Player/ViewModels/Account](Player/ViewModels/Account) and [Player/Views/Account](Player/Views/Account).

## Build & run workflow
- Restore/build the solution: `dotnet restore Dome.sln`, `dotnet build Dome.sln`. See [README.md](README.md).
- Build per app/platform using the app .csproj and target framework (e.g., `net9.0-android`, `net9.0-ios`, `net9.0-windows10.0.19041.0`). See [README.md](README.md).
- Update secrets and endpoints in [Dome.Shared/appsettings.json](Dome.Shared/appsettings.json) before running locally.

## Where to look first
- Player app entry + DI: [Player/MauiProgram.cs](Player/MauiProgram.cs)
- Admin app entry + DI: [Admin/MauiProgram.cs](Admin/MauiProgram.cs)
- Shared constants/config: [Dome.Shared/Constants/AppConstants.cs](Dome.Shared/Constants/AppConstants.cs)
- Shared auth services: [Dome.Shared/Services/Implementations/Authentication](Dome.Shared/Services/Implementations/Authentication)
- Shared service interfaces: [Dome.Shared/Services/Interfaces](Dome.Shared/Services/Interfaces)
