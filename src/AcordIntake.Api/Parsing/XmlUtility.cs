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

        RemoveNamespaces(root, document);
        return document.OuterXml;
    }

    public static string GetAttributeValue(XmlNode node, string attributeName)
    {
        return node.Attributes?[attributeName]?.Value ?? string.Empty;
    }

    private static void RemoveNamespaces(XmlNode node, XmlDocument document)
    {
        if (node is XmlElement element)
        {
            document.RenameNode(element, null, element.LocalName);

            for (var index = element.Attributes.Count - 1; index >= 0; index--)
            {
                var attribute = element.Attributes[index];
                if (attribute.Prefix == "xmlns" || attribute.Name == "xmlns")
                    element.RemoveAttributeNode(attribute);
                else if (attribute.NamespaceURI.Length > 0)
                    document.RenameNode(attribute, null, attribute.LocalName);
            }
        }

        foreach (XmlNode child in node.ChildNodes)
            RemoveNamespaces(child, document);
    }
}