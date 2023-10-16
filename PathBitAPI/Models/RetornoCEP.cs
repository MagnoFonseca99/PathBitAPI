namespace PathBitAPI.Models
{
    public class RetornoCEP
    {
        public int? addressId { get; set; }
        public string? zipCode { get; set; }
        public string? zipCodeFormatted { get; set; }
        public string? addressType { get; set; }
        public string? addressName { get; set; }
        public string? referenceName { get; set; }
        public string? districtName { get; set; }
        public string? cityName { get; set; }
        public string? cityIbgeCode { get; set; }
        public string? cityDddCode { get; set; }
        public string? stateCode { get; set; }
        public string? stateName { get; set; }
        public string? countryCode { get; set; }
        public string? countryShortCode { get; set; }
        public string? countryName { get; set; }
        public string? countryDdi { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
    }
}
