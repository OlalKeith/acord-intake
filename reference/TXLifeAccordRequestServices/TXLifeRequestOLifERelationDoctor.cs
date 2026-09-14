using System;
using System.Xml;

namespace BusinessServices.BAL.ACORD
{
	/// <summary>
	/// Summary description for TXLifeRequestOLifERelation.
	/// </summary>
	public class TXLifeRequestOLifERelationDoctor :Logger, IProcessXML
	{
        public TXLifeRequestOLifERelationDoctor()
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


       

        private XmlNode GetDoctorOrFacility(XmlDocument node)
        {

            // got the relation node
            XmlNodeList relationNodelist = node.SelectNodes("//OLifE/Relation");
            XmlNode tmpNode = null;

            // Physician 
            foreach (XmlNode relationnode in relationNodelist)
            {
                // primary Physician
                tmpNode = relationnode.SelectSingleNode("//RelationRoleCode[@tc='58']");
                if (tmpNode != null)
                    return tmpNode.ParentNode;

                // Physician
                tmpNode = relationnode.SelectSingleNode("//RelationRoleCode[@tc='41']");
                if (tmpNode != null)
                    return tmpNode.ParentNode;

               
            }
            
            
            return null;
        }

		
		public bool ProcessXML(XmlNode node)
		{
			try
			{
                if (node == null)
                    return false;

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(node.OuterXml);

				XmlNode	tmpNode		= null;

                // get the doctor information or primary doctor information
                tmpNode = GetDoctorOrFacility(xmldoc);

                

                //// get the Party node with the party id
                //if (OriginatingObjectID != "")
                //    tmpNode			= node.SelectSingleNode("//OLifE/Relation[@OriginatingObjectID='" +  
                //        this.OriginatingObjectID + "']");
				
                //if (RelatedObjectID != "")
                //    tmpNode			= node.SelectSingleNode("//OLifE/Relation[@RelatedObjectID='" +  
                //        this.RelatedObjectID + "']");

				if (tmpNode != null)
				{
                    // The related ObjectID is the PartyID that the doctor contains
                    this.RelatedObjectID = XMLUtility.GetAttributeValue(tmpNode, "RelatedObjectID");
                    this.OriginatingObjectID = XMLUtility.GetAttributeValue(tmpNode, "OriginatingObjectID");
                    //foreach (XmlNode childnode in tmpNode.ChildNodes)
                    //{
                    //    switch (childnode.Name.ToString().Trim().ToUpper())
                    //    {
						
                    //        case "ORIGINATINGOBJECTTYPE":
                    //            this.OriginatingObjectType = childnode.InnerText;
                    //    //		this.PhoneTypeCode	= CommonUtilities.GetAttributeValue(childnode, "tc");
                    //            break;

                    //        case "RELATEDOBJECTTYPE":
                    //    //		this.AreaCode	 	= childnode.InnerText ;
                    //            break;

                    //        case "RELATIONROLECODE":
                    //    //		this.RelationRoleCode	= childnode.InnerText;
                    //    //		this.RelationRoleCodeTC = CommonUtilities.GetAttributeValue(childnode, "tc") ;
                    //            break;

                    //    }
                    //}
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
