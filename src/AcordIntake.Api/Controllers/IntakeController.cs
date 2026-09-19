using System.Xml;
using AcordIntake.Api.Data;
using AcordIntake.Api.Mapping;
using AcordIntake.Api.Models;
using AcordIntake.Api.Parsing;
using Microsoft.AspNetCore.Mvc;

namespace AcordIntake.Api.Controllers;

[ApiController]
[Route("api/intake")]
public sealed class IntakeController(AcordDbContext db) : ControllerBase
{
    [HttpPost("xml")]
    [Consumes("application/xml", "text/xml")]
    public async Task<IActionResult> PostXml()
    {
        using var reader = new StreamReader(Request.Body);
        var payload = await reader.ReadToEndAsync();
        if (string.IsNullOrWhiteSpace(payload))
            return BadRequest(new { error = "The XML request body is empty." });

        XmlDocument document;
        try
        {
            var normalizedXml = new XmlUtility().RemoveNamespacesFromXml(payload);
            document = new XmlDocument();
            document.LoadXml(normalizedXml);
        }
        catch (XmlException exception)
        {
            return BadRequest(new { error = $"Invalid XML: {exception.Message}" });
        }

        var requestNode = document.SelectSingleNode("/TXLife/TXLifeRequest");
        if (requestNode is null)
            return BadRequest(new { error = "TXLifeRequest was not found." });

        var request = new TXLifeRequest();
        if (!request.ProcessXml(requestNode))
            return BadRequest(new { error = "TXLifeRequest could not be parsed." });

        var workOrder = MapWorkOrder(requestNode, request);
        Validate(workOrder);
        var entity = MapEntity(workOrder);

        db.APSIncomingEntities.Add(entity);
        await db.SaveChangesAsync();

        return Ok(new
        {
            workOrder,
            apsIncomingEntity = entity,
            validationErrors = workOrder.ErrorMessage
        });
    }

    private static WorkOrder MapWorkOrder(XmlNode requestNode, TXLifeRequest request)
    {
        var insured = requestNode.SelectSingleNode("OLifE/Party[@id='Party_Insured']");
        var policy = requestNode.SelectSingleNode("OLifE/Holding/Policy");
        var requirement = policy?.SelectSingleNode("RequirementInfo");
        var requesterId = requirement?.Attributes?["RequesterPartyID"]?.Value ?? string.Empty;
        var requester = requestNode.SelectSingleNode($"OLifE/Party[@id='{requesterId}']");
        var physicianId = RelationTarget(requestNode, "Physician");
        var physician = requestNode.SelectSingleNode($"OLifE/Party[@id='{physicianId}']");
        var writingAgentId = RelationTarget(requestNode, "Agent");
        var writingAgent = requestNode.SelectSingleNode($"OLifE/Party[@id='{writingAgentId}']");
        var attachment = requirement?.SelectSingleNode("Attachment");

        return new WorkOrder
        {
            TransRefGUID = request.TransRefGUID,
            TransCode = request.TransCode,
            TransExeDate = request.TransExeDate,
            TransExeTime = request.TransExeTime,
            TestOnly = request.TestIndicator.Equals("true", StringComparison.OrdinalIgnoreCase)
                || request.TestIndicator.Equals("yes", StringComparison.OrdinalIgnoreCase),
            ApplicationInfoTrackingID = Value(policy, "ApplicationInfo/TrackingID"),
            PolicyNumber = Value(policy, "PolNumber"),
            FaceAmount = Value(policy, "Life/FaceAmt"),
            InsuranceCompany = Value(requestNode, "OLifE/Party[@id='Party_Carrier']/FullName"),
            ReqCodeTC = Attribute(requirement?.SelectSingleNode("ReqCode"), "tc"),
            RequirementDetails = Value(requirement, "RequirementDetails"),
            RequestedDate = Value(requirement, "RequestedDate"),
            RequirementAcctNum = Value(requirement, "RequirementAcctNum"),
            RequirementInfoUniqueID = requirement?.Attributes?["id"]?.Value ?? string.Empty,
            AppliesToPartyID = requirement?.Attributes?["AppliesToPartyID"]?.Value ?? string.Empty,
            RequesterPartyID = requesterId,
            RequestorCompanyName = Value(requester, "FullName"),
            AgencyCarrierCode = Value(requester, "Producer/CarrierAppointment/CarrierCode"),
            CompanyProducerID = Value(requester, "Producer/CarrierAppointment/CompanyProducerID"),
            ApplicantFirstName = Value(insured, "Person/FirstName"),
            ApplicantMiddleInitial = Value(insured, "Person/MiddleName"),
            ApplicantLastName = Value(insured, "Person/LastName"),
            ApplicantDOB = Value(insured, "Person/BirthDate"),
            ApplicantSSN = Value(insured, "GovtID"),
            Gender = Value(insured, "Person/Gender"),
            ApplicantAddress = Value(insured, "Address/Line1"),
            ApplicantCity = Value(insured, "Address/City"),
            ApplicantState = Value(insured, "Address/AddressState"),
            ApplicantZip = Value(insured, "Address/Zip"),
            PatientEmail = Value(insured, "EMailAddress/AddrLine"),
            PatientPhone1 = Value(insured, "Phone/DialNumber"),
            DoctorFirstName = Value(physician, "Person/FirstName"),
            DoctorLastName = Value(physician, "Person/LastName"),
            DoctorOrFacilityName = Value(physician, "FullName"),
            FacilityAddress = Value(physician, "Address/Line1"),
            FacilityCity = Value(physician, "Address/City"),
            FacilityState = Value(physician, "Address/AddressState"),
            FacilityZipCode = Value(physician, "Address/Zip"),
            FacilityPhone = Value(physician, "Phone/DialNumber"),
            WritingAgentFirstName = Value(writingAgent, "Person/FirstName")
                is var writingAgentFirstName && writingAgentFirstName.Length > 0
                    ? writingAgentFirstName
                    : Value(writingAgent, "FullName"),
            WritingAgentLastName = Value(writingAgent, "Person/LastName"),
            WritingAgentEmail = Value(writingAgent, "EMailAddress/AddrLine"),
            WritingAgentPhone = Value(writingAgent, "Phone/DialNumber"),
            Note = Value(requirement, "RequirementDetails"),
            AttachmentLocation = Value(attachment, "AttachmentLocation")
        };
    }

