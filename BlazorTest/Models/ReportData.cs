namespace BlazorTest.Models;

public class ReportData
{
    public string ReportTitle { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
    public List<MonthlyRevenueItem> MonthlyRevenue { get; set; } = new List<MonthlyRevenueItem>();
    public string Summary { get; set; } = string.Empty;
}
