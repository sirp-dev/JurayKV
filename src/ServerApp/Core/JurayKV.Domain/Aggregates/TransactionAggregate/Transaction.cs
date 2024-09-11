using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.WalletAggregate;
using JurayKV.Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Domain.Aggregates.TransactionAggregate
{
    public sealed class Transaction : AggregateRoot
    {
        public Transaction(Guid id)
        {
            Id = id;
            CreatedAtUtc = DateTime.UtcNow;
        }

        // This is needed for EF Core query mapping or deserialization.
        public Transaction()
        {
        }
        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string UniqueReference { get; set; }
        public string OptionalNote { get; set; }
        public decimal Amount { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public TransactionTypeEnum TransactionType { get; set; }
        public EntityStatus Status { get; set; }
        public string TransactionReference { get; set; }
        public string Description { get; set; }
        public string TrackCode { get; set; }
        public string TransactionVerificationId { get; set; }
    }
}

