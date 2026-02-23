namespace Dome.Shared.Synced.Payment
{
    public partial class Charge : IRealmObject
    {
        [MapTo("_id")]
        [PrimaryKey]
        public ObjectId Id { get; set; }

        [MapTo("convenienceFeePercent")]
        public decimal ConvenienceFeePercent { get; set; }

        [MapTo("taxPercent")]
        public decimal TaxPercent { get; set; }
    }
}
