using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using System.Text;
using System.IO;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Configuration;
using BusinessServices.BAL.Mail;
using System.Data;

namespace BusinessServices.BAL.ACORD
{
    public class GenericACORDController 
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _logid = "";
        public GenericACORDController(string logid) {

            _logid = logid;
            if (string.IsNullOrEmpty(_logid))
            {
                _logger.Info($"New LogId generated. {_logid}");
                _logid = Guid.NewGuid().ToString("N");
            }

        }

        //private string XML_121_RESPONSE_LOCATION = System.Configuration.ConfigurationManager.AppSettings["SAVE_121_RESPONSE"];

        //private const string xsltstylesheet = "<xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\"><xsl:output method=\"xml\" indent=\"no\" encoding=\"UTF-8\"/>" +
        //                                "<xsl:template match=\"/|comment()|processing-instruction()\"><xsl:copy><xsl:apply-templates/></xsl:copy></xsl:template><xsl:template match=\"*\">" +
        //                                "<xsl:element name=\"{local-name()}\"><xsl:apply-templates select=\"@*|node()\"/></xsl:element></xsl:template><xsl:template match=\"@*\">" +
        //                                "<xsl:attribute name=\"{local-name()}\"><xsl:value-of select=\".\"/></xsl:attribute></xsl:template></xsl:stylesheet>";

        #region PROPERTIES
        private string _serviceusername = "";
        public string ServiceUserName
        {
            get { return _serviceusername; }
            set
            {
                if (value.Trim().Length > 0)
                    _serviceusername = value.Trim();
            }
        }


        private string _servicepassword = "";
        public string ServicePassword
        {
            get { return _servicepassword; }
            set
            {
                if (value.Trim().Length > 0)
                    _servicepassword = value.Trim();
            }
        }
        #endregion

        


        //public void ResetSendStatus(string requestid, string status)
        //{

        //    //DAL.AccessProvider provider = new DAL.AccessProvider();
        //    //provider.UpdateReadyToSendStatus(requestid, status);
        //}


        //#region GeneratWorkOrder
        //private WorkOrder GenerateWorkOrder(DataRow row)
        //{

        //    WorkOrder wo = new WorkOrder();
        //    wo.WorkOrderID = ConversionTools.ConvertToInt64(row["WorkOrderID"], 0);
        //    wo.RequestID = ConversionTools.GetStringValue(row["RequestID"]);
        //    wo.ApplicationInfoTrackingID = ConversionTools.GetStringValue(row["ApplicationInfoTrackingID"]);
        //    wo.CaseNumber = ConversionTools.GetStringValue(row["CaseNo"]);
        //    wo.CopyInstruction = ConversionTools.GetStringValue(row["CopyInstruction"]).Trim();  // RequirementDetails
        //    wo.RequestorName = ConversionTools.GetStringValue(row["RequestorName"]).Trim();
        //    wo.RequestorEmail = ConversionTools.GetStringValue(row["RequestorEmail"]).Trim();
        //    //wo.re = Utility.GetStringValue(row["RequestorPhone"]);
        //    wo.RequestorCompanyName = ConversionTools.GetStringValue(row["RequestorCompanyName"]).Trim();
        //    wo.AgentName = ConversionTools.GetStringValue(row["AgentName"]).Trim();
        //    wo.AgentID = ConversionTools.GetStringValue(row["AgentID"]).Trim();
        //    wo.AgentPhone = ConversionTools.GetStringValue(row["AgentPhone"]).Trim();
        //    wo.AgentEmail = ConversionTools.GetStringValue(row["AgentEmail"]).Trim();
        //    wo.ReceiveDate = ConversionTools.GetStringValue(row["ReceiveDate"]).Trim();
        //    wo.ApplicantLastName = ConversionTools.GetStringValue(row["ApplicantLastName"]).Trim();
        //    wo.ApplicantFirstName = ConversionTools.GetStringValue(row["ApplicantFirstName"]).Trim();
        //    wo.ApplicantMiddleInitial = ConversionTools.GetStringValue(row["ApplicantMiddleInit"]).Trim();
        //    wo.ApplicantSSN = ConversionTools.GetNumberFromString(ConversionTools.GetStringValue(row["ApplicantSSN"])).Trim();
        //    wo.ApplicantAddress = ConversionTools.GetStringValue(row["ApplicantAddress"]).Trim();
        //    wo.ApplicantCity = ConversionTools.GetStringValue(row["ApplicantCity"]).Trim();
        //    wo.ApplicantDOB = ConversionTools.GetStringValue(row["ApplicantDOB"]).Trim();
        //    wo.ApplicantState = ConversionTools.GetStringValue(row["ApplicantStateCode"]).Trim();
        //    wo.ApplicantZip = ConversionTools.GetStringValue(row["ApplicantZipCode"]).Trim();
        //    wo.InsuranceCompany = ConversionTools.GetStringValue(row["InsuranceCompany"]).Trim();
        //    wo.PolicyNumber = ConversionTools.GetStringValue(row["PolicyNumber"]).Trim();
        //    wo.TransExeDate = ConversionTools.GetStringValue(row["TransExeDate"]).Trim();
        //    wo.TransExeTime = ConversionTools.GetStringValue(row["TransExeTime"]).Trim();
        //    wo.PolicyProductType = ConversionTools.GetStringValue(row["PolicyProductType"]).Trim();
        //    wo.PolicyProductTypeTC = ConversionTools.GetStringValue(row["PolicyProductTypeTC"]).Trim();
        //    wo.FaceAmount = ConversionTools.GetStringValue(row["FaceAmount"]).Trim();
        //    wo.ReqCodeTC = ConversionTools.GetStringValue(row["ReqCodeTC"]).Trim();
        //    wo.RequirementDetails = ConversionTools.GetStringValue("RequirementDetails").Trim();
        //    wo.RequestedDate = ConversionTools.GetStringValue(row["RequestedDate"]).Trim();
        //    wo.ScheduledDate = ConversionTools.GetStringValue(row["ScheduledDate"]).Trim();
        //    wo.RequirementAcctNum = ConversionTools.GetStringValue(row["RequirementAcctNum"]).Trim();
        //    wo.EISWorkOrder = ConversionTools.GetStringValue(row["EISWorkorder"]).Trim();
        //    wo.EISWorkOrderStatus = ConversionTools.GetStringValue(row["EISWorkOrderStatus"]).Trim();
        //    wo.EISUpdated = ConversionTools.GetStringValue(row["EISLastUpdated"]).Trim();
        //    wo.ErrorMessage = ConversionTools.GetStringValue(row["ErrorMessage"]).Trim();
        //    wo.Created = ConversionTools.GetStringValue(row["Created"]).Trim();
        //    wo.CompletedImageLocation = ConversionTools.GetStringValue(row["CompletedImageLocation"]).Trim();
        //    wo.RequirementInfoUniqueID = ConversionTools.GetStringValue(row["RequirementInfoUniqueID"]).Trim();
        //    wo.FormalAppInd = ConversionTools.GetStringValue(row["FormalAppInd"]).Trim();
        //    wo.EISWorkOrderStatusNote = ConversionTools.GetStringValue(row["EISWorkOrderStatusNote"]).Trim();
        //    wo.HoldingTypeCode = ConversionTools.GetStringValue(row["HoldingTypeCode"]).Trim();
        //    wo.HoldingTypeCodeTC = ConversionTools.GetStringValue(row["HoldingTypeCodeTC"]).Trim();
        //    wo.TransRefGUID = ConversionTools.GetStringValue(row["TransRefGUID"]);
        //    wo.IsExam = ConversionTools.GetStringValue(row["IsExam"]);


        //    return wo;
        //}
        //#endregion





        //private string ProcessComplete(WorkOrder wo)
        //{
        //    TXLifeResponse1122 response = new TXLifeResponse1122(wo);
        //    XmlDocument doc = response.Create();
        //    if (doc == null)
        //    {
        //        // something is wrong
        //    }

        //    return doc.InnerXml;
        //}


        //private string ProcessPending(WorkOrder wo)
        //{
        //    TXLifeResponse1122 response = new TXLifeResponse1122(wo);
        //    XmlDocument doc = response.Create();
           
        //    if (doc == null)
        //    {
        //        // something is wrong
        //    }

        //    return doc.InnerXml;
        //}


