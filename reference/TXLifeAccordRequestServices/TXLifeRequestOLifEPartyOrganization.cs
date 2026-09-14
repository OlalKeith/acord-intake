using System;
using System.Xml;

namespace BusinessServices.BAL.ACORD
{
    /// <summary>
    /// Return an organization/company/agency.  This is the requestor company name, it must match what's in our system or it will not be processed.
    /// </summary>
    public class TXLifeRequestOLifEPartyOrganization :Logger, IProcessXML
    {
        
        public TXLifeRequestOLifEPartyOrganization() { }
        private string _abbrname = string.Empty;
        public string AbbrName
        {
            get { return _abbrname; }
            set { _abbrname = value.Trim(); }
        }

        private string _orgCode = string.Empty;
        public string OrgCode
        {
            get { return _orgCode; }
            set { _orgCode = value.Trim(); }
        }

        private string _partyID = string.Empty;
        public string PartyID
        {
            get { return _partyID; }
            set { _partyID = value.Trim(); }
        }


        

        public bool ProcessXML(XmlNode node)
        {
            XmlNode tmpNode = null;
            // application information
            if (node == null)
                return false;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(node.OuterXml);

            if (string.IsNullOrEmpty(this.PartyID) == true)
            {
                tmpNode = xmldoc.SelectSingleNode("//OLifE/Relation/RelationRoleCode[@tc='48']");
                this.PartyID = XMLUtility.GetAttributeValue(tmpNode.ParentNode, "RelatedObjectID");
            }

            if (string.IsNullOrEmpty(this.PartyID))
            {
                tmpNode = null;
                return false;
            }

            try
            {

                // get the Person node with the party id
                tmpNode = xmldoc.SelectSingleNode("//OLifE/Party[@id='" + this.PartyID + "']/Organization");

                if (tmpNode != null)
                {
                    foreach (XmlNode childnode in tmpNode.ChildNodes)
                    {
                        switch (childnode.Name.ToString().Trim().ToUpper())
                        {
                            case "ABBRNAME":
                                this.AbbrName = childnode.InnerText;
                                break;

                            case "ORGCODE":
                                this.OrgCode = childnode.InnerText;
                                break;
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                
                return false;
            }
            finally
            {
                tmpNode = null;
            }
        }
    }
}
