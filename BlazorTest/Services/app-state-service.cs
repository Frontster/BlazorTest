namespace BlazorTest.Services;

/// <summary>
/// This service manages the global application state including:
/// - The currently selected category from the dropdown
/// - Application-wide loading state
/// - Authentication state
/// </summary>
public class AppStateService
{
    // Selected category from the dropdown
    private string _selectedCategory = string.Empty;
    public string SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (_selectedCategory != value)
            {
                Console.WriteLine($"AppStateService: Category changing from '{_selectedCategory}' to '{value}'");
                _selectedCategory = value;
                NotifyStateChanged();
            }
        }
    }

    // Flag to track if the app is in a loading state
    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            if (_isLoading != value)
            {
                Console.WriteLine($"AppStateService: IsLoading changing from {_isLoading} to {value}");
                _isLoading = value;
                NotifyStateChanged();
            }
        }
    }

    // Flag to track if the user is authenticated
    private bool _isAuthenticated;
    public bool IsAuthenticated
    {
        get => _isAuthenticated;
        set
        {
            if (_isAuthenticated != value)
            {
                Console.WriteLine($"AppStateService: IsAuthenticated changing from {_isAuthenticated} to {value}");
                _isAuthenticated = value;
                NotifyStateChanged();
            }
        }
    }

    /// <summary>
    /// Publishes state change events to subscribers
    /// </summary>
    public event Action? OnChange;

    /// <summary>
    /// Method to notify subscribers about state changes
    /// </summary>
    private void NotifyStateChanged()
    {
        Console.WriteLine("AppStateService: Notifying state changed");
        OnChange?.Invoke();
    }

    /// <summary>
    /// Simulates a loading delay with a cancellable task
    /// </summary>
    /// <param name="milliseconds">The delay in milliseconds</param>
    public async Task SimulateLoadingAsync(int milliseconds = 1000)
    {
        IsLoading = true;
        Console.WriteLine($"AppStateService: Simulating loading for {milliseconds}ms");
        await Task.Delay(milliseconds);
        IsLoading = false;
    }

    /// <summary>
    /// Initialize the app state with default values
    /// </summary>
    public void InitializeDefaults()
    {
        Console.WriteLine("AppStateService: Initializing defaults");
        // Set default category if not already set
        if (string.IsNullOrEmpty(_selectedCategory))
        {
            _selectedCategory = "Category 1";
            Console.WriteLine($"AppStateService: Set default category to {_selectedCategory}");
        }
    }

    /// <summary>
    /// Reset the app state when logging out
    /// </summary>
    public void Reset()
    {
        Console.WriteLine("AppStateService: Resetting state");
        IsAuthenticated = false;
        SelectedCategory = string.Empty;
    }
}