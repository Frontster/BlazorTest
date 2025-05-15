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
    private readonly SemaphoreSlim _authLock = new SemaphoreSlim(1, 1);
    private const string AUTH_TOKEN_KEY = "auth_token";
    private const string USER_NAME_KEY = "user_name";
    private const string LAST_LOGIN_KEY = "last_login";

    public AuthService(ILocalStorageService localStorage, AppStateService appStateService)
    {
        _localStorage = localStorage;
        _appStateService = appStateService;
        Console.WriteLine($"AuthService: Constructor called at {DateTime.Now:HH:mm:ss.fff}");
    }

    /// <summary>
    /// Simulates a login process with a delay to mimic a real API call
    /// </summary>
    /// <param name="username">The username to login with</param>
    /// <param name="password">The password to login with</param>
    /// <returns>True if login was successful, false otherwise</returns>
    public async Task<bool> LoginAsync(string username, string password)
    {
        // Use a lock to prevent concurrent login attempts
        await _authLock.WaitAsync();

        try
        {
            Console.WriteLine($"AuthService: Attempting login for user: {username} at {DateTime.Now:HH:mm:ss.fff}");

            // Check if already logged in
            var existingToken = await _localStorage.GetItemAsync<string>(AUTH_TOKEN_KEY);
            if (!string.IsNullOrEmpty(existingToken))
            {
                Console.WriteLine($"AuthService: User already has auth token, clearing before new login at {DateTime.Now:HH:mm:ss.fff}");
                await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
                await _localStorage.RemoveItemAsync(USER_NAME_KEY);
                await _localStorage.RemoveItemAsync(LAST_LOGIN_KEY);
            }

            // Simulate API call delay
            await Task.Delay(1500); // 1.5 second delay to simulate network request

            // For demo purposes, accept any non-empty credentials
            bool isSuccess = !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password);
            Console.WriteLine($"AuthService: Login success: {isSuccess} at {DateTime.Now:HH:mm:ss.fff}");

            if (isSuccess)
            {
                // Generate a fake token and store it
                var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                await _localStorage.SetItemAsync(AUTH_TOKEN_KEY, token);
                await _localStorage.SetItemAsync(USER_NAME_KEY, username);
                await _localStorage.SetItemAsync(LAST_LOGIN_KEY, DateTime.Now.ToString("o"));
                Console.WriteLine($"AuthService: Token and username stored in local storage at {DateTime.Now:HH:mm:ss.fff}");

                // Update authenticated state - ensure this happens before InitializeDefaults
                _appStateService.IsAuthenticated = true;
                
                // Initialize default values and ensure state changes are propagated
                _appStateService.InitializeDefaults();
                
                // Force an additional state notification to ensure UI components update
                _appStateService.NotifyStateChanged();
            }

            return isSuccess;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"AuthService: Error during login: {ex.Message} at {DateTime.Now:HH:mm:ss.fff}");
            return false;
        }
        finally
        {
            _authLock.Release();
        }
    }

    /// <summary>
    /// Logs the user out by clearing stored auth data
    /// </summary>
    public async Task LogoutAsync()
    {
        // Use a lock to prevent concurrent logout operations
        await _authLock.WaitAsync();

        try
        {
            Console.WriteLine($"AuthService: Logging out user at {DateTime.Now:HH:mm:ss.fff}");
            await Task.Delay(500); // Short delay to show loading state

            // Clear stored auth data
            await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
            await _localStorage.RemoveItemAsync(USER_NAME_KEY);
            await _localStorage.RemoveItemAsync(LAST_LOGIN_KEY);
            Console.WriteLine($"AuthService: Token and username removed from local storage at {DateTime.Now:HH:mm:ss.fff}");

            // Reset app state
            _appStateService.Reset();
            Console.WriteLine($"AuthService: Logout complete at {DateTime.Now:HH:mm:ss.fff}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"AuthService: Error during logout: {ex.Message} at {DateTime.Now:HH:mm:ss.fff}");
        }
        finally
        {
            _authLock.Release();
        }
    }

    /// <summary>
    /// Checks if the user is currently authenticated
    /// </summary>
    /// <returns>True if authenticated, false otherwise</returns>
    public async Task<bool> IsAuthenticatedAsync()
    {
        Console.WriteLine($"AuthService: Checking authentication status at {DateTime.Now:HH:mm:ss.fff}");

        try
        {
            var token = await _localStorage.GetItemAsync<string>(AUTH_TOKEN_KEY);
            var lastLoginStr = await _localStorage.GetItemAsync<string>(LAST_LOGIN_KEY);

            var isAuthenticated = !string.IsNullOrEmpty(token);
            Console.WriteLine($"AuthService: IsAuthenticated: {isAuthenticated} at {DateTime.Now:HH:mm:ss.fff}");

            // Additional validation for token age if needed
            if (isAuthenticated && !string.IsNullOrEmpty(lastLoginStr))
            {
                if (DateTime.TryParse(lastLoginStr, out var lastLogin))
                {
                    var tokenAge = DateTime.Now - lastLogin;
                    Console.WriteLine($"AuthService: Token age: {tokenAge.TotalMinutes:F1} minutes");

                    // Token expiration could be checked here
                    // if (tokenAge.TotalHours > 24) {...}
                }
            }

            // Update AppState without triggering events if state is already correct
            if (_appStateService.IsAuthenticated != isAuthenticated)
            {
                _appStateService.IsAuthenticated = isAuthenticated;
            }

            if (isAuthenticated)
            {
                Console.WriteLine($"AuthService: User is authenticated, initializing defaults at {DateTime.Now:HH:mm:ss.fff}");
                _appStateService.InitializeDefaults();
            }

            return isAuthenticated;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"AuthService: Error checking authentication: {ex.Message} at {DateTime.Now:HH:mm:ss.fff}");
            return false;
        }
    }

    /// <summary>
    /// Gets the current user's name if authenticated
    /// </summary>
    /// <returns>The username or empty string if not authenticated</returns>
    public async Task<string> GetUserNameAsync()
    {
        try
        {
            var username = await _localStorage.GetItemAsync<string>(USER_NAME_KEY) ?? string.Empty;
            Console.WriteLine($"AuthService: Retrieved username: {username} at {DateTime.Now:HH:mm:ss.fff}");
            return username;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"AuthService: Error retrieving username: {ex.Message} at {DateTime.Now:HH:mm:ss.fff}");
            return string.Empty;
        }
    }
}