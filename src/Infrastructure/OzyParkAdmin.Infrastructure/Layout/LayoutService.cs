using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.Layout;

/// <summary>
/// Servicio que administra lo conserniente al diseño visual.
/// </summary>
public class LayoutService : IInfrastructure
{
    private readonly IUserPreferencesService _userPreferencesService;
    private UserPreferences? _userPreferences;
    private bool _systemPreferences;

    /// <summary>
    /// Crea una nueva instancia de <see cref="LayoutService"/>.
    /// </summary>
    /// <param name="userPreferencesService">El <see cref="IUserPreferencesService"/>.</param>
    public LayoutService(IUserPreferencesService userPreferencesService)
    {
        ArgumentNullException.ThrowIfNull(userPreferencesService);
        _userPreferencesService = userPreferencesService;
    }

    /// <summary>
    /// Evento que se dispara cuando ocurre una actualización mayor.
    /// </summary>
    public event EventHandler? MajorUpdateOccurred;

    /// <summary>
    /// El actual <see cref="DarkLightMode"/>.
    /// </summary>
    public DarkLightMode CurrentDarkLigntMode { get; private set; } = DarkLightMode.System;

    /// <summary>
    /// Si es modo oscuro.
    /// </summary>
    public bool IsDarkMode { get; private set; }

    /// <summary>
    /// Si el modo actual es obscuro.
    /// </summary>
    public bool CurrentModeIsDark => CurrentDarkLigntMode == DarkLightMode.Dark;

    /// <summary>
    /// Si el modo actual es claro.
    /// </summary>
    public bool CurrentModeIsLight => CurrentDarkLigntMode == DarkLightMode.Light;

    /// <summary>
    /// Si el modo actual es automático.
    /// </summary>
    public bool CurrentModeIsSystem => CurrentDarkLigntMode == DarkLightMode.System;

    /// <summary>
    /// Establece el modo oscuro.
    /// </summary>
    /// <param name="value">El valor que indica si se establece el modo oscuro.</param>
    public void SetDarkMode(bool value) =>
        IsDarkMode = value;

    /// <summary>
    /// Aplica las preferencias del usuario.
    /// </summary>
    /// <param name="isDarkModeDefaultTheme">Indica si el modo oscuro es el tema predeterminado.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardar.</returns>
    public async Task ApplyUserPreferences(bool isDarkModeDefaultTheme)
    {
        _systemPreferences = isDarkModeDefaultTheme;

        _userPreferences = await _userPreferencesService.LoadUserPreferences();

        if (_userPreferences is not null)
        {
            CurrentDarkLigntMode = _userPreferences.DarkLightMode;

            IsDarkMode = CurrentDarkLigntMode switch
            {
                DarkLightMode.Dark => true,
                DarkLightMode.Light => false,
                DarkLightMode.System => isDarkModeDefaultTheme,
                _ => IsDarkMode,
            };
        }
        else
        {
            IsDarkMode = isDarkModeDefaultTheme;
            _userPreferences = new UserPreferences { DarkLightMode = DarkLightMode.System };
            await _userPreferencesService.SaveUserPreferences(_userPreferences);
        }
    }

    /// <summary>
    /// Dispara los cambios mayores.
    /// </summary>
    /// <param name="newValue">Define que los cambios son por sistema.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    public Task OnSystemPreferenceChanged(bool newValue)
    {
        _systemPreferences = newValue;

        if (CurrentDarkLigntMode == DarkLightMode.System)
        {
            IsDarkMode = newValue;
            OnMajorUpdateOccurred();
        }

        return Task.CompletedTask;
    }

    private void OnMajorUpdateOccurred() =>
        MajorUpdateOccurred?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// Cambia el modo del tema.
    /// </summary>
    /// <param name="mode">El modo al cual cambiar.</param>
    /// <returns>Una tarea asyncrona.</returns>
    public async Task ChangeDarkLightModel(DarkLightMode mode)
    {
        CurrentDarkLigntMode = mode;
        switch (mode)
        {
            case DarkLightMode.Light:
                IsDarkMode = false;
                break;
            case DarkLightMode.Dark:
                IsDarkMode = true;
                break;
            case DarkLightMode.System:
                IsDarkMode = _systemPreferences;
                break;
        }

        if (_userPreferences is not null)
        {
            _userPreferences.DarkLightMode = CurrentDarkLigntMode;
            await _userPreferencesService.SaveUserPreferences(_userPreferences);
        }

        OnMajorUpdateOccurred();
    }

    /// <summary>
    /// Cambia el modo del tema en un ciclo.
    /// <para>De System a Light</para>
    /// <para>De Light a Dark</para>
    /// <para>De Dark a System.</para>
    /// </summary>
    /// <returns>Una tarea asyncrona.</returns>
    public async Task CycleDarkLightModeAsync()
    {
        switch (CurrentDarkLigntMode)
        {
            case DarkLightMode.System:
                CurrentDarkLigntMode = DarkLightMode.Light;
                IsDarkMode = false;
                break;
            case DarkLightMode.Light:
                CurrentDarkLigntMode = DarkLightMode.Dark;
                IsDarkMode = true;
                break;
            case DarkLightMode.Dark:
                CurrentDarkLigntMode = DarkLightMode.System;
                IsDarkMode = _systemPreferences;
                break;
        }

        if (_userPreferences is not null)
        {
            _userPreferences.DarkLightMode = CurrentDarkLigntMode;
            await _userPreferencesService.SaveUserPreferences(_userPreferences);
        }

        OnMajorUpdateOccurred();
    }
}
