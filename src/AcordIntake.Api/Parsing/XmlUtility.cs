using System.Xml;

namespace AcordIntake.Api.Parsing;

public sealed class XmlUtility
{
    public string RemoveNamespacesFromXml(string xml)
    {
        var document = new XmlDocument();
        document.LoadXml(xml);

        var root = document.DocumentElement
            ?? throw new XmlException("The XML document has no root element.");
        var namespaceFreeDocument = new XmlDocument();
        namespaceFreeDocument.AppendChild(CloneWithoutNamespaces(root, namespaceFreeDocument));
        return namespaceFreeDocument.OuterXml;
    }

    public static string GetAttributeValue(XmlNode node, string attributeName)
    {
        return node.Attributes?[attributeName]?.Value ?? string.Empty;
    }

    private static XmlElement CloneWithoutNamespaces(XmlElement source, XmlDocument document)
    {
        var element = document.CreateElement(source.LocalName);
        foreach (XmlAttribute attribute in source.Attributes)
        {
            if (attribute.Prefix != "xmlns" && attribute.Name != "xmlns")
                element.SetAttribute(attribute.LocalName, attribute.Value);
        }

        foreach (XmlNode child in source.ChildNodes)
        {
            if (child is XmlElement childElement)
                element.AppendChild(CloneWithoutNamespaces(childElement, document));
            else
                element.AppendChild(document.ImportNode(child, true));
        }

        return element;
    }
}