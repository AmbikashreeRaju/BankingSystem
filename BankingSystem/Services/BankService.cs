using BankingSystem.Models;
using BankingSystem.Utilities;

namespace BankingSystem.Services;

public class BankService
{
    private readonly Dictionary<string, Account> _accounts = new();
    private readonly SortedList<DateTime, InterestRule> _interestRules = new();
    private readonly InterestCalculator _interestCalculator = new();
    private readonly Dictionary<string, int> _txnCounter = new();

    public void InputTransaction()
    {
        while (true)
        {
            Console.WriteLine("Please enter transaction details in <Date> <Account> <Type> <Amount> format\n(or enter blank to go back to main menu):");
            Console.Write("> ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) break;

            if (!InputParser.TryParseTransaction(input, out var date, out var accId, out var type, out var amount, out var error))
            {
                Console.WriteLine($"Error: {error}");
                continue;
            }

            if (!_accounts.TryGetValue(accId, out var account))
            {
                if (type.ToUpper() == "W")
                {
                    Console.WriteLine("Error: Cannot withdraw from a non-existent account.");
                    continue;
                }
                account = new Account { AccountId = accId };
                _accounts[accId] = account;
            }

            var currentBalance = account.GetBalance();
            if (type.ToUpper() == "W" && currentBalance < amount)
            {
                Console.WriteLine("Error: Insufficient funds.");
                continue;
            }

            var txnCount = _txnCounter.TryGetValue(date.ToString("yyyyMMdd"), out var count) ? count + 1 : 1;
            _txnCounter[date.ToString("yyyyMMdd")] = txnCount;

            var txn = new Transaction
            {
                Date = date,
                Type = type.ToUpper(),
                Amount = amount,
                TxnId = $"{date:yyyyMMdd}-{txnCount:D2}"
            };
            account.Transactions.Add(txn);
            Console.WriteLine($"Account: {accId}");
            PrintAccountTransactions(account);
        }
    }

    public void DefineInterestRules()
    {
        while (true)
        {
            Console.WriteLine("Please enter interest rules details in <Date> <RuleId> <Rate in %> format\n(or enter blank to go back to main menu):");
            Console.Write("> ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) break;

            if (!InputParser.TryParseInterestRule(input, out var rule, out var error))
            {
                Console.WriteLine($"Error: {error}");
                continue;
            }

            _interestRules[rule.Date] = rule;
            Console.WriteLine("Interest rules:");
            Console.WriteLine("| Date     | RuleId | Rate (%) |");
            foreach (var r in _interestRules.Values)
            {
                Console.WriteLine($"| {r.Date:yyyyMMdd} | {r.RuleId,-6} | {r.Rate,8:F2} |");
            }
        }
    }

    public void PrintStatement()
    {
        Console.WriteLine("Please enter account and month to generate the statement <Account> <Year><Month>\n(or enter blank to go back to main menu):");
        Console.Write("> ");
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input)) return;

        var parts = input.Split();
        if (parts.Length != 2 || !_accounts.TryGetValue(parts[0], out var account) ||
            !DateTime.TryParseExact(parts[1] + "01", "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var month))
        {
            Console.WriteLine("Invalid input or account not found.");
            return;
        }

        var startDate = new DateTime(month.Year, month.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var interest = _interestCalculator.Calculate(account, _interestRules, startDate, endDate);

        if (interest > 0)
        {
            account.Transactions.Add(new Transaction
            {
                Date = endDate,
                Type = "I",
                Amount = interest,
                TxnId = ""
            });
        }

        Console.WriteLine($"Account: {account.AccountId}");
        Console.WriteLine("| Date     | Txn Id      | Type | Amount | Balance |");
        var balance = 0m;
        foreach (var txn in account.Transactions.Where(t => t.Date >= startDate && t.Date <= endDate).OrderBy(t => t.Date))
        {
            balance += txn.Type switch
            {
                "D" => txn.Amount,
                "W" => -txn.Amount,
                "I" => txn.Amount,
                _ => 0
            };
            Console.WriteLine($"| {txn.Date:yyyyMMdd} | {txn.TxnId,-11} | {txn.Type}    | {txn.Amount,6:F2} | {balance,7:F2} |");
        }
    }

    private static void PrintAccountTransactions(Account account)
    {
        Console.WriteLine("| Date     | Txn Id      | Type | Amount |");
        foreach (var txn in account.Transactions.OrderBy(t => t.Date))
        {
            Console.WriteLine($"| {txn.Date:yyyyMMdd} | {txn.TxnId,-11} | {txn.Type}    | {txn.Amount,6:F2} |");
        }
    }
}