using System.Text;
using System.Xml;
using System.Xml.Serialization;
using CombinedLab.Models;

namespace CombinedLab.Services;

public static class TextXmlExporter
{
    public static void Export(Text text, string path)
    {
        var serializer = new XmlSerializer(typeof(Text));
        var settings = new XmlWriterSettings
        {
            Indent = true,
            Encoding = new UTF8Encoding(false)
        };

        using var writer = XmlWriter.Create(path, settings);
        serializer.Serialize(writer, text);
    }
}
