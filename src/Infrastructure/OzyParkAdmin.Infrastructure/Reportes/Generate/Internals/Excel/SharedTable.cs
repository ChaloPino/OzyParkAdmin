using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel;
internal sealed class SharedTable
{
    public SharedTable()
    {
        Items = new Dictionary<string, int>();
    }

    public int Count { get; private set; }
    public int UniqueCount { get; set; }
    public IDictionary<string, int> Items { get; }

    public int this[string value] => Items[value];

    internal int AddString(string value)
    {
        if (!Items.TryGetValue(value, out int index))
        {
            Items.Add(value, UniqueCount);
            index = UniqueCount;
            UniqueCount++;
        }
        Count++;
        return index;
    }

    internal void Write(WorkbookPart workbookPart)
    {
        var sharedStringTablePart = workbookPart.AddNewPart<SharedStringTablePart>();

        using OpenXmlWriter writer = OpenXmlWriter.Create(sharedStringTablePart);
        List<OpenXmlAttribute> oxa =
        [
                new OpenXmlAttribute("count", null!, Count.ToString()),
                new OpenXmlAttribute("uniqueCount", null!, UniqueCount.ToString())
        ];

        writer.WriteStartElement(new SharedStringTable(), oxa);

        foreach (var item in Items)
        {
            writer.WriteStartElement(new SharedStringItem());
            writer.WriteElement(new Text(item.Key));
            writer.WriteEndElement();
        }

        writer.WriteEndElement();
    }
}