    private static APSIncomingEntity MapEntity(WorkOrder workOrder)
    {
        var entity = new APSIncomingEntity
        {
            ACORDVendorCode = string.Empty,
            CompanyID = 1,
            CopyInstructions = workOrder.RequirementDetails,
            Created = DateTime.UtcNow,
            ErrorMessage = workOrder.ErrorMessage,
            IsError = workOrder.ErrorMessage.Length > 0,
            IsTestOnly = workOrder.TestOnly,
            LoadToEISMain = workOrder.ErrorMessage.Length == 0 && !workOrder.TestOnly,
            OrderDate = ConversionTools.ConvertToDate(workOrder.TransExeDate, DateTime.UtcNow),
            DoctorCity = workOrder.FacilityCity,
            DoctorFacility = workOrder.DoctorOrFacilityName,
            DoctorFirstName = workOrder.DoctorFirstName,
            DoctorLastName = workOrder.DoctorLastName,
            DoctorPhone = workOrder.FacilityPhone,
            DoctorState = workOrder.FacilityState,
            DoctorStreet1 = workOrder.FacilityAddress,
            DoctorZipCode = workOrder.FacilityZipCode,
            HIPPALocation = workOrder.AttachmentLocation,
            PatientCity = workOrder.ApplicantCity,
            PatientDOB = ConversionTools.ConvertToDate(workOrder.ApplicantDOB, DateTime.MinValue),
            PatientEmail = workOrder.PatientEmail,
            PatientFirstName = workOrder.ApplicantFirstName,
            PatientLastName = workOrder.ApplicantLastName,
            PatientMiddleName = workOrder.ApplicantMiddleInitial,
            PatientPhone1 = workOrder.PatientPhone1,
            PatientGender = workOrder.Gender,
            PatientSSN = workOrder.ApplicantSSN,
            PatientState = workOrder.ApplicantState,
            PatientStreet1 = workOrder.ApplicantAddress,
            PatientZipCode = workOrder.ApplicantZip,
            PolicyAmcount = ConversionTools.ConvertToDecimal(workOrder.FaceAmount, 0),
            PolicyNumber = workOrder.PolicyNumber,
            RequirementAcctNum = workOrder.RequirementAcctNum,
            RequirementInfoUniqueID = workOrder.RequirementInfoUniqueID,
            RequestorFirstName = workOrder.RequestorFirstName,
            RequestorLastName = workOrder.RequestorLastName,
            RequestorEmail = workOrder.RequestorEmail,
            RequestorPhone = workOrder.RequestorPhone,
            TrackingID = workOrder.ApplicationInfoTrackingID,
            TransRefGUID = workOrder.TransRefGUID,
            WritingAgentFirstName = workOrder.WritingAgentFirstName,
            WritingAgentLastName = workOrder.WritingAgentLastName,
            WritingAgentEmail = workOrder.WritingAgentEmail,
            WritingAgentPhone = workOrder.WritingAgentPhone,
            AgencyCarrierCode = workOrder.AgencyCarrierCode,
            CompanyProducerID = workOrder.CompanyProducerID,
            DestinationCode = workOrder.InsuranceCompany
        };

        return entity;
    }

    private static void Validate(WorkOrder workOrder)
    {
        var errors = new List<string>();
        if (workOrder.TransRefGUID.Length < 2)
            errors.Add("Missing TransRefGUID");
        if (workOrder.ApplicationInfoTrackingID.Length < 2)
            errors.Add("Missing TrackingID");
        if (workOrder.ApplicantFirstName.Length < 2)
            errors.Add("Missing ApplicantFirstName");
        if (workOrder.ApplicantLastName.Length < 2)
            errors.Add("Missing ApplicantLastName");
        if (workOrder.ApplicantDOB.Length < 2)
            errors.Add("Missing ApplicantDOB");
        if (workOrder.ApplicantSSN.Length < 2)
            errors.Add("Missing ApplicantSSN");

        workOrder.ErrorMessage = string.Join(", ", errors);
    }

    private static string Value(XmlNode? node, string path)
    {
        return node?.SelectSingleNode(path)?.InnerText.Trim() ?? string.Empty;
    }

    private static string Attribute(XmlNode? node, string name)
    {
        return node?.Attributes?[name]?.Value ?? string.Empty;
    }

    private static string RelationTarget(XmlNode requestNode, string role)
    {
        return requestNode.SelectSingleNode(
            $"OLifE/Relation[RelationRoleCode='{role}']")?.Attributes?["RelatedObjectID"]?.Value
            ?? string.Empty;
    }
}