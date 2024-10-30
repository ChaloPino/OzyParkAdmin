using MudBlazor;
using MudBlazor.ThemeManager;
using System.ComponentModel;

namespace OzyParkAdmin.Shared;

/// <summary>
/// El tema de OzyParkAdmin.
/// </summary>
public class ThemeOzyPark : INotifyPropertyChanged
{
    private ThemeManagerTheme _themeManager = new()
    {
        AppBarElevation = 2,
        DrawerElevation = 2,
        DefaultElevation = 2,
        DrawerClipMode = DrawerClipMode.Docked,
        Theme = new()
        {
            PaletteLight = new()
            {
                Primary = "#2e2693ff",
                AppbarBackground = "#027788ff",
                Success = "#0f9015ff",
                Warning = "#f4bd1aff",
                Error = "#e53529ff",
                TextPrimary = "#4f4050ff",
            }
        }
    };
    /// <summary>
    /// Evento que se dispara cuando el <see cref="ThemeManagerTheme"/> cambia.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// El <see cref="ThemeManagerTheme"/>.
    /// </summary>
    public ThemeManagerTheme ThemeManager
    {
        get => _themeManager;
        set
        {
            _themeManager = value;
            OnPropertyChanged(nameof(ThemeManager));
        }
    }

    private void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
