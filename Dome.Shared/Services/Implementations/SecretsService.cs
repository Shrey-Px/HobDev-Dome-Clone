using System.Reflection;
using Amazon;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;

namespace Dome.Shared.Services.Implementations
{
    public class SecretsService : ISecretsService
    {
        private SecretsManagerCache? cache;

        IConfiguration? configuration;

        public string? TwilioAccountSid { get; set; }

        public string? TwilioAccountAuthToken { get; set; }

        public string? TwilioVerifyServiceSid { get; set; }

        public string? SendGridAPIKey { get; set; }

        public string? AWSS3AccessKey { get; set; }

        public string? AWSS3SecretAccessKey { get; set; }

        public string? AWSS3BucketName { get; set; }

        public string? RealmPlayerAppId { get; set; }

        public string? RealmAdminAppId { get; set; }

        public string? RealmVendorAppId { get; set; }

        public string? StripeSecretKey { get; set; }

        public SecretsService()
        {
            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("Dome.Shared.appsettings.json");
            configuration = new ConfigurationBuilder().AddJsonStream(stream).Build();

            string? accessKey = configuration["accessKey"];
            string? secretKey = configuration["secretKey"];
            string? region = configuration["region"];

            // AWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);
            // IAmazonSecretsManager amazonSecretsManager = new AmazonSecretsManagerClient(
            //     credentials,
            //     RegionEndpoint.GetBySystemName(region)
            // );
            // SecretCacheConfiguration cacheConfig = new SecretCacheConfiguration
            // {
            //     // the TTL cache refresh duration to 24 hours
            //     CacheItemTTL = 86400000,
            // };

            // cache = new SecretsManagerCache(amazonSecretsManager, cacheConfig);
        }

        public async Task LoadAllSecrets()
        {
            string secretName = string.Empty;
            string environment = AppConstants.Environment;

            if (environment == "production")
            {
                // stripe
                // StripeSecretKey = await GetSecret("prod/DomeMobile/StripeLiveSecretKey");
                // StripeSecretKey = await GetSecret("dev/DomeMobile/StripeTestSecretKey");

                // // mongo DB
                // RealmPlayerAppId = await GetSecret("prod/DomeMobile/MongoDBAppId");
                // RealmAdminAppId = await GetSecret("prod/DomeAdmin/MongoDBAppId");
                // RealmVendorAppId = await GetSecret("prod/DomeVendor/MongoDBAppId");

                // stripe
                //StripeSecretKey = configuration["stripeSecretKey"];
                StripeSecretKey = configuration["stripeSecretKey"];

                // mongo DB
                RealmPlayerAppId = configuration["realmPlayerAppId"];
                RealmAdminAppId = configuration["realmAdminAppId"];
                RealmVendorAppId = configuration["realmVendorAppId"];
            }
            else if (environment == "development")
            {
                // stripe
                // StripeSecretKey = await GetSecret("dev/DomeMobile/StripeTestSecretKey");

                // // mongo DB
                // RealmPlayerAppId = await GetSecret("dev/DomeMobile/MongoDBAppId");
                // RealmAdminAppId = await GetSecret("dev/DomeAdmin/MongoDBAppId");
                // RealmVendorAppId = await GetSecret("dev/DomeVendor/MongoDBAppId");

                StripeSecretKey = configuration["stripeSecretKeyDev"];

                // mongo DB
                // RealmPlayerAppId = configuration["realmPlayerAppIdDev"];
                // RealmAdminAppId = configuration["realmAdminAppIdDev"];
                // RealmVendorAppId = configuration["realmVendorAppIdDev"];

                RealmPlayerAppId = configuration["realmPlayerAppId"];
                RealmAdminAppId = configuration["realmAdminAppId"];
                RealmVendorAppId = configuration["realmVendorAppId"];
            }

            // twilio
            TwilioAccountSid = configuration["twilioAccountSid"];
            TwilioAccountAuthToken = configuration["twilioAccountAuthToken"];
            TwilioVerifyServiceSid = configuration["twilioVerifyServiceSid"];

            // sendgrid
            SendGridAPIKey = configuration["sendGridAPIKey"];

            // aws s3
            AWSS3AccessKey = configuration["awsS3AccessKey"];
            AWSS3SecretAccessKey = configuration["awsS3SecretAccessKey"];
            AWSS3BucketName = configuration["awsS3BucketName"];
        }

        public async Task<string> GetSecret(string secretName)
        {
            string response = string.Empty;

            try
            {
                response = await cache.GetSecretString(secretName);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }

            return response;
        }
    }
}
