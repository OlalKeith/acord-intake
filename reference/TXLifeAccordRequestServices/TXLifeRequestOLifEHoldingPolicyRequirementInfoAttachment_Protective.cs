using System;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using BusinessServices.BAL.Mail;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Drawing;

namespace BusinessServices.BAL.ACORD
{
    public class TXLifeRequestOLifEHoldingPolicyRequirementInfoAttachment_Protective : Logger, IProcessXML
    {
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public TXLifeRequestOLifEHoldingPolicyRequirementInfoAttachment_Protective() { }

        private List<string> _attachementdata	= new List<string>();
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public List<string> AttachmentData
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

        public async Task<string> SaveAttachment(string workordernumber, string folder, string fileprefix)
        {

            string filename = string.Empty;
            if (ImageType.ToLower().IndexOf("tif") >= 0)
                filename = fileprefix + workordernumber.Trim() + ".tif";
            else if (ImageType.ToLower().IndexOf("pdf") >= 0)
                filename = fileprefix + workordernumber.Trim() + ".pdf";

            if (filename.Length == 0)
                return "INVALID FILE TYPE. PDF OR TIFF ONLY.";



            List<byte[]> bytelist = new List<byte[]>();


            // add the folder path to it
            //filename = folder + @"\" + filename;
            
            foreach (string datastring in _attachementdata)
            {
                int fstreamLength = datastring.Length;
                try
                {
                    byte[] base64Convert = new byte[fstreamLength];

                    base64Convert = Convert.FromBase64String
                        (datastring.Replace(" ", "+"));

                    bytelist.Add(base64Convert);
                    // save the file to server location

                    //await streamWriter.WriteAsync(base64Convert, 0, base64Convert.Length);
                    //streamWriter.Close();
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
            }

            if (bytelist.Count > 0)
            {
                if (filename.EndsWith(".tif"))
                {
                    FileStream streamWriter = new FileStream(folder + @"\" + filename, FileMode.Create);
                    var mergebytes = MergeTiff(bytelist);
                    await streamWriter.WriteAsync(mergebytes, 0, mergebytes.Length);
                    streamWriter.Close();
                    return folder + filename;
                }
                else
                {
                    if (filename.EndsWith(".pdf"))
                    {
                        FileStream streamWriter = new FileStream(folder + @"\" + filename, FileMode.Create);
                        var mergebytes = MergePdfFiles(bytelist);
                        await streamWriter.WriteAsync(mergebytes, 0, mergebytes.Length);
                        streamWriter.Close();
                        return folder + filename;
                    }
                    // we only have 1 tif file
                }

            }
           
            return "ATTACHMENT DATA IS INVALID";


        }

        private byte[] MergeTiff(List<byte[]> tiffFiles)
        {
            byte[] tiffMerge = null;
            using (var msMerge = new MemoryStream())
            {
                //get the codec for tiff files
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo i in ImageCodecInfo.GetImageEncoders())
                    if (i.MimeType == "image/tiff")
                        ici = i;

                Encoder enc = Encoder.SaveFlag;
                EncoderParameters ep = new EncoderParameters(1);

                System.Drawing.Bitmap pages = null;
                int frame = 0;

                foreach (var tiffFile in tiffFiles)
                {
                    using (var imageStream = new MemoryStream(tiffFile))
                    {
                        using (Image tiffImage = Image.FromStream(imageStream))
                        {
                            foreach (Guid guid in tiffImage.FrameDimensionsList)
                            {
                                //create the frame dimension 
                                FrameDimension dimension = new FrameDimension(guid);
                                //Gets the total number of frames in the .tiff file 
                                int noOfPages = tiffImage.GetFrameCount(dimension);

                                for (int index = 0; index < noOfPages; index++)
                                {
                                    FrameDimension currentFrame = new FrameDimension(guid);
                                    tiffImage.SelectActiveFrame(currentFrame, index);
                                    using (MemoryStream tempImg = new MemoryStream())
                                    {
                                        tiffImage.Save(tempImg, ImageFormat.Tiff);
                                        {
                                            if (frame == 0)
                                            {
                                                //save the first frame
                                                pages = (Bitmap)Image.FromStream(tempImg);
                                                ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.MultiFrame);
                                                pages.Save(msMerge, ici, ep);
                                            }
                                            else
                                            {
                                                //save the intermediate frames
                                                ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.FrameDimensionPage);
                                                pages.SaveAdd((Bitmap)Image.FromStream(tempImg), ep);
                                            }
                                        }
                                        frame++;
                                    }
                                }
                            }
                        }
                    }
                }
                if (frame > 0)
                {
                    //flush and close.
                    ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.Flush);
                    pages.SaveAdd(ep);
                }

                msMerge.Position = 0;
                tiffMerge = msMerge.ToArray();
            }
            return tiffMerge;
        }


