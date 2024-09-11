namespace JurayKV.WebApi.Dto
{
    public class AirtimeRequestDto
    {
        public Guid BillerId { get; set; }
        public string PhoneNumber { get; set; }
        public string Network { get; set; }
        public string Amount { get; set; }
    }
}
