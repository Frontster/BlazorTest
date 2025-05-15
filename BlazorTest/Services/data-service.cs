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
    private readonly SemaphoreSlim _apiCallLock = new SemaphoreSlim(1, 1);

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
        Console.WriteLine("DataService: Constructor called");
    }

    /// <summary>
    /// Fetches dashboard data based on the selected category
    /// </summary>
    /// <returns>Dashboard data model</returns>
    public async Task<DashboardData> GetDashboardDataAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"DataService: Getting dashboard data for category: {_appStateService.SelectedCategory}");
        await SimulateApiCallAsync(1200, cancellationToken);

        var category = _appStateService.SelectedCategory;
        var data = new DashboardData
        {
            Title = $"{category} Dashboard",
            TotalItems = _random.Next(100, 500),
            CompletedItems = _random.Next(50, 100),
            RecentActivity = GenerateRecentActivity(category, 5)
        };

        Console.WriteLine($"DataService: Retrieved dashboard data with {data.RecentActivity.Count} activities");
        return data;
    }

    /// <summary>
    /// Fetches product data based on the selected category
    /// </summary>
    /// <returns>List of product models</returns>
    public async Task<List<Product>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"DataService: Getting products for category: {_appStateService.SelectedCategory}");
        await SimulateApiCallAsync(1500, cancellationToken);

        var category = _appStateService.SelectedCategory;
        var products = Enumerable.Range(1, 10)
            .Select(i => new Product
            {
                Id = i,
                Name = $"{category} Product {i}",
                Description = $"Description for {category} product {i}",
                Price = Math.Round(_random.NextDouble() * 100 + 10, 2),
                InStock = _random.Next(0, 50)
            })
            .ToList();

        Console.WriteLine($"DataService: Retrieved {products.Count} products");
        return products;
    }

    /// <summary>
    /// Fetches user data based on the selected category
    /// </summary>
    /// <returns>List of user models</returns>
    public async Task<List<User>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"DataService: Getting users for category: {_appStateService.SelectedCategory}");
        await SimulateApiCallAsync(1000, cancellationToken);

        var category = _appStateService.SelectedCategory;
        var users = Enumerable.Range(1, 8)
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

        Console.WriteLine($"DataService: Retrieved {users.Count} users");
        return users;
    }

    /// <summary>
    /// Fetches report data based on the selected category
    /// </summary>
    /// <returns>Report data model</returns>
    public async Task<ReportData> GetReportDataAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"DataService: Getting report data for category: {_appStateService.SelectedCategory}");
        await SimulateApiCallAsync(1800, cancellationToken);

        var category = _appStateService.SelectedCategory;
        var reportData = new ReportData
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

        Console.WriteLine($"DataService: Retrieved report data with {reportData.MonthlyRevenue.Count} revenue entries");
        return reportData;
    }

    /// <summary>
    /// Fetches settings data based on the selected category
    /// </summary>
    /// <returns>Settings data model</returns>
    public async Task<SettingsData> GetSettingsAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"DataService: Getting settings for category: {_appStateService.SelectedCategory}");
        await SimulateApiCallAsync(800, cancellationToken);

        var category = _appStateService.SelectedCategory;
        var settings = new SettingsData
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
                PageSize = (new int[] { 10, 20, 30, 40, 50 })[_random.Next(0, 5)]
            }
        };

        Console.WriteLine($"DataService: Retrieved settings with theme: {settings.UserPreferences.Theme}");
        return settings;
    }

    /// <summary>
    /// Simulates an API call with a delay
    /// </summary>
    /// <param name="maxDelay">Maximum delay in milliseconds</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
    private async Task SimulateApiCallAsync(int maxDelay, CancellationToken cancellationToken = default)
    {
        // Use a lock to prevent multiple API calls from interfering with each other
        await _apiCallLock.WaitAsync(cancellationToken);

        try
        {
            // Don't set global loading state - let components manage their own loading states
            // _appStateService.IsLoading = true; // Removed global loading state
            
            var delay = _random.Next(500, maxDelay);
            Console.WriteLine($"DataService: Simulating API call with {delay}ms delay");

            // Use task delay with cancellation token
            await Task.Delay(delay, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("DataService: API call was cancelled");
            throw; // Re-throw to allow caller to handle
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"DataService: Error during API call simulation: {ex.Message}");
            throw; // Re-throw to allow caller to handle
        }
        finally
        {
            // Don't reset global loading state - let components manage their own loading states
            // _appStateService.IsLoading = false; // Removed global loading state
            _apiCallLock.Release();
        }
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

        var result = Enumerable.Range(1, count)
            .Select(i => new ActivityItem
            {
                User = users[_random.Next(users.Count)],
                Action = activities[_random.Next(activities.Count)],
                Time = DateTime.Now.AddHours(-i),
                Category = category
            })
            .ToList();

        Console.WriteLine($"DataService: Generated {result.Count} activity items");
        return result;
    }
}