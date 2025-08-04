using BankingSystem.Services;
using System.IO;
using Xunit;

namespace BankingSystem.Tests;

public class BankServiceTests
{
    [Fact]
    public void InputTransaction_InvalidWithdrawalOnNewAccount_ShowsError()
    {        
        var input = new StringReader("20230601 AC002 W 100\n\n");
        var output = new StringWriter();
        Console.SetIn(input);
        Console.SetOut(output);

        var service = new BankService();
        service.InputTransaction();

        var result = output.ToString();
        Assert.Contains("Cannot withdraw from a non-existent account", result);
    }

    [Fact]
    public void DefineInterestRules_AddRule_ShowsInList()
    {
        var input = new StringReader("20230615 RULE05 2.25\n\n");
        var output = new StringWriter();
        Console.SetIn(input);
        Console.SetOut(output);

        var service = new BankService();
        service.DefineInterestRules();

        var result = output.ToString();
        Assert.Contains("RULE05", result);
        Assert.Contains("2.25", result);
    }

    [Fact]
    public void PrintStatement_ForExistingAccount_CalculatesInterestCorrectly()
    {
        var input = new StringReader("AC001 202306\n");
        var output = new StringWriter();
        Console.SetIn(input);
        Console.SetOut(output);

        var service = new BankService();

        var txnInput = new StringReader("20230601 AC001 D 1000\n\n");
        Console.SetIn(txnInput);
        service.InputTransaction();

        var ruleInput = new StringReader("20230101 RULEX 3.65\n\n");
        Console.SetIn(ruleInput);
        service.DefineInterestRules();

        Console.SetIn(input);
        Console.SetOut(output);
        service.PrintStatement();

        var result = output.ToString();
        Assert.Contains("Account: AC001", result);
        Assert.Contains("I", result); // Interest applied
    }
}