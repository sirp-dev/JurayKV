using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Queries.IdentityKvAdQueries
{
    public class IdentityKvAdListDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get;  set; }
        public string Fullname { get;  set; }
        public Guid KvAdId { get;  set; }
        public string KvAdName { get; set; }
        public string Company { get; set; }
        public string Activity { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? LastModifiedAtUtc { get; set; }
        public string VideoUrl { get; set; }
        public string ImageUrl { get; set; }
        public string ImageKey { get; set; }
        public bool Active { get; set; }
        public AdsStatus AdsStatus { get; set; }
        public int Points { get;set; }
    }
}
