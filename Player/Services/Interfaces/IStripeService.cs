namespace Player.Services.Interfaces
{
    public interface IStripeService
    {
        Task<CustomerCreateResponse?> CreateCustomer(CustomerCreateRequest customerCreateRequest);

        Task<ValidateAvailabilityResponse?> CreateBooking(CreateBookingsRequest request);

        Task<PaymentIntentCreateResponse?> FetchPaymentIntent(RequestWithBookingIds request);

        Task<CancelPaymentIntentResponse?> CancelPaymentIntent(RequestWithBookingIds request);

        Task<CancelBookingResponse?> CancelBooking(RequestWithBookingIds request);
    }
}
