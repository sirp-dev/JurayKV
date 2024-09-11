using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Commands.TransactionCommands
{
    public class TransactionResponseDto
    {
        public bool Success { get; set; }
        public string Area { get; set; }
        public string Path { get; set; }
        public string Note { get; set; }
    }
}
