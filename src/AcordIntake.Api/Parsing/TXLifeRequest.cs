using System.Xml;

namespace AcordIntake.Api.Parsing;

public sealed class TXLifeRequest
{
    public string TransRefGUID { get; private set; } = string.Empty;
    public string TransType { get; private set; } = string.Empty;
    public string TransCode { get; private set; } = string.Empty;
    public string TransExeDate { get; private set; } = string.Empty;
    public string TransExeTime { get; private set; } = string.Empty;
    public string TransMode { get; private set; } = string.Empty;
    public string TransModeCode { get; private set; } = string.Empty;
    public string TestIndicator { get; private set; } = string.Empty;
    public string TestIndicatorCode { get; private set; } = string.Empty;

    public bool ProcessXml(XmlNode? requestNode)
    {
        if (requestNode is null)
            return false;

        try
        {
            foreach (XmlNode childNode in requestNode.ChildNodes)
            {
                switch (childNode.LocalName.ToUpperInvariant())
                {
                    case "TRANSTYPE":
                        TransType = childNode.InnerText;
                        TransCode = XmlUtility.GetAttributeValue(childNode, "tc");
                        break;
                    case "TRANSEXEDATE":
                        TransExeDate = childNode.InnerText;
                        break;
                    case "TRANSEXETIME":
                        TransExeTime = childNode.InnerText;
                        break;
                    case "TRANSMODE":
                        TransMode = childNode.InnerText;
                        TransModeCode = XmlUtility.GetAttributeValue(childNode, "tc");
                        break;
                    case "TESTINDICATOR":
                        TestIndicator = childNode.InnerText.Trim();
                        TestIndicatorCode = XmlUtility.GetAttributeValue(childNode, "tc");
                        break;
                    case "TRANSREFGUID":
                        TransRefGUID = childNode.InnerText;
                        break;
                }
            }

            return true;
        }
        catch (XmlException)
        {
            return false;
        }
    }
}