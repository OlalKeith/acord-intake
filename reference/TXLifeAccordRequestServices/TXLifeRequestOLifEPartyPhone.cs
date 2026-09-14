using System;
using System.Xml;
using System.Diagnostics;


namespace BusinessServices.BAL.ACORD
{
	public class TXLifeTXLifeRequestOLifEPartyPhone :Logger, IProcessXML
	{
    
		/// <remarks/>
		public string PhoneType				= string.Empty ;


		/// <remarks/>
		public string PhoneTypeCode			= string.Empty ;

		
		/// <remarks/>
		public string AreaCode				= string.Empty;

		/// <remarks/>
		public string DialNumber			= string.Empty;

		
		/// <remarks/>
		public string PrefixPhone			= string.Empty;

		/// <summary>
		/// PartyID is required to retrieve
		/// </summary>
		public string PartyID				= string.Empty;

        public string PhoneExt = string.Empty;

		/// <summary>
		/// Default is 0 or false
		/// </summary>
		public string PhoneTypeToRetrieve	= "0";	// default value is set to 0

	
		public bool ProcessXML(XmlNode  node)
		{
            if (this.PartyID == "")
                return false;

            if (node == null)
                return false;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(node.OuterXml);

			try
			{
				XmlNode tmpNode;
				if (this.PhoneTypeToRetrieve != "")
                {
					tmpNode = xmldoc.SelectSingleNode("//OLifE/Party[@id='" +
												this.PartyID + "']/Phone/PhoneTypeCode[@tc='" + this.PhoneTypeToRetrieve + "']");

					// we found the phone type we want, but we need to go back to parent node to retrieve it
					if (tmpNode != null)
						tmpNode = tmpNode.ParentNode;
					
					

				}
				else  // just get any phone number then
                    tmpNode = xmldoc.SelectSingleNode("//OLifE/Party[@id='" +  
						this.PartyID + "']/Phone");

				
				if (tmpNode != null)
				{
					foreach (XmlNode childnode in tmpNode.ChildNodes)
					{
						switch (childnode.Name.ToString().Trim().ToUpper())
						{
							case "PHONETYPECODE":
								this.PhoneType 		= childnode.InnerText ;
                                this.PhoneTypeCode = XMLUtility.GetAttributeValue(childnode, "tc");
								break;

							case "AREACODE":
								this.AreaCode	 	= childnode.InnerText ;
								break;

							case "DIALNUMBER":
								this.DialNumber	 	= childnode.InnerText ;
								break;

							case "PREFPHONE":
                                this.PrefixPhone = XMLUtility.GetAttributeValue(childnode, "tc");
								break;
                                
							case "EXT":
                                this.PhoneExt = childnode.InnerText.Trim();
								break;
                                
						}
					}
					return true;
				}
				return false;
			}
			catch(Exception)
			{
				return false;
			}
		}
		
	}


	
}
