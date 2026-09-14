using System;
using System.Xml;
namespace BusinessServices.BAL.ACORD
{
	/// <summary>
	/// Summary description for TXLifeRequestOLifEHolding.
	/// </summary>
	public class TXLifeRequestOLifEHolding : Logger
	{
        public TXLifeRequestOLifEHolding()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		private string _holdingTypeCode 	= string.Empty ;
		public string HoldingTypeCode
		{
			get {return _holdingTypeCode;}
			set 
			{	
				if (value != "")
				{
					if (value.Length > 30)
						_holdingTypeCode	= value.Substring(0, 30);
					else
						_holdingTypeCode	= value.Substring (0, value.Length);
				}

			}
		}


		private string _holdingTypeCodeTC 	= string.Empty ;
		public string HoldingTypeCodeTC
		{
			get {return _holdingTypeCodeTC;}
			set 
			{	
				if (value != "")
				{
					if (_holdingTypeCodeTC.Length > 10)
						_holdingTypeCodeTC	= value.Substring(0, 10);
					else
						_holdingTypeCodeTC	= value.Substring (0, value.Length);
				}
			}
		}

        private string _casenumber = string.Empty;
        public string CaseNumber
        {
            get { return _casenumber; }
            set
            {
                if (value != "")
                {
                    if (_casenumber.Length > 50)
                        _casenumber = value.Substring(0, 50);
                    else
                        _casenumber = value.Trim();
                }
            }
        }



		public bool ProcessXML(XmlNode node)
		{
            // application information
            if (node == null)
                return false;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(node.OuterXml);

			// GUID ID
			//	XmlNode tmpNode	= dom.SelectSingleNode("//TXLife/TXLifeRequest");
			try
			{
                XmlNode tmpNode = xmldoc.SelectSingleNode("//OLifE/Holding");
				if (tmpNode != null)
				{
					
					foreach (XmlNode childNode in tmpNode.ChildNodes)
					{
						switch (childNode.Name.ToString().ToUpper ())
						{
							case "HOLDINGTYPECODE":
								this.HoldingTypeCode	 	= childNode.InnerText;
								this.HoldingTypeCodeTC 		= XMLUtility.GetAttributeValue(childNode, "tc");  // should be 121
								break;

						    case "HOLDINGKEY":
                                this.CaseNumber = childNode.InnerText;
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
