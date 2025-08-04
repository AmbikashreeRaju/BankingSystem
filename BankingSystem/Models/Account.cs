using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Models
{
    public class Account
    {
        public string AccountId { get; set; } = string.Empty;
        public List<Transaction> Transactions { get; set; } = new();

        public decimal GetBalanceUpTo(DateTime date)
        {
            return Transactions
                .Where(t => t.Date <= date)
                .Sum(t => t.Type.ToUpper() switch
                {
                    "D" => t.Amount,
                    "W" => -t.Amount,
                    "I" => t.Amount,
                    _ => 0
                });
        }

        public decimal GetBalance() => GetBalanceUpTo(DateTime.MaxValue);
    }
}
