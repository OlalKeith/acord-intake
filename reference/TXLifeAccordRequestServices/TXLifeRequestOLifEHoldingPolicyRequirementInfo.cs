using System;
using System.Xml.Serialization;
using System.Xml;


namespace BusinessServices.BAL.ACORD
{
	/// <summary>
	/// Summary description for TXLifeRequestOLifEHoldingPolicyRequirementInfo.
	/// </summary>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ACORD.org/Standards/Life/2")]
	public class TXLifeTXLifeRequestOLifEHoldingPolicyRequirementInfo :Logger, IProcessXML 
	{


        public TXLifeTXLifeRequestOLifEHoldingPolicyRequirementInfo() { }
    
		private string _requirementInfoUniqueID	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute("RequirementInfoUniqueID")]
		public string RequirementInfoUniqueID
		{
			get {return _requirementInfoUniqueID;}
			set 
			{	
				if (value != "")
					_requirementInfoUniqueID	= value;
			}
		}




		private string _reqCode	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute("ReqCode")]
		///
		/// Copy Instruction
		///
		public string ReqCode
		{
			get {return _reqCode;}
			set 
			{	
				if (value != "")
					_reqCode	= value;
			}
		}



		private string _reqCodeTC	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute("ReqCodeTC")]
			///
			/// Copy Instruction
			///
		public string ReqCodeTC
		{
			get {return _reqCodeTC;}
			set 
			{	
				if (value != "")
				{
					if (value.Length > 10)
						_reqCodeTC	= value.Substring (0, 10);
					else
						_reqCodeTC	= value.Substring (0, value.Length);
				}
			}
		}

	

		
		private string _sequence	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute("Sequence")]
		public string Sequence
		{
			get {return _sequence;}
			set 
			{	
				if (value != "")
					_sequence	= value;
			}
		}


		private string _requestedDate	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute("RequestedDate")]
		public string RequestedDate
		{
			get {return _requestedDate;}
			set 
			{	
				if (value.Length > 12)
					_requestedDate	= value.Substring(0,12);
				else
					_requestedDate	= value.Substring(0, value.Length);
			}
		}




		private string _scheduledDate	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute("ScheduledDate")]
		public string ScheduledDate
		{
			get {return _scheduledDate;}
			set 
			{	
				if (value != "")
				{
					if (value.Length > 12)
						_scheduledDate	= value.Substring(0,12);
					else
						_scheduledDate	= value.Substring(0, value.Length);
				}
			}
		}



		private string _releasePartyOrgCode	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute("ReleasePartyOrgCode")]
		public string ReleasePartyOrgCode
		{
			get {return _releasePartyOrgCode;}
			set 
			{	
				if (value != "")
					_releasePartyOrgCode	= value;
			}
		}


	


		 
		private string _reqCategory	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute("ReqCategory")]
		public string ReqCategory
		{
			get {return _reqCategory;}
			set 
			{	
				if (value != "")
					_reqCategory	= value;
			}
		}




		private string _reqStatus	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute("ReqStatus")]
		public string ReqStatus
		{
			get {return _reqStatus;}
			set 
			{	
				if (value != "")
					_reqStatus	= value;
			}
		}




		private string _requirementDetails	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute("RequirementDetails")]
		public string RequirementDetails
		{
			get {return _requirementDetails;}
			set 
			{
                _requirementDetails = value;
                //if (value != "")
                //{

                //    //if (value.Length > 50)
                //    //    _requirementDetails	= value.Substring(0, 50);
                //    //else
                //    //    _requirementDetails = value.Substring (0, value.Length);
                //}
			}
		}


		


		private string _requirementAcctNum	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute("RequirementAcctNum")]
		public string RequirementAcctNum
		{
			get {return _requirementAcctNum;}
			set 
			{	
				_requirementAcctNum	= value.Trim();
			}
		}





		private string _requestorContactPartyID	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute("RequestorContactPartyID")]
		public string RequestorContactPartyID
		{
			get {return _requestorContactPartyID;}
			set 
			{	
				_requestorContactPartyID	= value.Trim();
			}
		}


		

		private string _requesterPartyID	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute("RequesterPartyID")]
		public string RequesterPartyID
		{
			get {return _requesterPartyID;}
			set 
			{	
				_requesterPartyID	= value.Trim();
			}
		}
	


		private string _statusReceiverPartyID	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute("StatusReceiverPartyID")]
		public string StatusReceiverPartyID
		{
			get {return _statusReceiverPartyID;}
			set 
			{	
				_statusReceiverPartyID	= value.Trim();
			}
		}


		private string _resultsReceiverPartyID	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute("ResultsReceiverPartyID")]
		public string ResultsReceiverPartyID
		{
			get {return _resultsReceiverPartyID;}
			set 
			{	
				_resultsReceiverPartyID	= value.Trim();
			}
		}
		
		
		private string _appliesToPartyID	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string AppliesToPartyID
		{
			get {return _appliesToPartyID;}
			set 
			{	
				_appliesToPartyID	= value.Trim();
			}
		}
	



		private string _appliesToParticipantID	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string AppliesToParticipantID
		{
			get {return _appliesToParticipantID;}
			set 
			{	
				_appliesToParticipantID	= value.Trim();
			}
		}
	

    

		private string _fulfillerPartyID	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string FulfillerPartyID
		{
			get {return _fulfillerPartyID;}
			set 
			{	
				_fulfillerPartyID	= value.Trim();
			}
		}
	
		
		private string _id	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string ID
		{
			get {return _id;}
			set 
			{
                _id = value.Trim();
			}
		}



		private string _applicationInfoTrackingID	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute()]
        public string ApplicationInfoTrackingID
		{
            get { return _applicationInfoTrackingID; }
			set 
			{	
				_applicationInfoTrackingID = value.Trim();
			}
		}


		private string _providerOrderNum	= string.Empty;
		public  string ProviderOrderNum
		{
			get {return _providerOrderNum;}
			set 
			{
				if (value.Length > 0)
				{
					if (value.Length > 50)
						_providerOrderNum	= value.Substring(0, 50);
					else
						_providerOrderNum	= value.Substring(0, value.Length);
				}
			}
		}


		private string _deliveryInstructionDesc = string.Empty;
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string DeliveryInstructionDesc
		{
			get { return _deliveryInstructionDesc; }
			set
			{
				_deliveryInstructionDesc = value.Trim();
			}
		}


		private string _feeCapAmt = string.Empty;
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string FeeCapAmt
		{
			get { return _feeCapAmt; }
			set
			{
				_feeCapAmt = value.Trim();
			}
		}


		private string _priority = string.Empty;
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Priority
		{
			get { return _priority; }
			set
			{
				_priority = value.Trim();
			}
		}


		private string _unitcode = string.Empty;
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string UnitCode
		{
			get { return _unitcode; }
			set
			{
				_unitcode = value.Trim();
			}
		}

		// added 2022/03/13
		private string _hORequirementRefID = string.Empty;
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string HORequirementRefID
		{
			get { return _hORequirementRefID; }
			set
			{
				_hORequirementRefID = value.Trim();
			}
		}
		
		public bool ProcessXML(XmlNode node)
		{
			XmlNode tmpNode	= null;

			try
			{
                // application information
                if (node == null)
                    return false;

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(node.OuterXml);

				// GET THE APPLICANT INFORMATION
                tmpNode = xmldoc.SelectSingleNode("//OLifE/Holding/Policy/ApplicationInfo");
				if (tmpNode != null)
				{
					foreach (XmlNode childNode in tmpNode.ChildNodes )
					{
						switch (childNode.Name.ToString().ToUpper ())
						{
							case "TRACKINGID" : 
				
								this.ApplicationInfoTrackingID = childNode.InnerText;;
								break;							
						}
					
					}

				}


				// requirement information
                if (ID.Length > 0)
                    tmpNode = xmldoc.SelectSingleNode("//OLifE/Holding/Policy/RequirementInfo[@id='" + ID + "']");
                else
                    tmpNode = xmldoc.SelectSingleNode("//OLifE/Holding/Policy/RequirementInfo");
				
				if (tmpNode != null)
				{
                    this.RequestorContactPartyID = XMLUtility.GetAttributeValue(tmpNode, "RequestorContactPartyID");
                    this.AppliesToPartyID = XMLUtility.GetAttributeValue(tmpNode, "AppliesToPartyID");
                    this.FulfillerPartyID = XMLUtility.GetAttributeValue(tmpNode, "FulfillerPartyID");
                    this.AppliesToParticipantID = XMLUtility.GetAttributeValue(tmpNode, "AppliesToParticipantID");
                    this.RequesterPartyID = XMLUtility.GetAttributeValue(tmpNode, "RequesterPartyID");
					foreach (XmlNode childNode in tmpNode.ChildNodes )
					{
						switch (childNode.Name.ToString().ToUpper ())
						{
							case "REQCODE" : 
								this.ReqCode 					= childNode.InnerText;
								this.ReqCodeTC					= XMLUtility.GetAttributeValue (childNode, "tc");
								break;

							case "REQUIREMENTDETAILS":
								this.RequirementDetails			= childNode.InnerText;
								break;

							case "SCHEDULEDDATE":
								this.ScheduledDate				= childNode.InnerText;
								break;
								
							case "REQUIREMENTINFOUNIQUEID":
								this.RequirementInfoUniqueID 	= childNode.InnerText;
								
								break;

							case "SEQUENCE":
								this.Sequence 					= childNode.InnerText;
								break;

							case "REQSTATUS":
								this.ReqStatus					= childNode.InnerText;
								break;

							case "USERCODE":
								this.UnitCode = childNode.InnerText;
								break;

							case "REQUESTEDDATE":
								this.RequestedDate				= childNode.InnerText ;
								
								break;

							case "HOREQUIREMENTREFID":
								this.HORequirementRefID  = childNode.InnerText;
								break;

							case "RELEASEPARTYORGCODE":
								this.ReleasePartyOrgCode		= childNode.InnerText;
								break;


							case "REQUIREMENTACCTNUM":
								this.RequirementAcctNum			= childNode.InnerText ;
								break;

							case "REQCATEGORY":  
								this.ReqCategory				= childNode.InnerText;
								break;

							case "STATUSRECEIVERPARTY":
                                this.StatusReceiverPartyID = XMLUtility.GetAttributeValue(childNode, "StatusReceiverPartyID"); 
								break;


							case "RESULTSRECEIVERPARTY":
                                this.ResultsReceiverPartyID = XMLUtility.GetAttributeValue(childNode, "ResultsReceiverPartyID"); 
								break;

							
							case "PROVIDERORDERNUM":  
								this.ProviderOrderNum			= childNode.InnerText;
								break;

							case "PRIORITY":
								this.Priority = childNode.InnerText;
								break;
							case "DELIVERYINSTRUCTIONDESC":
								this.DeliveryInstructionDesc = childNode.InnerText;
								break;

                            case "DELIVERYINSTRUCTIONSDESC":
                                if (string.IsNullOrEmpty(this.DeliveryInstructionDesc))
                                    this.DeliveryInstructionDesc = childNode.InnerText;
                                break;

                            case "TRACKINGINFO":
								var localtemp = childNode.SelectSingleNode("//DeliveryInstructionDesc");
								if (localtemp != null)
                                {
									if (string.IsNullOrEmpty(this.DeliveryInstructionDesc))
										this.DeliveryInstructionDesc = localtemp.InnerText;
								}
								localtemp = childNode.SelectSingleNode("//Fee/FeeCapAmt");
								if (localtemp != null)
								{
									this.FeeCapAmt = localtemp.InnerText;
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
				tmpNode			= null;
				//EventLog.WriteEntry (e.Source , e.Message + "\n" + "Stack = " + e.StackTrace, EventLogEntryType.Error);
				return false;
			}
		}



	}
}
