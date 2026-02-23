# Dome

A multi-platform venue booking and management system built with .NET MAUI, featuring separate applications for players, vendors, and administrators.

## Overview

Dome is a comprehensive booking platform that connects sports venue operators with players. The solution consists of three mobile applications and a shared library, enabling seamless venue booking, management, and administration across Android, iOS, macOS, and Windows platforms.

## Project Structure

The solution contains four main projects:

### 1. **Player** (`Player.csproj`)
The customer-facing mobile application for players to discover and book venues.

**Features:**
- Venue browsing and booking
- Coach discovery and connection
- Booking management (view, modify, cancel)
- User account management
- Payment integration (Stripe)
- Notifications and feedback
- Learning resources

**Platforms:** Android, iOS

### 2. **Vendor** (`Vendor.csproj`)
The partner application for venue operators and business owners.

**Features:**
- Business dashboard and analytics
- Financial dashboard and reporting
- Booking management and attendance tracking
- Venue configuration and availability
- Data synchronization
- User account management

**Platforms:** Android, iOS, macOS, Windows

### 3. **Admin** (`Admin.csproj`)
The administrative application for platform management.

**Features:**
- System-wide dashboard and analytics
- Vendor onboarding and management
- Vendor password management
- Coupon and promotion management
- Coach management
- Data synchronization across the platform
- Venue yard management

**Platforms:** Android, iOS, macOS, Windows

### 4. **Dome.Shared** (`Dome.Shared.csproj`)
A shared class library containing common functionality used across all applications.

**Components:**
- Constants and enumerations
- Helper utilities (alerts, date handling)
- Local models (Country, VenueDate)
- Shared services and interfaces
- Authentication and sync logic
- Configuration management

## Technology Stack

- **Framework:** .NET 9.0 / .NET MAUI
- **UI Framework:** MAUI with Community Toolkit
- **Architecture:** MVVM (Model-View-ViewModel)
- **State Management:** CommunityToolkit.Mvvm
- **Database:** Realm
- **Authentication:** Ory Client
- **Cloud Storage:** AWS S3
- **Payment Processing:** Stripe
- **Communication:** Twilio (SMS), SendGrid (Email)
- **UI Components:** Syncfusion.Maui.Toolkit, Mopups (Popups), Sharpnado.Tabs
- **Code Enhancement:** Fody

## Prerequisites

- Visual Studio 2022 17.0 or later
- .NET 9.0 SDK
- Platform-specific SDKs:
  - Android SDK (API 21+ for Admin/Vendor, API 28+ for Player)
  - iOS SDK (15.0+)
  - Windows SDK (10.0.17763.0+) for Admin/Vendor
- AWS Account (for S3 storage)
- Stripe Account (for payment processing)
- Ory Account (for authentication)

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/HobDev/Dome.git
cd Dome
```

### 2. Restore Dependencies

Open the solution in Visual Studio or run:

```bash
dotnet restore Dome.sln
```

### 3. Configuration

Update the configuration file at `Dome.Shared/appsettings.json` with your API keys and endpoints:

- AWS credentials and S3 bucket information
- Stripe API keys
- Ory authentication endpoints
- Twilio credentials
- SendGrid API key

### 4. Build the Solution

```bash
dotnet build Dome.sln
```

### 5. Run the Applications

#### Player App:
```bash
dotnet build Player/Player.csproj -f net9.0-android
# or
dotnet build Player/Player.csproj -f net9.0-ios
```

#### Vendor App:
```bash
dotnet build Vendor/Vendor.csproj -f net9.0-android
# or choose other platforms: net9.0-ios, net9.0-maccatalyst, net9.0-windows10.0.19041.0
```

#### Admin App:
```bash
dotnet build Admin/Admin.csproj -f net9.0-android
# or choose other platforms: net9.0-ios, net9.0-maccatalyst, net9.0-windows10.0.19041.0
```

## Project Architecture

### Design Patterns

- **MVVM:** All applications follow the Model-View-ViewModel pattern
- **Dependency Injection:** Services are registered and injected through .NET MAUI's built-in DI container
- **Repository Pattern:** Data access is abstracted through service interfaces
- **Command Pattern:** User interactions are handled through RelayCommand from CommunityToolkit.Mvvm

### Key Components

#### Custom Controls
Each application includes custom controls for:
- Borders, Buttons, Entries, Labels, Pickers
- Collection Views
- Shell navigation customization

#### Services
- Authentication and authorization
- Data synchronization
- API communication
- Local storage management
- Push notifications

#### Models
- Local models for offline functionality
- Synced models for server data
- View models for UI state management

## Environment Configuration

The application supports two environments configured in `AppConstants.cs`:

- **Development:** For testing and development
- **Production:** For live deployment

Current environment is set to `production`. Change this in `Dome.Shared/Constants/AppConstants.cs`.

## Booking Statuses

The system uses the following booking workflow:

1. **Created:** Initial booking selection
2. **Reserved:** Slot confirmed, payment pending
3. **Booked:** Payment completed, booking confirmed
4. **Completed:** Booking time elapsed
5. **Cancelled:** Booking cancelled by user or system

## Application Identifiers

- **Player:** `com.daflo.dome`
- **Vendor:** `com.daflo.vendor` (displayed as "Partner")
- **Admin:** `com.daflo.admin`

## Version Information

- **Player:** v1.0 (Build 11)
- **Vendor:** v1.0 (Build 10)
- **Admin:** v1.0 (Build 2)

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

[Add your license information here]

## Support

For support and inquiries, please contact [Add contact information here]

## Acknowledgments

- Built with .NET MAUI
- Uses CommunityToolkit for MAUI
- Powered by Syncfusion components
- Payment processing by Stripe
- Authentication by Ory