        public virtual WorkOrder ProcessXMLFile(string accordxml)
        {
            //List<DCTXLifeResponse> txlifelist = new List<DCTXLifeResponse>();
            XmlDocument dom = new XmlDocument();



            string newAccordxml = new XMLUtility().RemoveNamespacesFromXML(accordxml);

            // we need to remove all the namespaces to load the xml faster and we don't have to worry about SelectNodes/SelectSingleNodes issue
            // string xml = RemoveAllNamespaces(accordxml);

            dom.LoadXml(newAccordxml);
         
            //XmlReader reader =XmlReader.Create(new System.IO.StringReader(accordxml));


            //XDocument xdoc = XDocument.Load(reader);

            XmlNamespaceManager  nsMgr = new XmlNamespaceManager(dom.NameTable);
         //   nsMgr.RemoveNamespace("xmlns", dom.DocumentElement.NamespaceURI);
           // nsMgr.AddNamespace("eisnfp", "http://ACORD.org/Standards/Life/2");
            //nsMgr.AddNamespace("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            //nsMgr.AddNamespace("xsi:schemalocation", "http://ACORD.org/Standards/Life/2 TXLife2.11.00enum.xsd");


            // the NodeList contains all TXLifeRequest

            XmlNodeList nodelist = dom.DocumentElement.SelectNodes("//TXLife/TXLifeRequest");

            List<WorkOrder> list = ProcessRequestObject(nodelist, nsMgr);

            if (list.Count == 0) return null;

            return list[0];

            //txlifelist = SaveList(list);

            //return txlifelist;
        }

        
        //#region SAVE LIST
        //private List<DCTXLifeResponse> SaveList(List<WorkOrder> list)
        //{
        //    List<DCTXLifeResponse> respList = new List<DCTXLifeResponse>();
        //    //}
        //    DCTXLifeResponse resp;

        //    string filename = string.Empty;
          
        //    // there should only be 1 count
        //    foreach (WorkOrder wo in list)
        //    {
              
        //        TXLifeResponse121 temp = new TXLifeResponse121(wo);
        //        resp = new DCTXLifeResponse();
        //        //doc = temp.Create();

        //        if (wo.IsCancelled)
        //        {
        //            if (wo.ErrorMessage.Length == 0)  // everything is good
        //                resp.Success = true;
        //            else
        //                resp.Success = false;
        //        }
        //        else // it is not a Cancell order
        //        {

        //            if (wo.ErrorMessage.Length == 0)
        //            {
        //                resp.Success = true;
                        
        //            }
        //            else
        //                resp.Success = false;
        
        //        }
                
        //        resp.TransRefGUID = wo.TransRefGUID;
        //        resp.ResultXML = "";
        //        respList.Add(resp);
                
        //    }

        //    return respList;
        //}

        //#endregion SAVELIST

        public virtual BusinessEntities.APSIncomingEntity WorkOrderMapper (WorkOrder wo)
        {
            BusinessEntities.APSIncomingEntity entity = new BusinessEntities.APSIncomingEntity();
            var minimumdate = new DateTime(1900, 1, 1);
            entity.ACORDVendorCode = "";
            entity.CompanyID = 1;
            //entity.CompletedImageLocation = wo.CompletedImageLocation;
            entity.CopyInstructions = ConversionTools.CheckForNull(wo.Note, "");
            entity.Created = DateTime.Now;
            entity.DoctorCity = ConversionTools.CheckForNull(wo.FacilityCity, "");
            entity.DoctorCountry = ConversionTools.CheckForNull(wo.DoctorCountry, "");
            entity.DoctorFacility = ConversionTools.CheckForNull(wo.DoctorOrFacilityName, "");
            entity.DoctorFax = ConversionTools.CheckForNull(wo.FacilityFax, "");
            entity.DoctorFirstName = ConversionTools.CheckForNull(wo.DoctorFirstName, "");
            entity.DoctorLastName = ConversionTools.CheckForNull(wo.DoctorLastName, "");
            entity.DoctorPhone = ConversionTools.CheckForNull(wo.FacilityPhone, "");
            entity.DoctorPhoneExtension = ConversionTools.CheckForNull(wo.FacilityPhoneExt, "");
            //entity.DoctorSalutation = wo.facs
            entity.DoctorState = ConversionTools.CheckForNull(wo.FacilityState, "");
            entity.DoctorZipCode = ConversionTools.CheckForNull(wo.FacilityZipCode, "");
            entity.DoctorStreet1 = ConversionTools.CheckForNull(wo.FacilityAddress, "");
            //entity.DoctorStreet2 = wo.
            //entity.EISStatus = wo.EISWorkOrderStatus;
            //entity.EISStatusDate = 
            entity.ErrorMessage = ConversionTools.CheckForNull(wo.ErrorMessage, "");
            entity.HIPPALocation = ConversionTools.CheckForNull(wo.AttachmentLocation, "");
            if (wo.ErrorMessage.Length > 0)
                entity.IsError = true;
            else
                entity.IsError = false;
            entity.IsTestOnly = false;
            
            if (!entity.IsTestOnly && !entity.IsError)
                entity.LoadToEISMain = true;
            else
                entity.LoadToEISMain = false;
            entity.OrderDate = ConversionTools.ConvertToDate(wo.TransExeDate, DateTime.MinValue);
            if (entity.OrderDate < minimumdate)
                entity.OrderDate = ConversionTools.ConvertToDate(wo.ReceiveDate, DateTime.MinValue);
            if (entity.OrderDate < minimumdate)
                entity.OrderDate = DateTime.Now;

            //entity.PatientBusinessCity = wo.ApplicantCity;
            entity.PatientCity = ConversionTools.CheckForNull(wo.ApplicantCity, "");
            //            entity.PatientCountry = wo.
            if (wo.ApplicantDOB.Length > 0)
            {
                var tempdate = ConversionTools.ConvertToDate(wo.ApplicantDOB,DateTime.MinValue);
                if (tempdate > minimumdate)
                    entity.PatientDOB = tempdate;
            }

            entity.PatientEmail = ConversionTools.CheckForNull(wo.PatientEmail,"");
            entity.PatientFirstName = ConversionTools.CheckForNull(wo.ApplicantFirstName, "");
            entity.PatientLastName = ConversionTools.CheckForNull(wo.ApplicantLastName, "");
            entity.PatientMiddleName = ConversionTools.CheckForNull(wo.ApplicantMiddleInitial,"");
            entity.PatientPhone1 = ConversionTools.CheckForNull(wo.PatientPhone1,"");
            
            entity.PatientPhone2 = ConversionTools.CheckForNull(wo.PatientPhone2, "");
            entity.PatientGender = ConversionTools.CheckForNull(wo.Gender, "");
            entity.PatientSSN = ConversionTools.CheckForNull(wo.ApplicantSSN, "");
            entity.PatientState = ConversionTools.CheckForNull(wo.ApplicantState, "");
            entity.PatientStreet1 = ConversionTools.CheckForNull(wo.ApplicantAddress,"");
            //entity.PatientStreet2 = wo.
            entity.PatientZipCode = ConversionTools.CheckForNull(wo.ApplicantZip, "");
            entity.PolicyAmcount = ConversionTools.ConvertToDecimal(wo.FaceAmount, 0);
            entity.PolicyNumber = ConversionTools.CheckForNull(wo.PolicyNumber, "");
            entity.RequestorAddess = ConversionTools.CheckForNull(wo.RequestorAddess, "");
            entity.RequestorCity = ConversionTools.CheckForNull(wo.RequestorCity, "");
            entity.RequestorEmail = ConversionTools.CheckForNull(wo.RequestorEmail, "");
            //entity.RequestorFax = wo.
            entity.RequestorFirstName = ConversionTools.CheckForNull(wo.RequestorFirstName, "");
            entity.RequestorLastName = ConversionTools.CheckForNull(wo.RequestorLastName, "");
            entity.RequestorPhone = ConversionTools.CheckForNull(wo.RequestorPhone, "");
            entity.RequestorPhoneExt = ConversionTools.CheckForNull(wo.RequestorPhoneExt, "");
            entity.RequestorState = ConversionTools.CheckForNull(wo.RequestorState, "");
            entity.RequestorZipCode = ConversionTools.CheckForNull(wo.RequestorZipcode, "");
            entity.RequirementAcctNum = ConversionTools.CheckForNull(wo.RequirementAcctNum, "");
            entity.RequirementInfoUniqueID = ConversionTools.CheckForNull(wo.RequirementInfoUniqueID, "");
            //entity.SendEmail = wo.SendAlert;
            entity.SendToCompany = false;
            entity.TrackingID = ConversionTools.CheckForNull(wo.ApplicationInfoTrackingID, "");
            entity.TransRefGUID = ConversionTools.CheckForNull(wo.TransRefGUID, "");
            entity.WritingAgentAddress = ConversionTools.CheckForNull(wo.WritingAgentAddress, "");
            entity.WritingAgentCity = ConversionTools.CheckForNull(wo.WritingAgentCity,"");
            entity.WritingAgentEmail = ConversionTools.CheckForNull(wo.WritingAgentEmail, "");
            entity.WritingAgentFirstName = ConversionTools.CheckForNull(wo.WritingAgentFirstName, "");
            entity.WritingAgentLastName = ConversionTools.CheckForNull(wo.WritingAgentLastName, "");
            entity.WritingAgentPhone = ConversionTools.CheckForNull(wo.WritingAgentPhone, "");
            entity.WritingAgentPhoneExt = ConversionTools.CheckForNull(wo.WritingAgentPhoneExt, "");
            entity.WritingAgentState = ConversionTools.CheckForNull(wo.WritingAgentState, "");
            entity.WritingAgentZipCode = ConversionTools.CheckForNull(wo.WritingAgentZipCode, "");
            entity.CarrierCode = ConversionTools.CheckForNull(wo.CarrierCode, "");
            entity.AgencyCarrierCode = ConversionTools.CheckForNull(wo.AgencyCarrierCode, "");
            entity.CompanyProducerID = ConversionTools.CheckForNull(wo.CompanyProducerID, "");
            entity.IsUrgent = wo.IsUrgent;
            // update entity 05/16/2020
            entity.DestinationCode = ConversionTools.CheckForNull(wo.InsuranceCompany, "");
            return entity;
        }



