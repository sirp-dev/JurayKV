﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Queries.ExchangeRateQueries
{
    public class ExchangeRateListDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
