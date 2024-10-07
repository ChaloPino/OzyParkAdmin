namespace OzyParkAdmin.Infrastructure.BarCodes;
internal static class BarCodeProvider
{
    private readonly static Dictionary<Type, object> _registeredBarCodes = [];

    static BarCodeProvider() => RegisterBarCode(new QRService());

    public static void RegisterBarCode<T>(IBarCodeService<T> barCodeService) =>
        _registeredBarCodes[typeof(T)] = barCodeService;

    public static IBarCodeService<T> GetBarCode<T>() =>
        (IBarCodeService<T>)_registeredBarCodes[typeof(T)];
}
