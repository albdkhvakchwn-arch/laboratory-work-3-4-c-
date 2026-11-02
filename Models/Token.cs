using System.Xml.Serialization;

namespace CombinedLab.Models;

[XmlInclude(typeof(Word))]
[XmlInclude(typeof(Punctuation))]
public abstract class Token
{
    protected Token()
    {
    }

    protected Token(string value)
    {
        Value = value;
    }

    [XmlText]
    public string Value { get; set; } = string.Empty;
}
