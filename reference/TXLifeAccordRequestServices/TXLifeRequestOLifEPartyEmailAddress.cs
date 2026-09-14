using System;
using System.Xml;


namespace BusinessServices.BAL.ACORD
{
	public class TXLifeTXLifeRequestOLifEPartyEmailAddress :Logger, IProcessXML
	{
        public TXLifeTXLifeRequestOLifEPartyEmailAddress () { }

		/// <remarks/>
		public string EMailType 				= string.Empty ;


		/// <remarks/>
		public string Email			= string.Empty ;


		/// <summary>
		/// PartyID is required to retrieve
		/// </summary>
		public string PartyID				= string.Empty;

		public bool PreferredPrimaryAddress	= true;


        public bool ProcessXML(XmlNode node)
		{
			if (this.PartyID == "" ) 
				throw new ApplicationException ("PartyID is blank.");

			try
			{
                // application information
                if (node == null)
                    return false;

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(node.OuterXml);


				XmlNode tmpNode;
				if (PreferredPrimaryAddress == true)
                    tmpNode = xmldoc.SelectSingleNode("//OLifE/Party[@id='" +  
						this.PartyID + "']/EMailAddress/PrefEMailAddr[@tc='1']");

				else
                {
					tmpNode = xmldoc.SelectSingleNode("//OLifE/Party[@id='" +
						this.PartyID + "']/EMailAddress/AddrLine");
				
					if (tmpNode == null)
						tmpNode = xmldoc.SelectSingleNode("//OLifE/Party[@id='" +
						this.PartyID + "']/EMailAddress/PrefEMailAddr");

				}



				if (tmpNode != null)
				{
					// we found the phone type we want, but we need to go back to parent node to retrieve it
					tmpNode			= tmpNode.ParentNode;
					foreach (XmlNode childnode in tmpNode.ChildNodes)
					{
						switch (childnode.Name.ToString().Trim().ToUpper())
						{
							case "EMAILTYPE":
								this.EMailType  		= childnode.InnerText ;
							
								break;

							case "ADDRLINE":
								this.Email			 	= childnode.InnerText ;
								break;

							case "PREFEMAILADDR":   // protective life sends us the email in the PREFEMAILADDR node. 05/05/2021
								this.Email = childnode.InnerText;
								if (!string.IsNullOrEmpty(this.Email))
									if (this.Email.Trim().ToUpper() == "TRUE")
                                    {
										this.Email = "";
									}
										
								break;

						}
					}
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
