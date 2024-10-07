namespace OzyParkAdmin.Infrastructure.BarCodes;
internal class BarCodeOptionsBase64Image : BarCodeOptions<byte[]>
{
    public BarCodeOptionsBase64Image()
    {
        Converter = ConvertToBase64Image;
    }

    public required string MimeType { get; set; }

    private string ConvertToBase64Image(byte[] content)
    {
        string base64 = Convert.ToBase64String(content);
        return $"data:{MimeType};base64,{base64}";
    }

    public static BarCodeOptionsBase64Image Create(int width, int height, string mimeType)
    {
        return new BarCodeOptionsBase64Image
        {
            Width = width,
            Height = height,
            MimeType = mimeType
        };
    }
}
