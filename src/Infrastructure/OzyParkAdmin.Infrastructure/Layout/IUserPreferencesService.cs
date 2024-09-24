namespace OzyParkAdmin.Infrastructure.Layout;

/// <summary>
/// Administra las preferencias del usuario.
/// </summary>
public interface IUserPreferencesService
{
    /// <summary>
    /// Saves UserPreferences in local storage.
    /// </summary>
    /// <param name="userPreferences">The userPreferences to save in hte local storage.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task SaveUserPreferences(UserPreferences userPreferences);

    /// <summary>
    /// Loads UserPreferences from local storage.
    /// </summary>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the UsePreferences object.
    /// <c>null</c> when no settings were found.</returns>
    Task<UserPreferences?> LoadUserPreferences();
}
