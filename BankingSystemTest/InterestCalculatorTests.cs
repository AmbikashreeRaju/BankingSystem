using BankingSystem.Models;
using BankingSystem.Services;
using Xunit;

namespace BankingSystem.Tests;

public class InterestCalculatorTests
{
    [Fact]
    public void CalculateInterest_SimpleCase_ReturnsCorrectInterest()
    {
        var account = new Account
        {
            AccountId = "AC001",
            Transactions = new List<Transaction>
            {
                new() { Date = new DateTime(2023, 6, 1), Type = "D", Amount = 1000, TxnId = "20230601-01" }
            }
        };

        var rules = new SortedList<DateTime, InterestRule>
        {
            [new DateTime(2023, 1, 1)] = new InterestRule { Date = new DateTime(2023, 1, 1), RuleId = "RULE01", Rate = 3.65m }
        };

        var calc = new InterestCalculator();
        var interest = calc.Calculate(account, rules, new DateTime(2023, 6, 1), new DateTime(2023, 6, 30));

        Assert.Equal(3.00m, interest); // (1000 * 3.65% * 30) / 365 = 3.00
    }
}