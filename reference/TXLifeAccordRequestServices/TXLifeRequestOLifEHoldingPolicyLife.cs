using System;
using System.Xml;

namespace BusinessServices.BAL.ACORD
{
	/// <summary>
	/// Summary description for TXLifeRequestOLifEHoldingPolicyLife.
	/// </summary>
	public class TXLifeRequestOLifEHoldingPolicyLife : Logger
	{
        public TXLifeRequestOLifEHoldingPolicyLife()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		private string _initialPremAmt	= string.Empty ;
		public string InitialPremAmt
		{
			get {return _initialPremAmt;}
			set 
			{	
				if (value != "")
				{
					if (value.Length > 10)
						_initialPremAmt	= value.Substring(0, 10);
					else
						_initialPremAmt	= value.Substring(0, value.Length);
				}
			}
		}



		private string _faceAmt	= string.Empty ;
		public string FaceAmt
		{
			get {return _faceAmt;}
			set 
			{	
				if (value != "")
				{
					if (value.Length > 20)
						_faceAmt	= value.Substring(0, 20);
					else
						_faceAmt	= value.Substring (0, value.Length);
				}
			}
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

				// application information
                XmlNode tmpNode = xmldoc.SelectSingleNode("//OLifE/Holding/Policy/Life");
				if (tmpNode != null)
				{
					foreach (XmlNode childNode in tmpNode.ChildNodes )
					{
						switch (childNode.Name.ToString().ToUpper ())
						{
							case "FACEAMT" : 
								this.FaceAmt 			= childNode.InnerText;
								
								break;

							case "INITIALPREMAMT":
								this.InitialPremAmt 	= childNode.InnerText;
								
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
