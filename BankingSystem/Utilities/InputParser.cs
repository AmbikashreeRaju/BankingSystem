using BankingSystem.Models;

namespace BankingSystem.Utilities;

public static class InputParser
{
    public static bool TryParseTransaction(string input, out DateTime date, out string acc, out string type, out decimal amount, out string error)
    {
        date = default;
        acc = type = error = string.Empty;
        amount = 0;

        var parts = input.Split();
        if (parts.Length != 4)
        {
            error = "Invalid input format.";
            return false;
        }

        if (!DateTime.TryParseExact(parts[0], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out date))
        {
            error = "Invalid date format.";
            return false;
        }

        acc = parts[1];
        type = parts[2].ToUpper();
        if (type != "D" && type != "W")
        {
            error = "Type must be D or W.";
            return false;
        }

        if (!decimal.TryParse(parts[3], out amount) || amount <= 0 || decimal.Round(amount, 2) != amount)
        {
            error = "Amount must be > 0 with max 2 decimal places.";
            return false;
        }

        return true;
    }

    public static bool TryParseInterestRule(string input, out InterestRule rule, out string error)
    {
        rule = new();
        error = string.Empty;
        var parts = input.Split();
        if (parts.Length != 3 ||
            !DateTime.TryParseExact(parts[0], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var date) ||
            !decimal.TryParse(parts[2], out var rate) || rate <= 0 || rate >= 100)
        {
            error = "Invalid format or data values.";
            return false;
        }

        rule.Date = date;
        rule.RuleId = parts[1];
        rule.Rate = rate;
        return true;
    }
}