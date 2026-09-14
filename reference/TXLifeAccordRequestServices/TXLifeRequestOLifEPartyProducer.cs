using System;
using System.Xml;

namespace BusinessServices.BAL.ACORD
{
	/// <summary>
	/// Summary description for TXLifeRequestOLifEParty.
	/// </summary>
	public class TXLifeRequestOLifEPartyProducer :Logger, IProcessXML 
	{

		#region PROPERTIES


		private string _partyID	= string.Empty ;
		public string PartyID
		{
			get {return _partyID;}
			set 
			{	
				if (value != "")
					_partyID	= value;
			}
		}
		

		private string _companyProducerID	= string.Empty ;
		/// <summary>
		/// AgentID
		/// </summary>
		public string CompanyProducerID
		{
			get {return _companyProducerID;}
			set 
			{	
				if (value != "")
					_companyProducerID	= value;
			}
		}

		/// <summary>
		/// Agent type, use 1 to search for Agent
		/// </summary>
		public string CarrierApptTypeCode	= string.Empty;


		/// <summary>
		/// Specify a search criteria for carrier type, use 1 for Agent
		/// </summary>
		public string CarrierApptTypeCodeToRetrieve	= string.Empty;
	

	

		public bool ProcessXML(XmlNode node)
		{

			if (_partyID == "" || CarrierApptTypeCodeToRetrieve == "") 
				throw new ApplicationException ("PartyID or Carrier type is blank.");

            if (node == null)
                return false;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(node.OuterXml);

			//XmlNode tmpChildNode;
			string tmpValue			= string.Empty ;
			try
			{
				// get the Party node with the party id
                XmlNode tmpNode = xmldoc.SelectSingleNode("//OLifE/Party[@id='" +  
					this.PartyID + "']/Producer/CarrierAppointment/CarrierApptTypeCode[@tc='" + this.CarrierApptTypeCodeToRetrieve + "']");
				
				

				if (tmpNode != null)
				{
					tmpNode			= tmpNode.ParentNode;
					foreach (XmlNode childNode in tmpNode.ChildNodes)
					{
					
						switch (childNode.Name.ToString().Trim().ToUpper())
						{
							case "COMPANYPRODUCERID":
								this.CompanyProducerID  		= childNode.InnerText ;
								
								break;

							case "CARRIERAPPTYPECODE":
                                this.CarrierApptTypeCode = XMLUtility.GetAttributeValue(childNode, "tc");
								break;

						}	
					}
					
				}

				return true;
			}
			catch(Exception)
			{
				//EventLog.WriteEntry (e.Source , e.Message + "\n" + "Stack = " + e.StackTrace, EventLogEntryType.Error);
				return false;
			}
			
		}
		

		#endregion PROPERTIES

		
		
	}
}