        private byte[] MergePdfFiles(List<byte[]> sourceFiles)
        {

            iTextSharp.text.Document document = new iTextSharp.text.Document();
            using (MemoryStream ms = new MemoryStream())
            {
                iTextSharp.text.pdf.PdfCopy copy = new iTextSharp.text.pdf.PdfCopy(document, ms);
                document.Open();
                int documentPageCounter = 0;

                // Iterate through all pdf documents
                for (int fileCounter = 0; fileCounter < sourceFiles.Count; fileCounter++)
                {
                    // Create pdf reader
                    iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(sourceFiles[fileCounter]);
                    int numberOfPages = reader.NumberOfPages;

                    // Iterate through all pages
                    for (int currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
                    {
                        documentPageCounter++;
                        iTextSharp.text.pdf.PdfImportedPage importedPage = copy.GetImportedPage(reader, currentPageIndex);
                        iTextSharp.text.pdf.PdfCopy.PageStamp pageStamp = copy.CreatePageStamp(importedPage);

                        // Write header
                        iTextSharp.text.pdf.ColumnText.ShowTextAligned(pageStamp.GetOverContent(), iTextSharp.text.Element.ALIGN_CENTER,
                            new iTextSharp.text.Phrase(""), importedPage.Width / 2, importedPage.Height - 30,
                            importedPage.Width < importedPage.Height ? 0 : 1);

                        // Write footer
                        iTextSharp.text.pdf.ColumnText.ShowTextAligned(pageStamp.GetOverContent(), iTextSharp.text.Element.ALIGN_CENTER,
                            new iTextSharp.text.Phrase(String.Format("Page {0}", documentPageCounter)), importedPage.Width / 2, 10,
                            importedPage.Width < importedPage.Height ? 0 : 1);

                        pageStamp.AlterContents();

                        copy.AddPage(importedPage);
                    }

                    copy.FreeReader(reader);
                    reader.Close();
                }

                document.Close();
                return ms.GetBuffer();
            }
        }


        public bool ProcessXML(XmlNode node)
		{
			//XmlNode tmpNode	= null;
            try
            {
                // application information
                if (node == null)
                    return false;

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(node.OuterXml);


                // GET THE APPLICANT INFORMATION
                var attachmentnodes = xmldoc.SelectNodes("//OLifE/Holding/Policy/RequirementInfo/Attachment");
                if (attachmentnodes == null)
                    return false;
                foreach (XmlNode attachmentNode in attachmentnodes)
                {
                    foreach (XmlNode childNode in attachmentNode.ChildNodes)
                    {
                        switch (childNode.Name.ToString().ToUpper())
                        {
                            case "ATTACHMENTDATA":
                                this.AttachmentData.Add(childNode.InnerText);

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
                                    break;
                                }
                                if (ImageType.Length == 0)
                                {
                                    if (childNode.InnerText.ToUpper().Contains("PDF"))
                                        this.ImageType = "pdf";

                                    if (childNode.InnerText.ToUpper().Contains("TIF"))
                                        this.ImageType = "tif";
                                }
                                break;
                        }
                    }
                    
                }
            }
            catch (Exception ex) {
                base.LogError(ex, this.GetType().FullName + ".ProcessXML()");
            }
            return true;
		}

    }
}
