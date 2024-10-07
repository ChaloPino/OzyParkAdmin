namespace OzyParkAdmin.Infrastructure.BarCodes;

internal interface IBarCodeService<TOut>
{
    TOut GenerateBarCode(string value, BarCodeOptions<TOut> options);
}