using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Models
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public string TxnId { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // D/W/I
        public decimal Amount { get; set; }
    }
}
