using System.ComponentModel.DataAnnotations;

namespace AcordIntake.Api.Models;

public class APSIncomingEntity
{
    [Key]
    public long APSIncomingEntityID { get; set; }

    public string ACORDVendorCode { get; set; } = string.Empty;
    public int CompanyID { get; set; }
    public string CopyInstructions { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public string DoctorCity { get; set; } = string.Empty;
    public string DoctorCountry { get; set; } = string.Empty;
    public string DoctorFacility { get; set; } = string.Empty;
    public string DoctorFax { get; set; } = string.Empty;
    public string DoctorFirstName { get; set; } = string.Empty;
    public string DoctorLastName { get; set; } = string.Empty;
    public string DoctorPhone { get; set; } = string.Empty;
    public string DoctorPhoneExtension { get; set; } = string.Empty;
    public string DoctorState { get; set; } = string.Empty;
    public string DoctorZipCode { get; set; } = string.Empty;
    public string DoctorStreet1 { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public string HIPPALocation { get; set; } = string.Empty;
    public bool IsError { get; set; }
    public bool IsTestOnly { get; set; }
    public bool LoadToEISMain { get; set; }
    public DateTime OrderDate { get; set; }
    public string PatientCity { get; set; } = string.Empty;
    public DateTime? PatientDOB { get; set; }
    public string PatientEmail { get; set; } = string.Empty;
    public string PatientFirstName { get; set; } = string.Empty;
    public string PatientLastName { get; set; } = string.Empty;
    public string PatientMiddleName { get; set; } = string.Empty;
    public string PatientPhone1 { get; set; } = string.Empty;
    public string PatientPhone2 { get; set; } = string.Empty;
    public string PatientGender { get; set; } = string.Empty;
    public string PatientSSN { get; set; } = string.Empty;
    public string PatientState { get; set; } = string.Empty;
    public string PatientStreet1 { get; set; } = string.Empty;
    public string PatientZipCode { get; set; } = string.Empty;
    public decimal PolicyAmcount { get; set; }
    public string PolicyNumber { get; set; } = string.Empty;
    public string RequestorAddess { get; set; } = string.Empty;
    public string RequestorCity { get; set; } = string.Empty;
    public string RequestorEmail { get; set; } = string.Empty;
    public string RequestorFirstName { get; set; } = string.Empty;
    public string RequestorLastName { get; set; } = string.Empty;
    public string RequestorPhone { get; set; } = string.Empty;
    public string RequestorPhoneExt { get; set; } = string.Empty;
    public string RequestorState { get; set; } = string.Empty;
    public string RequestorZipCode { get; set; } = string.Empty;
    public string RequirementAcctNum { get; set; } = string.Empty;
    public string RequirementInfoUniqueID { get; set; } = string.Empty;
    public bool SendToCompany { get; set; }
    public string TrackingID { get; set; } = string.Empty;
    public string TransRefGUID { get; set; } = string.Empty;
    public string WritingAgentAddress { get; set; } = string.Empty;
    public string WritingAgentCity { get; set; } = string.Empty;
    public string WritingAgentEmail { get; set; } = string.Empty;
    public string WritingAgentFirstName { get; set; } = string.Empty;
    public string WritingAgentLastName { get; set; } = string.Empty;
    public string WritingAgentPhone { get; set; } = string.Empty;
    public string WritingAgentPhoneExt { get; set; } = string.Empty;
    public string WritingAgentState { get; set; } = string.Empty;
    public string WritingAgentZipCode { get; set; } = string.Empty;
    public string CarrierCode { get; set; } = string.Empty;
    public string AgencyCarrierCode { get; set; } = string.Empty;
    public string CompanyProducerID { get; set; } = string.Empty;
    public bool IsUrgent { get; set; }
    public string DestinationCode { get; set; } = string.Empty;
}