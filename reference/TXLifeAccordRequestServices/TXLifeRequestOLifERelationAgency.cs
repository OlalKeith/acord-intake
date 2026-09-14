using System;
using System.Xml;

namespace BusinessServices.BAL.ACORD
{
	/// <summary>
	/// Summary description for TXLifeRequestOLifERelation.
	/// </summary>
	public class TXLifeRequestOLifERelationAgency : Logger, IProcessXML
	{
        public TXLifeRequestOLifERelationAgency()
		{
		}
			/// <remarks/>
		public string RelatedObjectID			= string.Empty ;


		/// <remarks/>
		public string OriginatingObjectID		= string.Empty ;

		
		/// <remarks/>
        
        public string OriginatingObjectType = string.Empty;
        
		/// <remarks/>
		
		private string _relationRoleCode	= string.Empty;
		public string RelationRoleCode
		{
			get {return _relationRoleCode;}
			set 
			{
				if (value.Length > 10)
					_relationRoleCode	= value.Substring(0, 10);
				else
					_relationRoleCode	= value;
			}
		}		



		private string _relationRoleCodeTC	= string.Empty;
		public string RelationRoleCodeTC
		{
			get {return _relationRoleCodeTC;}
			set 
			{
				if (value.Length > 10)
					_relationRoleCodeTC	= value.Substring(0, 10);
				else
					_relationRoleCodeTC	= value;
			}
		}


        private XmlNode GetWritingAgencyRelationNode(XmlDocument node)
        {

            // got the relation node
            XmlNodeList relationNodelist = node.SelectNodes("//OLifE/Relation");
            XmlNode tmpNode = null;
            foreach (XmlNode relationnode in relationNodelist)
            {
                tmpNode = relationnode.SelectSingleNode("//RelationRoleCode[@tc='121']");
                if (tmpNode != null)
                    return tmpNode.ParentNode;
            }
            
            return null;
        }

		
		public bool ProcessXML(XmlNode node)
		{
			try
			{
				
				XmlNode	tmpNode		= null;
                if (node == null)
                    return false;

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(node.OuterXml);

                // get the doctor information or primary doctor information
                tmpNode = GetWritingAgencyRelationNode(xmldoc);

				if (tmpNode != null)
				{
                    this.RelatedObjectID = XMLUtility.GetAttributeValue(tmpNode, "RelatedObjectID");
                    this.OriginatingObjectID = XMLUtility.GetAttributeValue(tmpNode, "OriginatingObjectID");
                    return true;
				}
				return false;
			}
			catch(Exception)
			{
				//EventLog.WriteEntry (e.Source , e.Message + "\n" + "Stack = " + e.StackTrace, EventLogEntryType.Error);
				return false;
			}
		}
	}
}