        private WorkOrder CreateWorkOrder(XmlNode tmpNode, string RequirementInfoID)
        {
            WorkOrder wo = new WorkOrder();
            TXLifeRequest alife;
            TXLifeRequestOLifEHolding holding;
            TXLifeRequestOLifEHoldingPolicyLife life;
            TXLifeTXLifeRequestOLifEHoldingPolicyRequirementInfo req;
            TXLifeRequestOLifEHoldingPolicyApplicationInfo appinfo;
            TXLifeRequestOLifEHoldingPolicy policy;
            TXLifeRequestOLifEParty party;
            TXLifeTXLifeRequestOLifEPartyPerson person;
          //  TXLifeRequestOLifEHoldingPolicyRequirementInfoAttachment attachment;  // for HIPPA
            TXLifeRequestOLifERelation relation = new TXLifeRequestOLifERelation();
            TXLifeTXLifeRequestOLifEPartyPhone phone;
            TXLifeTXLifeRequestOLifEPartyAddress address;
            TXLifeTXLifeRequestOLifEPartyEmailAddress email;
           // TxLifeUserAuthRequest userinforequest;

            //userinforequest = new TxLifeUserAuthRequest();
            //userinforequest.ProcessXML(tmpNode);
            //ServiceUserName = userinforequest.Username;
            //ServicePassword = userinforequest.Password;

            alife = new TXLifeRequest();
            alife.ProcessXML(tmpNode);
            // get transaction time

            wo.TransExeDate = alife.TransExeDate;
            wo.TransExeTime = alife.TransExeTime;
            wo.TransRefGUID = alife.TransRefGUID;
            wo.TestOnly = alife.TestIndicator;
            wo.TransCode = alife.TransCode;

            if (alife.TransModeCode == "6" || alife.TransMode.Trim().ToUpper() == "CANCELLATION")
            {
                // a cancellation request has been sent
                wo.IsCancelled = true;
            }
            else if (alife.TransCode == "4" || alife.TransMode.Trim().ToUpper() == "UPDATE")
            {
                wo.IsUpdate = true;
            }


            // ******************************************************************************************************************************************
            // RELATION NODE (10/28/2013) - eService WANTS TO SAVE ALL THE VALUES BACK TO THEM
            // ******************************************************************************************************************************************
            relation.ProcessXML(tmpNode);
            wo.RelationNodeXML = relation.RelationNodeXML;
            relation = null;
            


            // ******************************************************************************************************************************************
            // WORK ORDER NOTE  - Please see requirementDetails below
            // ******************************************************************************************************************************************
            //attachment = new TXLifeRequestOLifEHoldingAttachment();
            //attachment.ProcessXML(tmpNode);
            //wo.Note = attachment.Description;

            alife = null;

            // requestor id  and application id
            req = new TXLifeTXLifeRequestOLifEHoldingPolicyRequirementInfo();
            req.ID = RequirementInfoID;  // get the requirement based on the id attribute
            req.ProcessXML(tmpNode);

            // requestor information from requestor id
            wo.RequesterPartyID = req.RequesterPartyID;
            wo.AppliesToPartyID = req.AppliesToPartyID;
            wo.FullfillerPartyID = req.FulfillerPartyID;
            wo.RequestorContactPartyID = req.RequestorContactPartyID;
            wo.RequestID = wo.RequirementInfoUniqueID = req.RequirementInfoUniqueID; // this is uinque for each request
            
            if (!string.IsNullOrEmpty(req.RequirementInfoUniqueID))
                wo.RequirementInfoUniqueID = req.RequirementInfoUniqueID; // this is uinque for each request

            wo.CopyInstruction = req.ReqCode;
            wo.ReqCodeTC = req.ReqCodeTC;
            if (wo.CheckIfItIsExam(wo.ReqCodeTC))
                wo.IsExam = "1";  // exam indicator
            if (!string.IsNullOrEmpty(req.Priority) && req.Priority.Trim().ToUpper() == "RUSH")
                wo.IsUrgent = true;
            
            wo.NoteToEIS = ConversionTools.CheckForNull(req.DeliveryInstructionDesc, "");
            wo.MaxFee = ConversionTools.CheckForNull(req.FeeCapAmt, "");
            wo.UnitCode = ConversionTools.CheckForNull(req.UnitCode, "");

            //wo.RequirementDetails = req.RequirementDetails;
            wo.Note = req.RequirementDetails;
            wo.RequestedDate = req.RequestedDate;
            wo.ScheduledDate = req.ScheduledDate;
            wo.RequirementAcctNum = req.RequirementAcctNum;
            wo.ApplicationInfoTrackingID = req.ApplicationInfoTrackingID;
            //  wo.RequestorCompanyName = req.ReleasePartyOrgCode;


            // clean up req object
            req = null;

            wo.xml = "";


            // **************************************** HOLDING INFORMATION ********************************
            holding = new TXLifeRequestOLifEHolding();
            holding.ProcessXML(tmpNode);
            wo.HoldingTypeCode = holding.HoldingTypeCode;
            wo.HoldingTypeCodeTC = holding.HoldingTypeCodeTC;

            // clean up
            holding = null;


            // ********************************************** LIFE INFORMATION ********************************
            life = new TXLifeRequestOLifEHoldingPolicyLife();
            life.ProcessXML(tmpNode);
            wo.FaceAmount = life.FaceAmt;
            life = null;


            // ****************************************INSURANCE INFORMATION *********************************
            policy = new TXLifeRequestOLifEHoldingPolicy();
            policy.ProcessXML(tmpNode);

            wo.CarrierPartyID = policy.CarrierPartyID;
            wo.PolicyNumber = policy.PolNumber;
            wo.PolicyProductType = policy.ProductType;
            wo.PolicyProductTypeTC = policy.ProductTypeTC;

            // use the party object to get insurance name
            party = new TXLifeRequestOLifEParty();
            party.PartyID = wo.CarrierPartyID;   // get insurance party information
            party.ProcessXML(tmpNode);

            wo.BillCode = policy.BillCode;
            if (party.AbbrOrgName != "")
                // append the abbreviated name to full name
                wo.InsuranceCompany = party.FullName;
            else
                wo.InsuranceCompany = party.FullName;  // full name alone
            policy = null;

            //// **************************************** APPLICATION INFORMATION *********************************
            appinfo = new TXLifeRequestOLifEHoldingPolicyApplicationInfo();
            appinfo.ProcessXML(tmpNode);
            wo.FormalAppInd = appinfo.FormalAppInd;




            // **************************************** PERSONAL INFORMATION OF THE REQUIREMENT ****************************************
            

            // get applicant SSN and 
            party = new TXLifeRequestOLifEParty();
            party.PartyID = wo.AppliesToPartyID;
            party.ProcessXML(tmpNode);

            // only if the type is a person
            if (party.PartyTypeCode == "1")
            {
                wo.ApplicantSSN = party.GovtID;

                // get applicant personal information
                person = new TXLifeTXLifeRequestOLifEPartyPerson();
                person.PartyID = wo.AppliesToPartyID;
                person.ProcessXML(tmpNode);

                // assign personal info to Work order object
                wo.ApplicantFirstName = person.FirstName;
                wo.ApplicantLastName = person.LastName;
                wo.ApplicantMiddleInitial = person.MiddleName;
                wo.ApplicantDOB = person.BirthDate;
                wo.Gender = person.Gender;
                person = null;	// done with person

                // address information
                address = new TXLifeTXLifeRequestOLifEPartyAddress();
                address.AddressTypeToRetrieve = EIS.Applications.Common.Constants.AddressTypeCode_Residence;	// residence type code only
                address.PartyID = wo.AppliesToPartyID;
                address.ProcessXML(tmpNode);

                // assign address information to Work Order object for applicant
                wo.ApplicantAddress = address.Line1;
                wo.ApplicantCity = address.City;
                wo.ApplicantState = address.AddressStateTC;
                wo.ApplicantZip = address.Zip;

                // if residence is not available , get other address
                if (wo.ApplicantAddress.Length == 0)
                {
                    address.AddressTypeToRetrieve = EIS.Applications.Common.Constants.AddressTypeCode_Other;	// residence type code only
                    address.ProcessXML(tmpNode);


                    // assign address information to Work Order object for applicant
                    wo.ApplicantAddress = address.Line1;
                    wo.ApplicantCity = address.City;
                    wo.ApplicantState = address.AddressStateTC;
                    wo.ApplicantZip = address.Zip;

                }


                // if other is not available , get unknow address
                if (wo.ApplicantAddress.Length == 0)
                {
                    address.AddressTypeToRetrieve = EIS.Applications.Common.Constants.AddressTypeCode_Unknown;	// residence type code only
                    address.ProcessXML(tmpNode);


                    // assign address information to Work Order object for applicant
                    wo.ApplicantAddress = address.Line1;
                    wo.ApplicantCity = address.City;
                    wo.ApplicantState = address.AddressStateTC;
                    wo.ApplicantZip = address.Zip;

                }

                // if unknow is not available , get business address
                if (wo.ApplicantAddress.Length == 0)
                {
                    address.AddressTypeToRetrieve = EIS.Applications.Common.Constants.AddressTypeCode_Bussiness;	// residence type code only
                    address.ProcessXML(tmpNode);


                    // assign address information to Work Order object for applicant
                    wo.ApplicantAddress = address.Line1;
                    wo.ApplicantCity = address.City;
                    wo.ApplicantState = address.AddressStateTC;
                    wo.ApplicantZip = address.Zip;

                }

                // if business is not available , get work address
                if (wo.ApplicantAddress.Length == 0)
                {
                    address.AddressTypeToRetrieve = EIS.Applications.Common.Constants.AddressTypeCode_Work;	// residence type code only
                    address.ProcessXML(tmpNode);


                    // assign address information to Work Order object for applicant
                    wo.ApplicantAddress = address.Line1;
                    wo.ApplicantCity = address.City;
                    wo.ApplicantState = address.AddressStateTC;
                    wo.ApplicantZip = address.Zip;

                }

                // clean up address
                address = null;
                /**********************************************************************************
                 * PATIENT PHONE NUMBER 
                 **********************************************************************************/

                phone = new TXLifeTXLifeRequestOLifEPartyPhone();
                phone.PartyID = wo.AppliesToPartyID;

                
                phone.PartyID = wo.AppliesToPartyID;
                phone.PhoneTypeToRetrieve = "";
                phone.ProcessXML(tmpNode);
                wo.PatientPhone1 = phone.AreaCode + phone.DialNumber;


                // if phone is empty, get other phone
                if (wo.PatientPhone1.Length == 0)
                {
                    phone.PartyID = wo.AppliesToPartyID;
                    phone.PhoneTypeToRetrieve = "1";
                    phone.ProcessXML(tmpNode);
                    wo.PatientPhone1 = phone.AreaCode + phone.DialNumber;

                }

                if (wo.PatientPhone1.StartsWith("000"))
                    wo.PatientPhone1 = "";


                // if phone is empty, get other phone
                if (wo.PatientPhone1.Length == 0)
                {
                    phone.PartyID = wo.AppliesToPartyID;
                    phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Business;
                    phone.ProcessXML(tmpNode);
                    wo.PatientPhone1 = phone.AreaCode + phone.DialNumber;

                }

                if (wo.PatientPhone1.StartsWith("000"))
                    wo.PatientPhone1 = "";

                // if phone is empty, get other phone
                if (wo.PatientPhone1.Length == 0)
                {
                    phone.PartyID = wo.AppliesToPartyID;
                    phone.PhoneTypeToRetrieve = "1";  // residence
                    phone.ProcessXML(tmpNode);
                    wo.PatientPhone1 = phone.AreaCode + phone.DialNumber;

                }

                if (wo.PatientPhone1.StartsWith("000"))
                    wo.PatientPhone1 = "";

                // if phone is empty, get other phone
                if (wo.PatientPhone1.Length == 0)
                {
                    phone.PartyID = wo.AppliesToPartyID;
                    phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Other;
                    phone.ProcessXML(tmpNode);
                    wo.PatientPhone1 = phone.AreaCode + phone.DialNumber;

                }

                if (wo.PatientPhone1.StartsWith("000"))
                    wo.PatientPhone1 = "";

                // if phone is empty, get unknown phone
                if (wo.PatientPhone1.Length == 0)
                {
                    phone.PartyID = wo.AppliesToPartyID;
                    phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Unknown;
                    phone.ProcessXML(tmpNode);
                    wo.PatientPhone1 = phone.AreaCode + phone.DialNumber;

                }

                if (wo.PatientPhone1.StartsWith("000"))
                    wo.PatientPhone1 = "";

                // if phone is empty, get mobile phone
                if (wo.PatientPhone1.Length == 0)
                {
                    phone.PartyID = wo.AppliesToPartyID;
                    phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Mobile;
                    phone.ProcessXML(tmpNode);
                    wo.PatientPhone1 = phone.AreaCode + phone.DialNumber;

                }

                if (wo.PatientPhone1.StartsWith("000"))
                    wo.PatientPhone1 = "";



                // if phone is empty, get regional phone
                if (wo.PatientPhone1.Length == 0)
                {
                    phone.PartyID = wo.AppliesToPartyID;
                    phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_RegionalOffice;
                    phone.ProcessXML(tmpNode);
                    wo.PatientPhone1 = phone.AreaCode + phone.DialNumber;
                }

                wo.PatientPhone1 = wo.PatientPhone1.Replace("-", "");
                if (wo.PatientPhone1.StartsWith("000"))
                    wo.PatientPhone1 = "";


                // look for Phone 2 if phone 1 exists
                if (wo.PatientPhone1.Length > 0)
                {

                    // if phone is empty, get other phone
                    if (wo.PatientPhone2.Length == 0)
                    {
                        phone.PartyID = wo.AppliesToPartyID;
                        phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Business;
                        phone.ProcessXML(tmpNode);
                        wo.PatientPhone2 = phone.AreaCode + phone.DialNumber;

                    }

                    if (wo.PatientPhone2.StartsWith("000"))
                        wo.PatientPhone2 = "";

                    // if phone is empty, get other phone
                    if (wo.PatientPhone2 == wo.PatientPhone1 || wo.PatientPhone2.Length == 0)
                    {
                        phone.PartyID = wo.AppliesToPartyID;
                        phone.PhoneTypeToRetrieve = "1";  // residence
                        phone.ProcessXML(tmpNode);
                        wo.PatientPhone2 = phone.AreaCode + phone.DialNumber;

                    }

                    if (wo.PatientPhone2.StartsWith("000"))
                        wo.PatientPhone2 = "";

                    // if phone is empty, get other phone
                    if (wo.PatientPhone2 == wo.PatientPhone1 || wo.PatientPhone2.Length == 0)
                    {
                        phone.PartyID = wo.AppliesToPartyID;
                        phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Other;
                        phone.ProcessXML(tmpNode);
                        wo.PatientPhone2 = phone.AreaCode + phone.DialNumber;

                    }

                    if (wo.PatientPhone2.StartsWith("000"))
                        wo.PatientPhone2 = "";

                    // if phone is empty, get unknown phone
                    if (wo.PatientPhone2 == wo.PatientPhone1 || wo.PatientPhone2.Length == 0)
                    {
                        phone.PartyID = wo.AppliesToPartyID;
                        phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Unknown;
                        phone.ProcessXML(tmpNode);
                        wo.PatientPhone2 = phone.AreaCode + phone.DialNumber;

                    }

                    if (wo.PatientPhone2.StartsWith("000"))
                        wo.PatientPhone2 = "";

                    // if phone is empty, get mobile phone
                    if (wo.PatientPhone2 == wo.PatientPhone1 || wo.PatientPhone2.Length == 0)
                    {
                        phone.PartyID = wo.AppliesToPartyID;
                        phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Mobile;
                        phone.ProcessXML(tmpNode);
                        wo.PatientPhone2 = phone.AreaCode + phone.DialNumber;

                    }

                    if (wo.PatientPhone2.StartsWith("000"))
                        wo.PatientPhone2 = "";



                    // if phone is empty, get regional phone
                    if (wo.PatientPhone2 == wo.PatientPhone1 || wo.PatientPhone2.Length == 0)
                    {
                        phone.PartyID = wo.AppliesToPartyID;
                        phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_RegionalOffice;
                        phone.ProcessXML(tmpNode);
                        wo.PatientPhone2 = phone.AreaCode + phone.DialNumber;
                    }

                    if (wo.PatientPhone2.StartsWith("000"))
                        wo.PatientPhone2 = "";
                    if (wo.PatientPhone2 == wo.PatientPhone1)
                        wo.PatientPhone2 = "";

                    wo.PatientPhone2 = wo.PatientPhone2.Replace("-", "");
                }


                // clean up phone
                phone = null;


                // get application contact preferred email address
                email = new TXLifeTXLifeRequestOLifEPartyEmailAddress();
                email.PartyID = wo.AppliesToPartyID;
                email.PreferredPrimaryAddress = true;  // get preferred

                if (email.ProcessXML(tmpNode))
                    wo.PatientEmail = email.Email;
                else // preferred email doesn't exist, just get any email
                {
                    email.PreferredPrimaryAddress = false;
                    email.ProcessXML(tmpNode);
                    wo.PatientEmail = email.Email;
                }
                // clean up email
                email = null;

            }
            // clean up party object
            party = null;



            // **************************************** CARRIER information INFORMATION OF THE REQUIREMENT ****************************************
            //  TXLifeRequestOLifEPartyOrganization agency = new TXLifeRequestOLifEPartyOrganization();
            //  agency.ParseXML(tmpNode);
            //  wo.RequestorCompanyName = agency.OrgCode;   // SkyWire ideal to put use  orgcode for.
            //  agency = null;



            // **************************************** Doctor information INFORMATION OF THE REQUIREMENT ****************************************
            party = new TXLifeRequestOLifEParty();
            TXLifeRequestOLifERelationDoctor relationdoctor = new TXLifeRequestOLifERelationDoctor();

            relationdoctor.ProcessXML(tmpNode);

            // we have physician or facility information
            string doctorPartyID = party.PartyID = relationdoctor.RelatedObjectID;

            


            party.ProcessXML(tmpNode);

            if (party.PartyTypeCode == "1")  // physisican or primary physician
            {
                // get applicant personal information
                person = new TXLifeTXLifeRequestOLifEPartyPerson();
                person.PartyID = doctorPartyID;
                person.ProcessXML(tmpNode);

                // assign personal info to Work order object
                //wo.DoctorOrFacilityName = person.FirstName + " " + person.MiddleName + " " + person.LastName;
                wo.DoctorFirstName = person.FirstName;
                wo.DoctorLastName = person.LastName;
                //wo.DoctorOrFacilityName = wo.DoctorOrFacilityName.Replace("  ", " ").Trim();
                person = null;
            }

            // 11/13/2015
            // Transamerica sends us PartyTypeCode tc=1 but which is Person but the Person node is blank
            // let try to get it from FullName instead
            if (wo.DoctorOrFacilityName.Length == 0)
                wo.DoctorOrFacilityName = party.FullName.Trim();  // name of facility is in the party.FullName


            party = null;


            // get doctor or facility information


            // address information for the doctor
            address = new TXLifeTXLifeRequestOLifEPartyAddress();
            address.AddressTypeToRetrieve = EIS.Applications.Common.Constants.AddressTypeCode_Bussiness;	// business type code only
            address.PartyID = doctorPartyID;
            address.ProcessXML(tmpNode);

            // assig address information to Work Order object for doctor
            wo.FacilityAddress = address.Line1;
            wo.FacilityCity = address.City;
            var statecodetc = ConversionTools.ConvertToInt32(address.AddressStateTC, 0);
            if (statecodetc > 0)
            {
                var stateAbbr = Config.StateCodeList.Where(p => p.Code == statecodetc).FirstOrDefault();
                if (stateAbbr != null)
                    wo.FacilityState = stateAbbr.Description;
            }
            else
                wo.FacilityState = address.AddressStateTC;
            
            wo.FacilityZipCode = address.Zip;


            // we have no address for mailing
            if (wo.FacilityAddress == "" || wo.FacilityCity == "")
            {
                // let us get other address
                address.AddressTypeToRetrieve = EIS.Applications.Common.Constants.AddressTypeCode_Mailing;	// other AddressTypeCode_Mailing type code 
                address.ProcessXML(tmpNode);

                // assig address information to Work Order object for applicant
                wo.FacilityAddress = address.Line1;
                wo.FacilityCity = address.City;
                statecodetc = ConversionTools.ConvertToInt32(address.AddressStateTC, 0);
                if (statecodetc > 0)
                {
                    var stateAbbr = Config.StateCodeList.Where(p => p.Code == statecodetc).FirstOrDefault();
                    if (stateAbbr != null)
                        wo.FacilityState = stateAbbr.Description;
                }

                else
                    wo.FacilityState = address.AddressStateTC;
                
                wo.FacilityZipCode = address.Zip;
            }

            // we have no address for business
            if (wo.FacilityAddress == "" || wo.FacilityCity == "")
            {
                // let us get other address
                address.AddressTypeToRetrieve = EIS.Applications.Common.Constants.AddressTypeCode_Other;	// other address type code 
                address.ProcessXML(tmpNode);

                // assig address information to Work Order object for applicant
                wo.FacilityAddress = address.Line1;
                wo.FacilityCity = address.City;
                statecodetc = ConversionTools.ConvertToInt32(address.AddressStateTC, 0);
                if (statecodetc > 0)
                {
                    var stateAbbr = Config.StateCodeList.Where(p => p.Code == statecodetc).FirstOrDefault();
                    if (stateAbbr != null)
                        wo.FacilityState = stateAbbr.Description;
                }
                else
                    wo.FacilityState = address.AddressStateTC;
                wo.FacilityZipCode = address.Zip;
            }

            // we have no address still
            if (wo.FacilityAddress == "" || wo.FacilityCity == "")
            {
                // let us get other address
                address.AddressTypeToRetrieve = EIS.Applications.Common.Constants.AddressTypeCode_Unknown;	// unknown
                address.ProcessXML(tmpNode);

                // assig address information to Work Order object for applicant
                wo.FacilityAddress = address.Line1;
                wo.FacilityCity = address.City;
                statecodetc = ConversionTools.ConvertToInt32(address.AddressStateTC, 0);
                if (statecodetc > 0)
                {
                    var stateAbbr = Config.StateCodeList.Where(p => p.Code == statecodetc).FirstOrDefault();
                    if (stateAbbr != null)
                        wo.FacilityState = stateAbbr.Description;
                }
                else
                    wo.FacilityState = address.AddressStateTC;
                
                wo.FacilityZipCode = address.Zip;
            }


            // we have no address still
            if (wo.FacilityAddress == "" || wo.FacilityCity == "")
            {
                // let us get other address
                address.AddressTypeToRetrieve = EIS.Applications.Common.Constants.AddressTypeCode_RegionalOffice;	// regional office
                address.ProcessXML(tmpNode);

                // assig address information to Work Order object for applicant
                wo.FacilityAddress = address.Line1;
                wo.FacilityCity = address.City;
                statecodetc = ConversionTools.ConvertToInt32(address.AddressStateTC, 0);
                if (statecodetc > 0)
                {
                    var stateAbbr = Config.StateCodeList.Where(p => p.Code == statecodetc).FirstOrDefault();
                    if (stateAbbr != null)
                        wo.FacilityState = stateAbbr.Description;
                }
                else
                    wo.FacilityState = address.AddressStateTC;
                wo.FacilityZipCode = address.Zip;
            }


            // we have no address still
            if (wo.FacilityAddress == "" || wo.FacilityCity == "")
            {
                // let us get other address
                address.AddressTypeToRetrieve = EIS.Applications.Common.Constants.AddressTypeCode_Work;	// work
                address.ProcessXML(tmpNode);

                // assig address information to Work Order object for applicant
                wo.FacilityAddress = address.Line1;
                wo.FacilityCity = address.City;
                statecodetc = ConversionTools.ConvertToInt32(address.AddressStateTC, 0);
                if (statecodetc > 0)
                {
                    var stateAbbr = Config.StateCodeList.Where(p => p.Code == statecodetc).FirstOrDefault();
                    if (stateAbbr != null)
                        wo.FacilityState = stateAbbr.Description;
                }
                else
                    wo.FacilityState = address.AddressStateTC;
                wo.FacilityZipCode = address.Zip;
            }

            // we have no address still
            if (wo.FacilityAddress == "" || wo.FacilityCity == "")
            {
                // let us get other address
                address.AddressTypeToRetrieve = EIS.Applications.Common.Constants.AddressTypeCode_Residence;	// residence
                address.ProcessXML(tmpNode);

                // assig address information to Work Order object for applicant
                wo.FacilityAddress = address.Line1;
                wo.FacilityCity = address.City;
                statecodetc = ConversionTools.ConvertToInt32(address.AddressStateTC, 0);
                if (statecodetc > 0)
                {
                    var stateAbbr = Config.StateCodeList.Where(p => p.Code == statecodetc).FirstOrDefault();
                    if (stateAbbr != null)
                        wo.FacilityState = stateAbbr.Description;
                }
                else
                    wo.FacilityState = address.AddressStateTC;
                wo.FacilityZipCode = address.Zip;
            }


            // clean up address
            address = null;

            // get  business phone number for the doctor
            phone = new TXLifeTXLifeRequestOLifEPartyPhone();
            phone.PartyID = doctorPartyID;
            phone.PhoneTypeToRetrieve = ""; // no phone type
            phone.ProcessXML(tmpNode);
            wo.FacilityPhone = phone.AreaCode + phone.DialNumber;
            wo.FacilityPhoneExt = phone.PhoneExt;

            // if phone is empty, get other phone
            if (wo.FacilityPhone.Length == 0)
            {
                phone.PartyID = doctorPartyID;
                phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Business;
                phone.ProcessXML(tmpNode);
                wo.FacilityPhone = phone.AreaCode + phone.DialNumber;
                wo.FacilityPhoneExt = phone.PhoneExt;
            }


            // if phone is empty, get other phone
            if (wo.FacilityPhone.Length == 0)
            {
                phone.PartyID = doctorPartyID;
                phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Other;
                phone.ProcessXML(tmpNode);
                wo.FacilityPhone = phone.AreaCode + phone.DialNumber;
                wo.FacilityPhoneExt = phone.PhoneExt;
            }


            // if phone is empty, get unknown phone
            if (wo.FacilityPhone.Length == 0)
            {
                phone.PartyID = doctorPartyID;
                phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Unknown;
                phone.ProcessXML(tmpNode);
                wo.FacilityPhone = phone.AreaCode + phone.DialNumber;
                wo.FacilityPhoneExt = phone.PhoneExt;
            }


            // if phone is empty, get mobile phone
            if (wo.FacilityPhone.Length == 0)
            {
                phone.PartyID = doctorPartyID;
                phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Mobile;
                phone.ProcessXML(tmpNode);
                wo.FacilityPhone = phone.AreaCode + phone.DialNumber;
                wo.FacilityPhoneExt = phone.PhoneExt;
            }


            // if phone is empty, get regional phone
            if (wo.FacilityPhone.Length == 0)
            {
                phone.PartyID = doctorPartyID;
                phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_RegionalOffice;
                phone.ProcessXML(tmpNode);
                wo.FacilityPhone = phone.AreaCode + phone.DialNumber;
                wo.FacilityPhoneExt = phone.PhoneExt;
            }



            // clean up phone
            phone = null;


            // get business fax number doctor
            phone = new TXLifeTXLifeRequestOLifEPartyPhone();
            phone.PartyID = doctorPartyID;
            phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_BusinessFax;
            phone.ProcessXML(tmpNode);
            wo.FacilityFax = phone.AreaCode + phone.DialNumber;
            // clean up phone
            phone = null;


            // **************************************** REQUESTER INFORMATION *********************************

            party = new TXLifeRequestOLifEParty();
            party.PartyID = wo.RequesterPartyID;
            party.ProcessXML(tmpNode);

            wo.RequestorCompanyName = party.FullName;
            if (wo.RequestorCompanyName.Length == 0)
                wo.RequestorCompanyName = party.AbbrOrgName;

            wo.AgencyCarrierCode = party.AgencyCarrierCode.Trim();
            wo.CompanyProducerID = party.CompanyProducerID.Trim();
            person = new TXLifeTXLifeRequestOLifEPartyPerson();

            // we need to know if requester has contact information
            if (wo.RequestorContactPartyID != wo.RequesterPartyID && wo.RequestorContactPartyID != "")  // they are not the same that means there is contact information
            {
                // get the requestor from the contact party id.. use FullName for NIIT only
               
                party.PartyID = wo.RequestorContactPartyID;
                party.ProcessXML(tmpNode);

                wo.RequestorName = party.FullName;  // should be the requestor name

                if (wo.RequestorName.Length == 0)
                {
                    // get requestor contact information from the person node inside the party..
                    // NIIT will not send this but in case they do.. this will handle it
                    person.PartyID = wo.RequestorContactPartyID;
                    if (!string.IsNullOrEmpty(person.PartyID))
                    {
                        if (person.ProcessXML(tmpNode) == true) // assign personal info to Work requestor name		
                        {
                            wo.RequestorFirstName = person.FirstName;
                            wo.RequestorLastName = person.LastName;
                            wo.RequestorName = person.FirstName + " " + person.MiddleName + " " + person.LastName;
                        }

                        phone = new TXLifeTXLifeRequestOLifEPartyPhone();
                        phone.PartyID = wo.RequestorContactPartyID;

                        phone.PhoneTypeToRetrieve = ""; // no phone type
                        phone.ProcessXML(tmpNode);
                        wo.RequestorPhone = phone.AreaCode + phone.DialNumber;
                        wo.RequestorPhoneExt = phone.PhoneExt;

                
                        if (wo.RequestorPhone == "" && !string.IsNullOrEmpty(phone.PartyID)) // get other phone number will do
                        {
                            phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Business; // default set to retrieve non preferred phone number
                            phone.ProcessXML(tmpNode);
                            wo.RequestorPhone = phone.AreaCode + phone.DialNumber;
                            wo.RequestorPhoneExt = phone.PhoneExt;
                        }

                        if (wo.RequestorPhone == "" && !string.IsNullOrEmpty(phone.PartyID)) // get other phone number will do
                        {
                            phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Other; // default set to retrieve non preferred phone number
                            phone.ProcessXML(tmpNode);
                            wo.RequestorPhone = phone.AreaCode + phone.DialNumber;
                            wo.RequestorPhoneExt = phone.PhoneExt;
                        }



                        if (wo.RequestorPhone == "" && !string.IsNullOrEmpty(phone.PartyID)) // get unknown phone number will do
                        {
                            phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Unknown; // default set to retrieve non preferred phone number
                            phone.ProcessXML(tmpNode);
                            wo.RequestorPhone = phone.AreaCode + phone.DialNumber;
                            wo.RequestorPhoneExt = phone.PhoneExt;
                        }


                        if (wo.RequestorPhone == "" && !string.IsNullOrEmpty(phone.PartyID)) // get any phone number will do
                        {
                            phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Mobile; // default set to retrieve non preferred phone number
                            phone.ProcessXML(tmpNode);
                            wo.RequestorPhone = phone.AreaCode + phone.DialNumber;
                            wo.RequestorPhoneExt = phone.PhoneExt;
                        }
                    }
                      

                }
                // get requester contact preferred email address
                email = new TXLifeTXLifeRequestOLifEPartyEmailAddress();
                email.PartyID = wo.RequestorContactPartyID;
                email.PreferredPrimaryAddress = true;  // get preferred

                if (email.ProcessXML(tmpNode))
                    wo.RequestorEmail = email.Email;
                else // preferred email doesn't exist, just get any email
                {
                    email.PreferredPrimaryAddress = false;
                    email.ProcessXML(tmpNode);
                    wo.RequestorEmail = email.Email;
                }
                // clean up email
                email = null;



            }
            else
            {
                // get requestor contact information
                person.PartyID = wo.RequesterPartyID;
                if (!string.IsNullOrEmpty(person.PartyID))
                {
                    if (person.ProcessXML(tmpNode) == true) // assign personal info to Work requestor name		
                    {
                        wo.RequestorFirstName = person.FirstName;
                        wo.RequestorLastName = person.LastName;
                        wo.RequestorName = person.FirstName + " " + person.MiddleName + " " + person.LastName;
                    }


                    // get agent business phone number
                    phone = new TXLifeTXLifeRequestOLifEPartyPhone();
                    phone.PartyID = wo.RequesterPartyID;

                    phone.PhoneTypeToRetrieve = ""; // no phone type
                    phone.ProcessXML(tmpNode);
                    wo.RequestorPhone = phone.AreaCode + phone.DialNumber;
                    wo.RequestorPhoneExt = phone.PhoneExt;

                    if (wo.RequestorPhone == "" && !string.IsNullOrEmpty(phone.PartyID)) // get other phone number will do
                    {
                        phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Business; // default set to retrieve non preferred phone number
                        phone.ProcessXML(tmpNode);
                        wo.RequestorPhone = phone.AreaCode + phone.DialNumber;
                        wo.RequestorPhoneExt = phone.PhoneExt;
                    }

                    if (wo.RequestorPhone == "" && !string.IsNullOrEmpty(phone.PartyID)) // get other phone number will do
                    {
                        phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Other; // default set to retrieve non preferred phone number
                        phone.ProcessXML(tmpNode);
                        wo.RequestorPhone = phone.AreaCode + phone.DialNumber;
                        wo.RequestorPhoneExt = phone.PhoneExt;
                    }



                    if (wo.RequestorPhone == "" && !string.IsNullOrEmpty(phone.PartyID)) // get unknown phone number will do
                    {
                        phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Unknown; // default set to retrieve non preferred phone number
                        phone.ProcessXML(tmpNode);
                        wo.RequestorPhone = phone.AreaCode + phone.DialNumber;
                        wo.RequestorPhoneExt = phone.PhoneExt;
                    }


                    if (wo.RequestorPhone == "" && !string.IsNullOrEmpty(phone.PartyID)) // get any phone number will do
                    {
                        phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Mobile; // default set to retrieve non preferred phone number
                        phone.ProcessXML(tmpNode);
                        wo.RequestorPhone = phone.AreaCode + phone.DialNumber;
                        wo.RequestorPhoneExt = phone.PhoneExt;
                    }


                    // get requester contact preferred email address
                    email = new TXLifeTXLifeRequestOLifEPartyEmailAddress();
                    email.PartyID = wo.RequesterPartyID;
                    email.PreferredPrimaryAddress = true;  // get preferred
                    if (!string.IsNullOrEmpty(email.PartyID))
                    {
                        if (email.ProcessXML(tmpNode))
                            wo.RequestorEmail = email.Email;
                        else // preferred email doesn't exist, just get any email
                        {
                            email.PreferredPrimaryAddress = false;
                            email.ProcessXML(tmpNode);
                            wo.RequestorEmail = email.Email;
                        }
                    }

                }






                // clean up email
                email = null;


            }
            person = null;	// done with person


            // if the requestor name is blank, set it to equal the requestor company name
            if (wo.RequestorName == "")
                wo.RequestorName = wo.RequestorCompanyName;


            #region WRITING AGENT INFORMATION

            // **************************************** WRITING AGENT INFORMATION *********************************
            TXLifeRequestOLifERelationWritingAgent writingagent = new TXLifeRequestOLifERelationWritingAgent();
            writingagent.ProcessXML(tmpNode);
            string agentpartyID = writingagent.RelatedObjectID;
            writingagent = null;

            // get agent name
            if (agentpartyID.Length > 0)
            {
                person = new TXLifeTXLifeRequestOLifEPartyPerson();
                person.PartyID = agentpartyID;
                if (person.ProcessXML(tmpNode) == true)
                {
                    wo.WritingAgentFirstName = person.FirstName;
                    wo.WritingAgentLastName = person.LastName;
                    wo.CarrierCode = person.CarrierCode;
                }

                // done with person, clean up
                person = null;
            }

            // get agent email
            if (agentpartyID.Length > 0)
            {
                email = new TXLifeTXLifeRequestOLifEPartyEmailAddress();
                email.PartyID = agentpartyID;
                email.PreferredPrimaryAddress = true;

                if (email.ProcessXML(tmpNode))	// if there is a preferred email
                {
                    wo.WritingAgentEmail = email.Email;
                }
                else // we just get any email
                {
                    email.PreferredPrimaryAddress = false;
                    email.ProcessXML(tmpNode);
                    wo.WritingAgentEmail = email.Email;
                }
            }

            // done with email, clean up
            email = null;

            // get agent business phone number
            phone = new TXLifeTXLifeRequestOLifEPartyPhone();
            phone.PartyID = agentpartyID;
            phone.PhoneTypeToRetrieve = ""; // no phone type
            phone.ProcessXML(tmpNode);
            wo.WritingAgentPhone = phone.AreaCode + phone.DialNumber;
            wo.WritingAgentPhoneExt = phone.PhoneExt;

            if (wo.WritingAgentPhone == "" && !string.IsNullOrEmpty(phone.PartyID)) // get other phone number will do
            {
                phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Business; // default set to retrieve non preferred phone number
                phone.ProcessXML(tmpNode);
                wo.WritingAgentPhone = phone.AreaCode + phone.DialNumber;
                wo.WritingAgentPhoneExt = phone.PhoneExt;
            }

            if (wo.WritingAgentPhone == "" && !string.IsNullOrEmpty(phone.PartyID)) // get other phone number will do
            {
                phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Other; // default set to retrieve non preferred phone number
                phone.ProcessXML(tmpNode);
                wo.WritingAgentPhone = phone.AreaCode + phone.DialNumber;
                wo.WritingAgentPhoneExt = phone.PhoneExt;
            }



            if (wo.WritingAgentPhone == "" && !string.IsNullOrEmpty(phone.PartyID)) // get unknown phone number will do
            {
                phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Unknown; // default set to retrieve non preferred phone number
                phone.ProcessXML(tmpNode);
                wo.WritingAgentPhone = phone.AreaCode + phone.DialNumber;
                wo.WritingAgentPhoneExt = phone.PhoneExt;
            }


            if (wo.WritingAgentPhone == "" && !string.IsNullOrEmpty(phone.PartyID)) // get any phone number will do
            {
                phone.PhoneTypeToRetrieve = EIS.Applications.Common.Constants.PhoneTypeCode_Mobile; // default set to retrieve non preferred phone number
                phone.ProcessXML(tmpNode);
                wo.WritingAgentPhone = phone.AreaCode + phone.DialNumber;
                wo.WritingAgentPhoneExt = phone.PhoneExt;
            }

            // clean up phone
            phone = null;

            // address information for the doctor
            address = new TXLifeTXLifeRequestOLifEPartyAddress();
            address.AddressTypeToRetrieve = EIS.Applications.Common.Constants.AddressTypeCode_Bussiness;	// business type code only
            address.PartyID = agentpartyID;
            if (!string.IsNullOrEmpty(address.PartyID))
                address.ProcessXML(tmpNode);

            // assig address information to Work Order object for doctor
            wo.WritingAgentAddress = address.Line1;
            wo.WritingAgentCity = address.City;
            statecodetc = ConversionTools.ConvertToInt32(address.AddressStateTC, 0);
            if (statecodetc > 0)
            {
                var stateAbbr = Config.StateCodeList.Where(p => p.Code == statecodetc).FirstOrDefault();
                if (stateAbbr != null)
                    wo.WritingAgentState = stateAbbr.Description;
            }
            else
                wo.WritingAgentState = address.AddressStateTC;

            wo.WritingAgentZipCode = address.Zip;



            #endregion


            return wo;
        }

