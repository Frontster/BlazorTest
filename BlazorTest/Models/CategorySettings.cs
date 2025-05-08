namespace BlazorTest.Models;

public class CategorySettings
{
    public string DisplayName { get; set; } = string.Empty;
    public bool Notifications { get; set; }
    public bool AutoRefresh { get; set; }
    public int RefreshInterval { get; set; } = 60;
}
