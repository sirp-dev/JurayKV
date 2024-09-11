namespace JurayKV.Application.Services.AwsDtos
{
    public class S3GetObjectDto
    {
        public string? Key { get; set; }
        public string? Bucket { get; set; }
    }
}