        private void CorrectData(WorkOrder wo)
        {
            if (!string.IsNullOrEmpty(wo.ApplicantState) && wo.ApplicantState.Trim().Length > 2)
                wo.ApplicantState = CorrectStateCode(wo.ApplicantState);

            if (!string.IsNullOrEmpty(wo.FacilityState) && wo.FacilityState.Trim().Length > 2)
                wo.FacilityState = CorrectStateCode(wo.FacilityState);

            if (!string.IsNullOrEmpty(wo.RequestorState) && wo.RequestorState.Trim().Length > 2)
                wo.RequestorState = CorrectStateCode(wo.RequestorState);

            if (!string.IsNullOrEmpty(wo.WritingAgentState) && wo.WritingAgentState.Trim().Length > 2)
                wo.WritingAgentState = CorrectStateCode(wo.WritingAgentState);

            
        }

        private string CorrectStateCode (string statecode)
        {
            try
            {
                // correct statecode       
                var stateAbbr = Config.StateCodeList.Where(p => p.Name.Trim().ToLower() == statecode.Trim().ToLower()).FirstOrDefault();
                if (stateAbbr != null)
                    return stateAbbr.Description;

            }
            catch (Exception ex)
            {
                _logger.Error($"LogId: {_logid}", ex);
            }



            return "";
        }



