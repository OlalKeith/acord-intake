using System;
using System.Xml;

namespace BusinessServices.BAL.ACORD
{
	public class TXLifeTXLifeRequestOLifEPartyPerson :Logger, IProcessXML
	{

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public TXLifeTXLifeRequestOLifEPartyPerson() { }

		public string PartyID			= string.Empty ;
    
		/// <remarks/>
		public string FirstName			= string.Empty ;
		
		/// <remarks/>
		public string LastName			= string.Empty;

		/// <remarks/>
		public string MiddleName		= string.Empty;
    
		/// <remarks/>
		public string Occupation		= string.Empty;
    
		/// <remarks/>
		public string BirthDate			= string.Empty;
    
		/// <remarks/>
		public string Age				= string.Empty;
    
		/// <remarks/>
		public string DriversLicenseNum	= string.Empty;
    
		
		/// <remarks/>
		public string MarritalStatus	= string.Empty;

		/// <remarks/>
		public string Gender			= string.Empty;

		/// <remarks/>
		public string Citizenship			= string.Empty;


		public string FullName { get; set; } = "";

        private string _carriercode = string.Empty;
        public string CarrierCode
        {
            get { return _carriercode; }
            set
            {
                _carriercode = value;
            }
        }


		public string CompanyProducerId = string.Empty;

		public bool ProcessXML(XmlNode node)
		{
            if (node == null)
                return false;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(node.OuterXml);

			if (this.PartyID == "") 
				throw new ApplicationException ("PartyID is blank.");
			try
			{
			
                var carriernode = xmldoc.SelectSingleNode("//OLifE/Party[@id='" + this.PartyID + "']/Producer/CarrierAppointment/CarrierCode");
                if (carriernode != null)
                    this.CarrierCode = carriernode.InnerText;

				var companyproducernode = xmldoc.SelectSingleNode("//OLifE/Party[@id='" + this.PartyID + "']/Producer/CarrierAppointment/CompanyProducerID");
				if (companyproducernode != null)
					this.CompanyProducerId = companyproducernode.InnerText;


				// get the Person node with the party id
				XmlNode tmpNode = xmldoc.SelectSingleNode("//OLifE/Party[@id='" + this.PartyID + "']/Person");

				

                if (tmpNode != null)
				{
					foreach (XmlNode childnode in tmpNode.ChildNodes)
					{
						switch (childnode.Name.ToString().Trim().ToUpper())
						{
							case "FIRSTNAME":
								this.FirstName		= childnode.InnerText;
								break;

							case "FULLNAME":
								this.FullName = childnode.InnerText;
								break;

							case "MIDDLENAME":
								this.MiddleName 	= childnode.InnerText ;
								break;

							case "LASTNAME":
								this.LastName	 	= childnode.InnerText ;
								break;

							case "OCCUPATION":
								this.Occupation  	= childnode.InnerText ;
								break;

							case "MARSTAT":
								this.MarritalStatus = childnode.InnerText ;
								break;

							

							case "GENDER":
								this.Gender		 	= childnode.InnerText ;
								break;

							case "BIRTHDATE":
								this.BirthDate	 	= childnode.InnerText ;
								break;

							case "CITIZENSHIP":
								this.Citizenship  	= childnode.InnerText ;
								break;

							case "AGE":
								this.Age		  	= childnode.InnerText ;
								break;

                          
                        }
					}
					return true;
				}
                

				// get full name
				XmlNode notPersontmpnode = xmldoc.SelectSingleNode("//OLifE/Party[@id='" + this.PartyID + "']/FullName");
				if (notPersontmpnode != null)
                {
					this.FullName = notPersontmpnode.InnerText;
					return true;
				}
					
				return false;
			}
			catch(Exception e)
			{

                logger.Error("TXLifeTXLifeRequestOLifEPartyPerson",e);
				//EventLog.WriteEntry (e.Source , e.Message + "\n" + "Stack = " + e.StackTrace, EventLogEntryType.Error);
				return false;
			}
}
		
	}


	
}
