using System.Xml.Serialization;

namespace CombinedLab.Models;

public sealed class Word : Token
{
    public Word()
    {
    }

    public Word(string value, int lineNumber) : base(value)
    {
        LineNumber = lineNumber;
    }

    [XmlAttribute("line")]
    public int LineNumber { get; set; }
}
