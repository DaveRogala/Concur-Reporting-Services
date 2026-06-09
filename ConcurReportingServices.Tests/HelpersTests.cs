using ConcurReporting.Helpers;

namespace ConcurReportingServices.Tests;

public class HelpersTests
{
    // ── ToDateOnly ──────────────────────────────────────────────────────────

    [Theory]
    [InlineData("2024-03-15", 2024, 3, 15)]
    [InlineData("2000-01-01", 2000, 1, 1)]
    [InlineData("1999-12-31", 1999, 12, 31)]
    public void ToDateOnly_ValidIso8601Date_ReturnsParsedDate(string input, int year, int month, int day)
    {
        var result = input.ToDateOnly();

        Assert.Equal(new DateOnly(year, month, day), result);
    }

    [Theory]
    [InlineData("15-03-2024")]   // day-month-year (old wrong format)
    [InlineData("2024/03/15")]   // slashes instead of dashes
    [InlineData("2024-3-15")]    // no zero-padding
    [InlineData("not-a-date")]
    [InlineData("")]
    public void ToDateOnly_InvalidFormat_ReturnsNull(string input)
    {
        Assert.Null(input.ToDateOnly());
    }

    // ── ToDateTime ──────────────────────────────────────────────────────────

    [Fact]
    public void ToDateTime_ValidIso8601DateTime_ReturnsParsedDateTime()
    {
        var result = "2024-03-15T10:30:00".ToDateTime();

        Assert.Equal(new DateTime(2024, 3, 15, 10, 30, 0), result);
    }

    [Theory]
    [InlineData("2024-03-15")]          // date only, no time part
    [InlineData("2024-03-15 10:30:00")] // space separator instead of T
    [InlineData("not-a-date")]
    [InlineData("")]
    public void ToDateTime_InvalidFormat_ReturnsNull(string input)
    {
        Assert.Null(input.ToDateTime());
    }

    // ── ToBoolean ───────────────────────────────────────────────────────────

    [Fact]
    public void ToBoolean_Y_ReturnsTrue()
    {
        Assert.True("Y".ToBoolean());
    }

    [Fact]
    public void ToBoolean_N_ReturnsFalse()
    {
        Assert.False("N".ToBoolean());
    }

    [Theory]
    [InlineData("y")]
    [InlineData("n")]
    [InlineData("Yes")]
    [InlineData("No")]
    [InlineData("true")]
    [InlineData("")]
    [InlineData("X")]
    public void ToBoolean_UnrecognisedValue_ReturnsNull(string input)
    {
        Assert.Null(input.ToBoolean());
    }

    // ── ToDecimal ───────────────────────────────────────────────────────────

    [Theory]
    [InlineData("1.23", 1.23)]
    [InlineData("0", 0)]
    [InlineData("-99.5", -99.5)]
    public void ToDecimal_ValidDecimalString_ReturnsParsedValue(string input, double expected)
    {
        Assert.Equal((decimal)expected, input.ToDecimal());
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("")]
    [InlineData("1,23")]  // comma decimal separator not valid with default culture
    public void ToDecimal_InvalidString_ReturnsNull(string input)
    {
        Assert.Null(input.ToDecimal());
    }

    // ── ToInt ───────────────────────────────────────────────────────────────

    [Theory]
    [InlineData("42", 42)]
    [InlineData("0", 0)]
    [InlineData("-7", -7)]
    public void ToInt_ValidIntString_ReturnsParsedValue(string input, int expected)
    {
        Assert.Equal(expected, input.ToInt());
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("")]
    [InlineData("1.5")]
    public void ToInt_InvalidString_ReturnsNull(string input)
    {
        Assert.Null(input.ToInt());
    }
}
