using Blazored.LocalStorage;
using OzyParkAdmin.Infrastructure.Layout;

namespace OzyParkAdmin.Shared;

internal class UserPreferencesService : IUserPreferencesService
{
    private const string Key = "userPreferences";
    private readonly ILocalStorageService _localStorage;
    public UserPreferencesService(ILocalStorageService localStorage)
    {
        ArgumentNullException.ThrowIfNull(localStorage);
        _localStorage = localStorage;
    }

    public async Task<UserPreferences?> LoadUserPreferences()
    {
        return await _localStorage.GetItemAsync<UserPreferences>(Key);
    }

    public async Task SaveUserPreferences(UserPreferences userPreferences)
    {
        await _localStorage.SetItemAsync(Key, userPreferences);
    }
}
