using System;
using System.Xml;

namespace BusinessServices.BAL.ACORD
{
	/// <summary>
	/// Summary description for IITTXLifeRequest.
	/// </summary>
	public class TXLifeRequestAttachment : Logger, IProcessXML
	{
		private string _attachmentdata	= string.Empty ;
		public string AttachmentData
		{
            get { return _attachmentdata; }
			set 
			{
                _attachmentdata = value;
			}
		}


        private string _description = string.Empty;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
            }
        }

        private string _attachmentTypetc = "1";
        public string AttachmentTypetc
		{
            get { return _attachmentTypetc; }
			set 
			{	
				if (value != "")
                    _attachmentTypetc = value;
			}
		}


        private string _mimeTypeTC = string.Empty;	// should be 2 or 3 for PDF and tiff
        public string MimeTypeTC  
		{
            get { return _mimeTypeTC; }
			set 
			{	
				if (value != "")
                    _mimeTypeTC = value;
			}
		}

        
        public string ImageType
        {
            get {
                switch (_mimeTypeTC)
                {
                    case "2" : return "PDF";
                    case "3": return "TIFF";
                    default:
                        return "";
                }
            }
            
        }


        public TXLifeRequestAttachment()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		/// <summary>
		/// Retrieves the values of TXLifeRequest node and populates the properties of ACORDTXLifeRequest object
		/// </summary>
		/// <param name="dom">XML dom</param>
		/// <returns>True/False</returns>
        public bool ProcessXML(XmlNode node)
		{
            // application information
            if (node == null)
                return false;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(node.OuterXml);

			// GUID ID
            XmlNode tmpNode2 = xmldoc.SelectSingleNode("//Attachment");
			try
			{
                if (tmpNode2 != null)
				{
                    foreach (XmlNode childNode in tmpNode2.ChildNodes)
					{
						switch (childNode.Name.ToString().ToUpper ())
						{
							case "ATTACHMENTDATA":
								this.AttachmentData		= childNode.InnerText;
								//this.TransCode		= XMLUtility.GetAttributeValue(childNode, "tc");  // should be 121
								break;
							
							case "DESCRIPTION":
								this.Description	= childNode.InnerText;
								break;

							case "ATTACHMENTTYPE":
                                this.AttachmentTypetc = XMLUtility.GetAttributeValue(childNode, "tc");
								break;

							case "MIMETYPETC":
                                this.MimeTypeTC = XMLUtility.GetAttributeValue(childNode, "tc");
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
