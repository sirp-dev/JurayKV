using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.KvAdAggregate;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Commands.IdentityKvAdCommands
{
    public class IdentityKvAdCreateDto
    {

        public Guid UserId { get; set; }
        public Guid KvAdId { get; set; }
        public string VideoUrl { get; set; }
        public string VideoKey { get; set; }
    }
}
