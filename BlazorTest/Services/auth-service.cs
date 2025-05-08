using Blazored.LocalStorage;

namespace BlazorTest.Services;

/// <summary>
/// This service handles authentication-related operations including:
/// - User login and logout
/// - Checking authentication status
/// - Storing/retrieving the auth token from local storage
/// </summary>
public class AuthService
{
    private readonly ILocalStorageService _localStorage;
    private readonly AppStateService _appStateService;
    private const string AUTH_TOKEN_KEY = "auth_token";
    private const string USER_NAME_KEY = "user_name";

    public AuthService(ILocalStorageService localStorage, AppStateService appStateService)
    {
        _localStorage = localStorage;
        _appStateService = appStateService;
        Console.WriteLine("AuthService: Constructor called");
    }

    /// <summary>
    /// Simulates a login process with a delay to mimic a real API call
    /// </summary>
    /// <param name="username">The username to login with</param>
    /// <param name="password">The password to login with</param>
    /// <returns>True if login was successful, false otherwise</returns>
    public async Task<bool> LoginAsync(string username, string password)
    {
        Console.WriteLine($"AuthService: Attempting login for user: {username}");

        // Simulate API call delay
        _appStateService.IsLoading = true;
        await Task.Delay(1500); // 1.5 second delay to simulate network request

        // For demo purposes, accept any non-empty credentials
        bool isSuccess = !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password);
        Console.WriteLine($"AuthService: Login success: {isSuccess}");

        if (isSuccess)
        {
            // Generate a fake token and store it
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            await _localStorage.SetItemAsync(AUTH_TOKEN_KEY, token);
            await _localStorage.SetItemAsync(USER_NAME_KEY, username);
            Console.WriteLine("AuthService: Token and username stored in local storage");

            // Update authenticated state
            _appStateService.IsAuthenticated = true;
            _appStateService.InitializeDefaults();
        }

        _appStateService.IsLoading = false;
        return isSuccess;
    }

    /// <summary>
    /// Logs the user out by clearing stored auth data
    /// </summary>
    public async Task LogoutAsync()
    {
        Console.WriteLine("AuthService: Logging out user");
        _appStateService.IsLoading = true;
        await Task.Delay(500); // Short delay to show loading state

        // Clear stored auth data
        await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
        await _localStorage.RemoveItemAsync(USER_NAME_KEY);
        Console.WriteLine("AuthService: Token and username removed from local storage");

        // Reset app state
        _appStateService.Reset();
        _appStateService.IsLoading = false;
        Console.WriteLine("AuthService: Logout complete");
    }

    /// <summary>
    /// Checks if the user is currently authenticated
    /// </summary>
    /// <returns>True if authenticated, false otherwise</returns>
    public async Task<bool> IsAuthenticatedAsync()
    {
        Console.WriteLine("AuthService: Checking authentication status");
        var token = await _localStorage.GetItemAsync<string>(AUTH_TOKEN_KEY);
        var isAuthenticated = !string.IsNullOrEmpty(token);
        Console.WriteLine($"AuthService: IsAuthenticated: {isAuthenticated}");

        _appStateService.IsAuthenticated = isAuthenticated;

        if (isAuthenticated)
        {
            Console.WriteLine("AuthService: User is authenticated, initializing defaults");
            _appStateService.InitializeDefaults();
        }

        return isAuthenticated;
    }

    /// <summary>
    /// Gets the current user's name if authenticated
    /// </summary>
    /// <returns>The username or empty string if not authenticated</returns>
    public async Task<string> GetUserNameAsync()
    {
        var username = await _localStorage.GetItemAsync<string>(USER_NAME_KEY) ?? string.Empty;
        Console.WriteLine($"AuthService: Retrieved username: {username}");
        return username;
    }
}