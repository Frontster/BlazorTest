namespace BlazorTest.Models;

public class DashboardData
{
    public string Title { get; set; } = string.Empty;
    public int TotalItems { get; set; }
    public int CompletedItems { get; set; }
    public List<ActivityItem> RecentActivity { get; set; } = new List<ActivityItem>();
}
