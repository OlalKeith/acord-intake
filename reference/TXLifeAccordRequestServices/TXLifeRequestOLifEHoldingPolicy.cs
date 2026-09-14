using System;
using System.Xml;


namespace BusinessServices.BAL.ACORD
{
	/// <summary>
	/// Summary description for TXLifeRequestOLifEHoldingPolicy.
	/// </summary>
	public class TXLifeRequestOLifEHoldingPolicy :Logger, IProcessXML
	{
        public TXLifeRequestOLifEHoldingPolicy()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// could be an insurance carrier
		/// </summary>
		private string _carrierPartyID	= string.Empty ;
		public string CarrierPartyID
		{
			get {return _carrierPartyID;}
			set 
			{	
				if (value != "")
					_carrierPartyID	= value;
			}
		}

		/// <remarks/>
		private string _polNumber	= string.Empty ;
		public string PolNumber
		{
			get {return _polNumber;}
			set 
			{	
				if (value.Length > 20)
					_polNumber	= value.Substring(0, 20);
				else
					_polNumber	= value;
			}
		}
    

		/// case number
		private string _caseControlNumberAssuming	= string.Empty ;
		public string CaseControlNumberAssuming
		{
			get {return _caseControlNumberAssuming;}
			set 
			{	
				if (value != "")
					_caseControlNumberAssuming	= value;
			}
		}

		/// <remarks/>
		private string _productCode	= string.Empty ;
		public string ProductCode
		{
			get {return _productCode;}
			set 
			{	
				if (value != "")
					_productCode	= value;
			}
		}




		/// <remarks/>
		private string _productType	= string.Empty ;
		public string ProductType
		{
			get {return _productType;}
			set 
			{	
				if (value.Length > 30)
				  _productType		= value.Substring(0,30);
				else
					_productType	= value;
			}
		}


		private string _productTypeTC	= string.Empty ;
		public string ProductTypeTC
		{
			get {return _productTypeTC;}
			set 
			{	
				if (value != "")
				{
					if (_productTypeTC.Length > 10)
						_productTypeTC	= value.Substring(0, 10);
					else
						_productTypeTC	= value.Substring(0, value.Length);
				}
			}
		}


    

		private string _carrierCode	= string.Empty ;
		public string CarrierCode
		{
			get {return _carrierCode;}
			set 
			{	
				if (value != "")
					_carrierCode	= value;
			}
		}

    
		

		private string _planName	= string.Empty ;
		public string PlanName
		{
			get {return _planName;}
			set 
			{	
				if (value != "")
					_planName	= value;
			}
		}



    
		
		private string _effDate	= string.Empty ;
		public string EffDate
		{
			get {return _effDate;}
			set 
			{	
				if (value != "")
					_effDate	= value;
			}
		}




		private string _paymentMode	= string.Empty ;
		public string PaymentMode
		{
			get {return _paymentMode;}
			set 
			{	
				if (value != "")
					_paymentMode	= value;
			}
		}



		private string _paymentModeCode	= string.Empty ;
		public string PaymentModeCode
		{
			get {return _paymentModeCode;}
			set 
			{	
				if (value != "")
					_paymentModeCode	= value;
			}
		}



		
		private string _paymentMethod	= string.Empty ;
		public string PaymentMethod
		{
			get {return _paymentMethod;}
			set 
			{	
				if (value != "")
					_paymentMethod	= value;
			}
		}
    



		private string _paymentMethodCode	= string.Empty ;
		public string PaymentMethodCode
		{
			get {return _paymentMethodCode;}
			set 
			{	
				if (value != "")
					_paymentMethodCode	= value;
			}
		}

		private string _billcode = string.Empty;
		public string BillCode
		{
			get { return _billcode; }
			set
			{
				if (value != "")
					_billcode = value;
			}
		}

		public bool ProcessXML(XmlNode node)
		{
			
			try
			{
                // application information
                if (node == null)
                    return false;

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(node.OuterXml);
				// Holding is always required
                XmlNode tmpNode = xmldoc.SelectSingleNode("//OLifE/Holding/Policy");
				if (tmpNode != null)
				{

                    this.CarrierPartyID = XMLUtility.GetAttributeValue(tmpNode, "CarrierPartyID"); // we need this to find out about carrier information
					
					foreach (XmlNode childNode in tmpNode.ChildNodes)
					{
						switch (childNode.Name.ToString().ToUpper ())
						{
							case "POLNUMBER":
								this.PolNumber		= childNode.InnerText ;
								break;

							case "PRODUCTTYPE":
								this.ProductType 		= childNode.InnerText ;
                                this.ProductTypeTC = XMLUtility.GetAttributeValue(childNode, "tc");
								break;

							case "PRODUCTCODE":
								this.ProductCode 		= childNode.InnerText ;
								break;

							case "CARRIERCODE":
								this.CarrierCode  		= childNode.InnerText ;
								break;

							case "PLANNAME":
								this.PlanName	 		= childNode.InnerText ;
								break;

							case "EFFDATE":
								this.EffDate			= childNode.InnerText;
								break;

							case "PAYMENTMODE":
								this.PaymentMode  		= childNode.InnerText ;
								break;

							case "PAYMENTMETHOD":
								this.PaymentMethod 		= childNode.InnerText ;
								break;
							case "BILLNUMBER":
								this.BillCode = childNode.InnerText;
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



	}
}
