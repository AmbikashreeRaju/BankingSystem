using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Models
{
    public class InterestRule
    {
        public DateTime Date { get; set; }
        public string RuleId { get; set; } = string.Empty;
        public decimal Rate { get; set; } // In %
    }
}
