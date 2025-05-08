namespace BlazorTest.Models;

public class ActivityItem
{
    public string User { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public DateTime Time { get; set; }
    public string Category { get; set; } = string.Empty;
}
