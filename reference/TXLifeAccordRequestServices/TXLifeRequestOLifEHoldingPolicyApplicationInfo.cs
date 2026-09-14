using System;
using System.Xml;


namespace BusinessServices.BAL.ACORD
{
	/// <summary>
	/// Summary description for TXLifeRequestOLifEHoldingPolicyApplicationInfo.
	/// </summary>
	public class TXLifeRequestOLifEHoldingPolicyApplicationInfo : Logger, IProcessXML 
	{

		private string _trackingID	= string.Empty ;
		public string TrackingID
		{
			get {return _trackingID;}
			set 
			{	
				if (value != "")
					_trackingID	= value;
			}
		}


        private string _formalAppInd = string.Empty;
        public string FormalAppInd
		{
            get { return _formalAppInd; }
			set 
			{	
				_formalAppInd = value;
			}
		}

        



		private string _applicationJurisdiction	= string.Empty ;
		public string ApplicationJurisdiction
		{
			get {return _applicationJurisdiction;}
			set 
			{	
				if (value != "")
					_applicationJurisdiction	= value;
			}
		}


		private string _signedDate	= string.Empty ;
		public string SignedDate
		{
			get {return _signedDate;}
			set 
			{	
				if (value != "")
					_signedDate	= value;
			}
		}






        public TXLifeRequestOLifEHoldingPolicyApplicationInfo()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		
		public bool ProcessXML (XmlNode node)
		{
			try
			{
				// application information
                if (node == null)
                    return false;

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(node.OuterXml);
                XmlNode tmpNode = xmldoc.SelectSingleNode("//OLifE/Holding/Policy/ApplicationInfo");
				if (tmpNode != null)
				{
					foreach (XmlNode childNode in tmpNode.ChildNodes )
					{
						switch (childNode.Name.ToString().ToUpper ())
						{
							case "TRACKINGID" : 
								this.TrackingID					= childNode.InnerText;
								
								break;

							case "APPLICATIONJURISDICTION":
								this.ApplicationJurisdiction	= childNode.InnerText;
								
								break;

							case "SIGNEDDATE":
								this.SignedDate					= childNode.InnerText;
								break;

                            case "FORMALAPPIND":
                                this.FormalAppInd                = childNode.InnerText;
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
