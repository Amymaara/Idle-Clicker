public static class NumberFormatter
{
    public static string FormatMoney(double value)
    {
        if (value >= 1_000_000_000_000_000_000d)
            return "$" + (value / 1_000_000_000_000_000_000d).ToString("0.##") + "Qi";

        if (value >= 1_000_000_000_000_000d)
            return "$" + (value / 1_000_000_000_000_000d).ToString("0.##") + "Qa";

        if (value >= 1_000_000_000_000d)
            return "$" + (value / 1_000_000_000_000d).ToString("0.##") + "T";

        if (value >= 1_000_000_000d)
            return "$" + (value / 1_000_000_000d).ToString("0.##") + "B";

        if (value >= 1_000_000d)
            return "$" + (value / 1_000_000d).ToString("0.##") + "M";

        if (value >= 1_000d)
            return "$" + (value / 1_000d).ToString("0.##") + "K";

        return "$" + value.ToString("F0");
    }
}