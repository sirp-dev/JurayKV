using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Queries.IdentityKvAdQueries
{
    public class IdentityKvAdDetailsDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get;  set; }
        public string Fullname { get;  set; }
        public Guid KvAdId { get;  set; }
        public string KvAdName { get;  set; }
        public string? KvAdImage { get;  set; }
        public string? KvAdImageKey { get;  set; }
        public string Activity { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? LastModifiedAtUtc { get; set; }
        public string VideoUrl { get; set; }
        public bool Active { get; set; }
        public AdsStatus AdsStatus { get; set; }
        public Guid CompanyId { get; set; }
        public int ResultOne { get; set; }
        public int ResultTwo { get; set; }
        public int ResultThree { get; set; }
    }
}
