using BlazorTest.Models;

namespace BlazorTest.Services;

/// <summary>
/// This service handles data operations for each page/component including:
/// - Fetching data based on the selected category
/// - Simulating API calls with delays and loading states
/// </summary>
public class DataService
{
    private readonly AppStateService _appStateService;
    private readonly Random _random = new Random();

    // Available categories for the dropdown
    public List<string> AvailableCategories { get; } = new List<string>
    {
        "Category 1",
        "Category 2",
        "Category 3",
        "Category 4",
        "Category 5"
    };

    public DataService(AppStateService appStateService)
    {
        _appStateService = appStateService;
    }

    /// <summary>
    /// Fetches dashboard data based on the selected category
    /// </summary>
    /// <returns>Dashboard data model</returns>
    public async Task<DashboardData> GetDashboardDataAsync()
    {
        await SimulateApiCallAsync(1200);
        
        var category = _appStateService.SelectedCategory;
        return new DashboardData
        {
            Title = $"{category} Dashboard",
            TotalItems = _random.Next(100, 500),
            CompletedItems = _random.Next(50, 100),
            RecentActivity = GenerateRecentActivity(category, 5)
        };
    }

    /// <summary>
    /// Fetches product data based on the selected category
    /// </summary>
    /// <returns>List of product models</returns>
    public async Task<List<Product>> GetProductsAsync()
    {
        await SimulateApiCallAsync(1500);
        
        var category = _appStateService.SelectedCategory;
        return Enumerable.Range(1, 10)
            .Select(i => new Product
            {
                Id = i,
                Name = $"{category} Product {i}",
                Description = $"Description for {category} product {i}",
                Price = Math.Round(_random.NextDouble() * 100 + 10, 2),
                InStock = _random.Next(0, 50)
            })
            .ToList();
    }

    /// <summary>
    /// Fetches user data based on the selected category
    /// </summary>
    /// <returns>List of user models</returns>
    public async Task<List<User>> GetUsersAsync()
    {
        await SimulateApiCallAsync(1000);
        
        var category = _appStateService.SelectedCategory;
        return Enumerable.Range(1, 8)
            .Select(i => new User
            {
                Id = i,
                UserName = $"user{i}_{category.Replace(" ", "").ToLower()}",
                FullName = $"User {i} ({category})",
                Email = $"user{i}@{category.Replace(" ", "").ToLower()}.com",
                Role = i % 3 == 0 ? "Admin" : "User",
                LastActive = DateTime.Now.AddDays(-_random.Next(0, 10))
            })
            .ToList();
    }

    /// <summary>
    /// Fetches report data based on the selected category
    /// </summary>
    /// <returns>Report data model</returns>
    public async Task<ReportData> GetReportDataAsync()
    {
        await SimulateApiCallAsync(1800);
        
        var category = _appStateService.SelectedCategory;
        return new ReportData
        {
            ReportTitle = $"{category} Performance Report",
            GeneratedAt = DateTime.Now,
            MonthlyRevenue = Enumerable.Range(1, 12)
                .Select(i => new MonthlyRevenueItem
                {
                    Month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMM"),
                    Revenue = _random.Next(5000, 20000)
                })
                .ToList(),
            Summary = $"This is a summary of {category} performance over the last year."
        };
    }

    /// <summary>
    /// Fetches settings data based on the selected category
    /// </summary>
    /// <returns>Settings data model</returns>
    public async Task<SettingsData> GetSettingsAsync()
    {
        await SimulateApiCallAsync(800);
        
        var category = _appStateService.SelectedCategory;
        return new SettingsData
        {
            CategorySettings = new CategorySettings
            {
                DisplayName = category,
                Notifications = _random.Next(0, 2) == 1,
                AutoRefresh = _random.Next(0, 2) == 1,
                RefreshInterval = _random.Next(30, 120)
            },
            UserPreferences = new UserPreferences
            {
                Theme = _random.Next(0, 2) == 1 ? "Light" : "Dark",
                Language = _random.Next(0, 3) == 0 ? "English" : _random.Next(0, 2) == 0 ? "Spanish" : "French",
                PageSize = _random.Next(10, 51)
            }
        };
    }

    /// <summary>
    /// Simulates an API call with a delay
    /// </summary>
    /// <param name="maxDelay">Maximum delay in milliseconds</param>
    private async Task SimulateApiCallAsync(int maxDelay)
    {
        _appStateService.IsLoading = true;
        await Task.Delay(_random.Next(500, maxDelay));
        _appStateService.IsLoading = false;
    }

    /// <summary>
    /// Generates recent activity data for the dashboard
    /// </summary>
    /// <param name="category">The category to generate activities for</param>
    /// <param name="count">Number of activities to generate</param>
    /// <returns>List of activity items</returns>
    private List<ActivityItem> GenerateRecentActivity(string category, int count)
    {
        var activities = new List<string>
        {
            "added a new item",
            "updated settings",
            "removed an item", 
            "generated a report",
            "modified user permissions",
            "created a new user",
            "exported data",
            "imported data"
        };

        var users = new List<string>
        {
            "John", "Jane", "Mike", "Sarah", "David", "Emma"
        };

        return Enumerable.Range(1, count)
            .Select(i => new ActivityItem
            {
                User = users[_random.Next(users.Count)],
                Action = activities[_random.Next(activities.Count)],
                Time = DateTime.Now.AddHours(-i),
                Category = category
            })
            .ToList();
    }
}
