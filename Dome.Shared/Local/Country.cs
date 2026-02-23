namespace Dome.Shared.Local
{
    /// <summary>
    /// the country model is used to bind the country's phone code and flag image with the UI when filling the user's mobile number
    /// </summary>
    public class Country
    {
        public Country(string countryName, string code, string flagImage)
        {
            CountryName = countryName;
            Code = code;
            FlagImage = flagImage;
        }

        public string CountryName { get; set; }

        public string Code { get; set; }

        public string FlagImage { get; set; }
    }
}
