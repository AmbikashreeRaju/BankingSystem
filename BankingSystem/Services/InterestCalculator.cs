using BankingSystem.Models;

namespace BankingSystem.Services;

public class InterestCalculator
{
    public decimal Calculate(Account account, SortedList<DateTime, InterestRule> rules, DateTime from, DateTime to)
    {
        if (account == null || account.Transactions == null || !account.Transactions.Any())
            return 0;

        decimal interest = 0;
        decimal balance = 0;

        // Sort transactions and apply them up to the start date
        var transactions = account.Transactions.OrderBy(t => t.Date).ToList();
        int txnIndex = 0;

        // Build a running balance starting from transactions before 'from'
        while (txnIndex < transactions.Count && transactions[txnIndex].Date < from)
        {
            var t = transactions[txnIndex++];
            balance += t.Type.ToUpper() == "D" ? t.Amount : -t.Amount;
        }

        for (var date = from; date <= to; date = date.AddDays(1))
        {
            // Apply any transactions on this date
            while (txnIndex < transactions.Count && transactions[txnIndex].Date == date)
            {
                var t = transactions[txnIndex++];
                balance += t.Type.ToUpper() == "D" ? t.Amount : -t.Amount;
            }

            // Find latest rule as of this date
            var rule = rules.LastOrDefault(r => r.Key <= date).Value;
            if (rule != null)
            {
                interest += (balance * rule.Rate * 1) / 100;
            }
        }

        return Math.Round(interest / 365, 2, MidpointRounding.AwayFromZero); // annualized
    }
}
