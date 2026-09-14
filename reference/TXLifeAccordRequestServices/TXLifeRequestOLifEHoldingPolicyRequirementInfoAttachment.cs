using System;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using BusinessServices.BAL.Mail;
using System.Threading.Tasks;

namespace BusinessServices.BAL.ACORD
{
    public class TXLifeRequestOLifEHoldingPolicyRequirementInfoAttachment :Logger, IProcessXML
    {
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public TXLifeRequestOLifEHoldingPolicyRequirementInfoAttachment() { }

        private string _attachementdata	= string.Empty ;
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string AttachmentData
		{
            get { return _attachementdata; }
			set 
			{
                _attachementdata = value;
			}
		}


        private string _attachmenttype  = string.Empty;
        public string AttachmentType
        {
            get { return _attachmenttype; }
            set { _attachmenttype = value; }
        }


        private string _imagetype = string.Empty;
        public string ImageType
        {
            get { return _imagetype; }
            set { _imagetype = value; }
        }


        private string _mimetypetc = string.Empty;
        public string MimeTypeTC
        {
            get { return _mimetypetc; }
            set { _mimetypetc = value; }
        }

        public async Task<string> SaveAttachment (string workordernumber, string folder, string fileprefix)
        {

            string filename = string.Empty;
            if (ImageType.ToLower().IndexOf("tif") >= 0)
                filename = fileprefix + workordernumber.Trim() + ".tif";
            else if (ImageType.ToLower().IndexOf("pdf") >= 0)
                filename = fileprefix + workordernumber.Trim() + ".pdf";

            if (filename.Length == 0)
                return "INVALID FILE TYPE. PDF OR TIFF ONLY.";

            // add the folder path to it
            //filename = folder + @"\" + filename;

            int fstreamLength = _attachementdata.Length;
            try
            {
                byte[] base64Convert = new byte[fstreamLength];

                base64Convert = Convert.FromBase64String(_attachementdata.Replace(" ", "+"));

         
                // save the file to server location
                FileStream streamWriter = new FileStream(folder + @"\" + filename, FileMode.Create);
                await streamWriter.WriteAsync(base64Convert, 0, base64Convert.Length);
                streamWriter.Close();
            }
            catch (Exception ex)
            {
                
                base.LogError(ex, this.GetType().FullName + ".SaveAttachment()");
                EmailConfiguration ec = new EmailConfiguration() { Password = Config.SMTP_PASSWORD, Server = Config.SMTP_SERVER, Port = Config.SMTP_PORT, Username = Config.SMTP_USERNAME };
                SMTPEmailer smtp = new SMTPEmailer(ec);
                Emailer emailer = new Emailer(smtp);

                emailer.SendEmail("APSCentral@eisonline.com", Config.SUPPORT_EMAIL, this.GetType().FullName + ".SaveAttachment()",
                  ex.Message + "<br><br>Stack Trace: " + ex.StackTrace + "<br><br>" + "File Name: " + folder + @"\" + filename);

                filename = "ATTACHMENT DATA IS INVALID";
            }

            base.LogInfo(folder + filename + " saved");

            return folder + filename;


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
                tmpNode = xmldoc.SelectSingleNode("//OLifE/Holding/Policy/RequirementInfo/Attachment");
                if (tmpNode == null)
                    tmpNode = xmldoc.SelectSingleNode("//OLifE/Holding/Attachment");
                if (tmpNode == null) return false;
                foreach (XmlNode childNode in tmpNode?.ChildNodes)
                {
                    switch (childNode.Name.ToString().ToUpper())
                    {
                        case "ATTACHMENTDATA":
                            this.AttachmentData = childNode.InnerText;

                            break;

                        case "ATTACHMENTTYPE":
                            // SHOULD have a type 239
                            this.AttachmentType = XMLUtility.GetAttributeValue(childNode, "tc");
                            break;

                        //// <ImageType tc="3">3</ImageType>  for eService, 4 is PDF
                        case "IMAGETYPE":
                            if (this.ImageType.Contains("tif") || this.ImageType.Contains("pdf"))
                                break;

                            if (childNode.InnerText == "4" || childNode.InnerText == "PDF")
                                ImageType = "pdf";
                            else if (childNode.InnerText == "3")
                                ImageType = "tiff";

                            var tc = XMLUtility.GetAttributeValue(childNode, "tc");
                            if (tc == "3")
                                ImageType = "tiff";
                            break;


                        case "MIMETYPE":   /// NIIT uses this instead of MIMETYPETC
                            if (this.ImageType.Contains("tif") || this.ImageType.Contains("pdf"))
                                break;
                            this.ImageType = childNode.InnerText;
                            
                           break;


                        case "MIMETYPETC":  // we will loook at both MIMETYPE and MIMETYPETC for pdf or tiff as well

                           this.MimeTypeTC = XMLUtility.GetAttributeValue(childNode, "tc");
                           if (ImageType.Length == 0)  // sometimes eService send both types.. MimeType and ImageType so we have to account for it.
                           {
                               if (MimeTypeTC == "17")
                                   ImageType = "pdf";
                               else if (MimeTypeTC == "3" || MimeTypeTC == "11")
                                   ImageType = "tiff";
                                return true;
                           }
                           if (ImageType.Length == 0)
                            {
                                if (childNode.InnerText.ToUpper().Contains("PDF"))
                                    this.ImageType = "pdf";

                                if (childNode.InnerText.ToUpper().Contains("TIF"))
                                    this.ImageType = "tiff";
                            }
                           break;
                    }
                }
            }
            catch { }
            return true;
		}

    }
}
