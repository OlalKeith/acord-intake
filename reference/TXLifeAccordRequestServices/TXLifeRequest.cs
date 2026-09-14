using System;
using System.Xml;

namespace BusinessServices.BAL.ACORD
{
	/// <summary>
	/// Summary description for IITTXLifeRequest.
	/// </summary>
	public class TXLifeRequest : Logger, IProcessXML
	{
        

        private string _transRefGUID	= string.Empty ;
		public string TransRefGUID
		{
			get {return _transRefGUID;}
			set 
			{	
				if (value.Length > 100)
					_transRefGUID	= value.Substring (0, 100);
				else
					_transRefGUID	= value;
			}
		}



		private string _transType	= string.Empty ;
		public string TransType 
		{
			get {return _transType;}
			set 
			{	
				if (value != "")
					_transType	= value;
			}
		}


		private string _transCode	= string.Empty ;	// should be 121 for IIT
		public string TransCode 
		{
			get {return _transCode;}
			set 
			{	
				if (value != "")
					_transCode	= value;
			}
		}



		private string _transExeDate	= string.Empty ;
		public string TransExeDate 
		{
			get {return _transExeDate;}
			set 
			{	
				if (value.Length > 20)
					_transExeDate	= value.Substring(0,20);
				else
					_transExeDate	= value;
			}
		}

		private string _transExeTime	= string.Empty ;
		public string TransExeTime 
		{
			get {return _transExeTime;}
			set 
			{	
				if (value.Length > 20)
					_transExeTime	= value.Substring(0,20);
				else
					_transExeTime	= value;
			}

		}


		private string _transMode 	= string.Empty ;
		public string TransMode  
		{
			get {return _transMode;}
			set 
			{	
				if (value != "")
					_transMode	= value;
			}
		}


		private string _transModeCode 	= string.Empty ;
		public string TransModeCode  
		{
			get {return _transModeCode;}
			set 
			{	
				if (value != "")
					_transModeCode	= value;
			}
		}

        private string _testIndicator = string.Empty;
		public string TestIndicator  
		{
			get {return _testIndicator;}
			set 
			{
               _testIndicator = value.Trim();
               
			}
		}

		private string _testIndicatorCode 	= string.Empty ;
		public string TestIndicatorCode
		{
			get {return _testIndicatorCode;}
			set 
			{	
				if (value != "")
					_testIndicatorCode	= value;
			}
		}


		 
		

		
		public TXLifeRequest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public bool ProcessXML(WorkOrder wo, XmlDocument dom)
		{
			bool parsedSuccess	= ProcessXML(dom);
			try
			{
				if (parsedSuccess)
				{
					wo.RequestID		= this.TransRefGUID;
					wo.TransExeDate		= this.TransExeDate;
					wo.TransExeTime		= this.TransExeTime;
					
				}

				return true;
			}
			catch (Exception)
			{
				//EventLog.WriteEntry (e.Source , e.Message + "\n" + "Stack = " + e.StackTrace, EventLogEntryType.Error);
				return false;

			}
		}

		public bool ProcessXML(XmlDocument dom)
		{
            
			return true;
		}

		/// <summary>
		/// Retrieves the values of TXLifeRequest node and populates the properties of ACORDTXLifeRequest object
		/// </summary>
		/// <param name="dom">XML dom</param>
		/// <returns>True/False</returns>
		public bool ProcessXML(XmlNode tmpNode)
		{

			// GUID ID
		//	XmlNode tmpNode	= dom.SelectSingleNode("//TXLife/TXLifeRequest");
			try
			{
				if (tmpNode != null)
				{
					foreach (XmlNode childNode in tmpNode.ChildNodes)
					{
						switch (childNode.Name.ToString().ToUpper ())
						{
							case "TRANSTYPE":
								this.TransType		= childNode.InnerText;
								this.TransCode		= XMLUtility.GetAttributeValue(childNode, "tc");  // should be 121
								break;
							
							case "TRANSEXEDATE":
								this.TransExeDate	= childNode.InnerText;
								break;

							case "TRANSEXETIME":
								this.TransExeTime	= childNode.InnerText;
								break;

							case "TRANSMODE":       // this is use for cancellation
								this.TransMode		= childNode.InnerText;
                                this.TransModeCode  = XMLUtility.GetAttributeValue(childNode, "tc");  // 
								break;

							case "TESTINDICATOR":
								this.TestIndicator		= childNode.InnerText;
                                this.TestIndicatorCode = XMLUtility.GetAttributeValue(childNode, "tc");
								break;

							case "TRANSREFGUID":
								this.TransRefGUID		= childNode.InnerText;
								break;
						}
					}

					return true;
				}
				return false;
			}
			catch(Exception e)
			{
                //EventLog.WriteEntry (e.Source , e.Message + "\n" + "Stack = " + e.StackTrace, EventLogEntryType.Error);
                base.LogError(e, "ProcessXML");
                return false;
			}
		}

	}
}
