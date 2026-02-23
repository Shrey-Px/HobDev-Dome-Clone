namespace Dome.Shared.Constants
{
    /// <summary>
    ///   all the constants used in the app
    /// </summary>
    public class AppConstants
    {
        public static readonly int[] MultiSportCourtNumbers = { 19, 20, 21, 22 };

        public static readonly string[] MultiSportOptions = { "Badminton", "Pickleball" };

        // the booking is selected from the Booking timing view. This is the initial status of the booking. The server will change the status to Reserved if the slot is free and then will create a payment Intent for the booking.
        public const string Created = "Created";

        //the payment process of the booking is started but not paid
        public const string Reserved = "Reserved";

        //the booking is paid and confirmed
        public const string Booked = "Booked";

        //the paid booking time has elapsed. This needs to be implemented on backend
        public const string Completed = "Completed";

        //the booking is cancelled by the user or cancelled from the backend if not paid within a certain time
        public const string Cancelled = "Cancelled";

        // the dome app will set it's environment from the environment variable
        public const string Environment = "development";

       // public const string Environment = "production";

        public const string BaseUrl = "https://kind-kapitsa-rmqphy91zf.projects.oryapis.com";
    }
}
