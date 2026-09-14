using System;
using System.Xml;


namespace BusinessServices.BAL.ACORD
{
	/// <summary>
	/// Summary description for TXLifeRequestOLifEParty.
	/// </summary>
	public class TXLifeRequestOLifEParty :Logger, IProcessXML 
	{

		#region PROPERTIES

		
	
		private bool _isApplicant	= false;
		public bool IsApplicant
		{
			get {return _isApplicant;}
			set 
			{	
				_isApplicant	= value;
			}
		}



		private bool _isAgent		= false;
		public bool IsAgent
		{
			get {return _isAgent;}
			set 
			{	
				_isAgent	= value;
			}
		}





		private string _partyTypeCode	= string.Empty ;
		public string PartyTypeCode
		{
			get {return _partyTypeCode;}
			set 
			{	
				if (value != "")
					_partyTypeCode	= value;
			}
		}




		private string _partyType	= string.Empty ;
		public string PartyType
		{
			get {return _partyType;}
			set 
			{	
				if (value != "")
					_partyType	= value;
			}
		}


		private string _fullName	= string.Empty ;
		public string FullName
		{
			get {return _fullName;}
			set 
			{	
				if (value != "")
					_fullName	= value;
			}
		}



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



		private string _govtID	= string.Empty ;
		public string GovtID
		{
			get {return _govtID;}
			set 
			{	
				if (value != "")
					_govtID	= value;
			}
		}

        private string _IDReferenceNo = string.Empty;
        public string IDReferenceNo
        { 
            get { return _IDReferenceNo; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _IDReferenceNo = value;

            }
        
        }

        private string _companyProducerID = string.Empty;
        public string CompanyProducerID
        {
            get { return _companyProducerID; }
            set
            {
                _companyProducerID = value;
            }
        }

        private string _agencycarriercode = string.Empty;
        public string AgencyCarrierCode
        {
            get { return _agencycarriercode; }
            set
            {
                _agencycarriercode = value;
            }
        }

        /// <summary>
        /// Full organization Name
        /// </summary>
        public string FullOrgName	= string.Empty ;

		/// <summary>
		/// Abrieviated organization name
		/// </summary>
		public string AbbrOrgName	= string.Empty;




		public bool ProcessXML(XmlNode  node)
		{
            if (_partyID == "")
            {
                return false;
            }
			//XmlNode tmpChildNode;
			string tmpValue			= string.Empty ;
			try
			{
				// get the Party node with the party id
				XmlNode tmpNode			= node.SelectSingleNode("//OLifE/Party[@id='" +  
											this.PartyID + "']");


				if (tmpNode != null)
				{
					foreach (XmlNode tmpChild in tmpNode.ChildNodes)
					{
						switch (tmpChild.Name)
						{
							case "PartyTypeCode" :
								this.PartyType		= tmpChild.InnerText;  // should be person
                                this.PartyTypeCode = XMLUtility.GetAttributeValue(tmpChild, "tc");
								break;

							case "GovtID" : 
								this.GovtID 		= tmpChild.InnerText;  // ssn
								
								break;

							case "FullName" : 
								this.FullName 		= tmpChild.InnerText;  // Organization name
								
								break;

							case "Organization":
								XmlNode abbrNode	= node.SelectSingleNode("//Organization/AbbrName");
								if (abbrNode != null ) this.AbbrOrgName	= abbrNode.InnerText;
								break;

                            case "IDReferenceNo":
                                this.IDReferenceNo = tmpChild.InnerText;  // memberid for usaa alip
                                break;

                            case "Producer":
                                var carriernode = tmpChild.SelectSingleNode("//CarrierAppointment/CompanyProducerID");
                                if (carriernode != null)
                                    this.CompanyProducerID = carriernode.InnerText;

                                carriernode = tmpChild.SelectSingleNode("//CarrierAppointment/CarrierCode");
                                if (carriernode != null)
                                    this.AgencyCarrierCode = carriernode.InnerText;

                                break;

                                //							case "Person" : 
                                //								this.Person.ParseXML (tmpChild);
                                //								break;
                                //
                                //							case "Phone" : 
                                //								// get the phone type so we can get the phone values
                                //								tmpValue			= CommonUtilities.GetAttributeValue (tmpChild, "tc");
                                //								if (tmpValue == "2")  // business phone
                                //								{
                                //									
                                //								}
                                //								else if (tmpValue == "4") // business fax
                                //								{
                                //								}
                                //								else 
                                //								{
                                //									// house phone
                                //									
                                //								}
                                //
                                //								break;
                                //
                                //
                                //
                                //							
                        }
					}
					
				}

				return true;
			}
			catch(Exception e)
			{
                //EventLog.WriteEntry (e.Source , e.Message + "\n" + "Stack = " + e.StackTrace, EventLogEntryType.Error);
                base.LogError(e);
				return false;
			}
			
		}

		#endregion PROPERTIES

        public TXLifeRequestOLifEParty()
		{
			//
			// TODO: Add constructor logic here
			//

		}


	

		
	}
}
