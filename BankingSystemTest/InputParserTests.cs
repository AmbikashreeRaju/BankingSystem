using BankingSystem.Utilities;
using Xunit;

namespace BankingSystem.Tests;

public class InputParserTests
{
    [Fact]
    public void TryParseTransaction_ValidInput_ReturnsTrue()
    {
        var input = "20230601 AC001 D 100.00";
        var result = InputParser.TryParseTransaction(input, out var date, out var acc, out var type, out var amount, out var error);

        Assert.True(result);
        Assert.Equal("AC001", acc);
        Assert.Equal("D", type);
        Assert.Equal(100.00m, amount);
        Assert.Equal(new DateTime(2023, 6, 1), date);
        Assert.True(string.IsNullOrEmpty(error));
    }

    [Fact]
    public void TryParseInterestRule_InvalidRate_ReturnsFalse()
    {
        var input = "20230601 RULEX 0";
        var result = InputParser.TryParseInterestRule(input, out var rule, out var error);

        Assert.False(result);
        Assert.NotNull(error);
    }
}