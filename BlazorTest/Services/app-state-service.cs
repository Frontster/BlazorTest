namespace BlazorTest.Services;

/// <summary>
/// This service manages the global application state including:
/// - The currently selected category from the dropdown
/// - Application-wide loading state
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
                _isAuthenticated = value;
                NotifyStateChanged();
            }
        }
    }

    // Event for notifying components about state changes
    public event Action? OnChange;

    // Method to notify subscribers about state changes
    private void NotifyStateChanged() => OnChange?.Invoke();

    // Simulates a loading delay with a cancellable task
    public async Task SimulateLoadingAsync(int milliseconds = 1000)
    {
        IsLoading = true;
        await Task.Delay(milliseconds);
        IsLoading = false;
    }

    // Initialize the app state with default values
    public void InitializeDefaults()
    {
        // Set default category
        if (string.IsNullOrEmpty(_selectedCategory))
        {
            _selectedCategory = "Category 1";
        }
    }

    // Reset the app state when logging out
    public void Reset()
    {
        IsAuthenticated = false;
        SelectedCategory = string.Empty;
    }
}