        public virtual List<WorkOrder> ProcessRequestObject(XmlNodeList nodelist, XmlNamespaceManager nsmgr)
        {
            if (nodelist.Count == 0) return new List<WorkOrder>();
             
            List<WorkOrder> list = new List<WorkOrder>();


            WorkOrder tempwo;


          
            foreach (XmlNode tmpNode in nodelist)
            {
                
                if (tmpNode.Name != "TXLifeRequest")
                    continue;

                // each TXLifeRequest might have more than one RequirementInfo node
                // if it is, that's how many WorkOrder object we have to create.
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(tmpNode.OuterXml);
                //requirementinfoNodeList = doc.DocumentElement.SelectNodes("//Policy/RequirementInfo");
                
                //
                //TransamericaService only send the one RequirementInfo per XML
                //foreach (XmlNode node in requirementinfoNodeList)
                //{

                    tempwo = CreateWorkOrder(tmpNode, "");
                    
                    // added : Dennis Tran
                    // Date  : 06/21/2018
                    // Need it for cancellation order
                    if (tempwo.IsCancelled)
                    {
                    string resultcancel = "";//dac.CancelOrder(tempwo.TransRefGUID);
                        if (resultcancel.Length > 1)
                            // no update is done based on the TransRefGuid given
                            tempwo.ErrorMessage = resultcancel;
                        else
                            tempwo.ErrorMessage = "";
                        list.Add(tempwo);
                        continue;
                    }


                    // assign the service ID/password here so the validate method will process it too
                    tempwo.ServicePassword = this.ServicePassword;
                    tempwo.ServiceUserName = this.ServiceUserName;
                    
                    if (!tempwo.IsUpdate && !tempwo.IsCancelled)
                        Validate(tempwo);
                    else if (tempwo.IsCancelled || tempwo.IsUpdate)
                        ValidateUpdateCancel(tempwo);

                    //tempwo.Validate();
                    tempwo.TestOnly = Config.TEST_ENVIRONMENT;
                    _logger.Info("Adding Work Order: " + tempwo.TransRefGUID);


                    // santatize the data because AXA does not send correct data..ex they use full state as statecode.
                    CorrectData(tempwo);
                    // save the workorder
                    //tempwo.WorkOrderID = dataprovider.InsertWorkOrder(tempwo);
                    //base.LogInfo("After Inserting Work Order: " + tempwo.TransRefGUID + " WorkOrder ID: " + tempwo.RequestID);

                    

                    list.Add(tempwo);
            }

           // dac = null;
            return list;
        }

