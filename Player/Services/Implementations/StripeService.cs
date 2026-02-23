using System.Text;

namespace Player.Services.Implementations
{
    public class StripeService : IStripeService
    {
        HttpClient httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://dafloserver.com/dome/stripe/"),
        };

        public async Task<CustomerCreateResponse?> CreateCustomer(
            CustomerCreateRequest customerCreateRequest
        )
        {
            var response = await httpClient.PostAsJsonAsync(
                "createCustomer",
                customerCreateRequest
            );

            var dataInJSON = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<CustomerCreateResponse>(dataInJSON);

            return data;
        }

        public async Task<ValidateAvailabilityResponse?> CreateBooking(
            CreateBookingsRequest request
        )
        {
            string? json = JsonSerializer.Serialize(
                request,
                new JsonSerializerOptions { WriteIndented = true }
            );

            System.Diagnostics.Debug.WriteLine($"JSON being sent: {json}");

            var response = await httpClient.PostAsync(
                "createBooking",
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            // var response = await httpClient.PostAsJsonAsync("createBooking", request);

            var dataInJSON = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<ValidateAvailabilityResponse>(dataInJSON);

            return data;
        }

        public async Task<PaymentIntentCreateResponse?> FetchPaymentIntent(
            RequestWithBookingIds request
        )
        {
            var response = await httpClient.PostAsJsonAsync("paymentIntent", request);

            var dataInJSON = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<PaymentIntentCreateResponse>(dataInJSON);

            return data;
        }

        public async Task<CancelPaymentIntentResponse?> CancelPaymentIntent(
            RequestWithBookingIds request
        )
        {
            var response = await httpClient.PostAsJsonAsync("cancelPaymentIntent", request);

            var dataInJSON = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<CancelPaymentIntentResponse>(dataInJSON);

            return data;
        }

        public async Task<CancelBookingResponse?> CancelBooking(RequestWithBookingIds request)
        {
            var response = await httpClient.PostAsJsonAsync("cancelBooking", request);

            var dataInJSON = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<CancelBookingResponse>(dataInJSON);

            return data;
        }
    }
}
