namespace OzyParkAdmin.Components.Admin.Reportes.Models;

/// <summary>
/// El tamaño que tendrá el layout.
/// </summary>
/// <param name="xs">Tamaño para dispositivos xs.</param>
/// <param name="sm">Tamaño para dispositivos sm.</param>
/// <param name="md">Tamaño para dispositivos md.</param>
/// <param name="lg">Tamaño para dispositivos lg.</param>
/// <param name="xl">Tamaño para dispositivos xl.</param>
/// <param name="xxl">Tamaño para dispositivos xxl.</param>
public readonly record struct SizeLayout(int xs, int sm, int md, int lg, int xl, int xxl);