        private void ValidateUpdateCancel(WorkOrder wo)
        {
            string errorMsg = string.Empty;

            if (wo.TransRefGUID.Length < 2)
            {
                errorMsg += "Missing TransRefGUID, ";
            }

            if (wo.ApplicationInfoTrackingID.Length < 2)
            {
                errorMsg += "Missing TrackingID, ";
            }

           
            if (errorMsg.Length > 0)
            {
                wo.ErrorMessage = errorMsg.Substring(0, errorMsg.Length - 2);
                _logger.Error($"Validate UpdateCancel-Transreguid: {wo.TransRefGUID} - {wo.ErrorMessage}");

            }
        }

        private void Validate(WorkOrder wo)
        {
            string errorMsg = string.Empty;

            if (wo.TransRefGUID.Length < 2)
            {
                errorMsg += "Missing TransRefGUID, ";
            }

            if (wo.ApplicationInfoTrackingID.Length < 2)
            {
                errorMsg += "Missing TrackingID, ";
            }

            //if (string.IsNullOrEmpty(wo.DoctorFirstName) && string.IsNullOrEmpty(wo.DoctorLastName) && string.IsNullOrEmpty(wo.DoctorOrFacilityName))
            //{
            //    errorMsg += "Missing Doctor/Facility Name, ";
            //}

            //if (wo.FacilityAddress.Length == 0)
            //    errorMsg += "Missing Doctor/Facility Address, ";

            //if (wo.FacilityZipCode.Length == 0)
            //    errorMsg += "Missing Doctor/Facility Zip Code, ";

            //if (wo.FacilityState.Length == 0)
            //    errorMsg += "Missing Doctor/Facility State, ";

            //if (wo.FacilityCity.Length == 0)
            //    errorMsg += "Missing Doctor/Facility City, ";

            //if (wo.DoctorOrFacilityName.Length == 0)
            //    errorMsg += "Missing Doctor/Facility Name, ";


            if (wo.ApplicantFirstName.Length == 0)
                errorMsg += "Missing Applicant First Name, ";

            if (wo.ApplicantLastName.Length == 0)
                errorMsg += "Missing Applicant Last Name, ";

            if (wo.ApplicantDOB.Length == 0)
                errorMsg += "Missing Applicant DOB, ";


            if (wo.ApplicantSSN.Length == 0)
                errorMsg += "Missing Applicant SSN, ";

            //if (wo..Length == 0)
            //    errorMsg += "Missing Applicant SSN, ";


            //if (wo.Gender.Trim().Length == 0)
            //    errorMsg += "Missing Gender, ";

            //if (wo.WritingAgentFirstName.Trim().Length == 0)
            //    errorMsg += "Missing Agent First Name, ";

            //if (wo.WritingAgentLastName.Trim().Length == 0)
            //    errorMsg += "Missing Agent Last Name, ";

            //if (ReqCodeTC != "11")
            //{
            //    if (CheckIfItIsExam(ReqCodeTC))
            //    {
            //        if (ApplicantState != "CA")  // we only do Exam in CA only
            //        {
            //            errorMsg += "Exam is for state of CA only,";
            //        }

            //        // if we made here, that means we have a good exam
            //        // let's check additional fields
            //        if (this.ApplicantDOB.Length == 0)
            //            errorMsg += "Missing Applicant DOB (for Exam only),";
            //    }
            //    else
            //        // APS orders for now,
            //        errorMsg += "ReqCodeTC (" + ReqCodeTC + ") is invalid, please use ReqCodeTC of 1,2,5,10,12,13,14,15,16,18,26, or 31 only.,";
            //}


            if (errorMsg.Length > 0)
            {
                wo.ErrorMessage = errorMsg.Substring(0, errorMsg.Length - 2);
                _logger.Error($"Validate Order-Transreguid: {wo.TransRefGUID} - {wo.ErrorMessage}");

            }
        }


    }
}