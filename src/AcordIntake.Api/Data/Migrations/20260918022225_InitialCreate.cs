using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AcordIntake.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "aps_incoming",
                columns: table => new
                {
                    APSIncomingEntityID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ACORDVendorCode = table.Column<string>(type: "text", nullable: false),
                    CompanyID = table.Column<int>(type: "integer", nullable: false),
                    CopyInstructions = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DoctorCity = table.Column<string>(type: "text", nullable: false),
                    DoctorCountry = table.Column<string>(type: "text", nullable: false),
                    DoctorFacility = table.Column<string>(type: "text", nullable: false),
                    DoctorFax = table.Column<string>(type: "text", nullable: false),
                    DoctorFirstName = table.Column<string>(type: "text", nullable: false),
                    DoctorLastName = table.Column<string>(type: "text", nullable: false),
                    DoctorPhone = table.Column<string>(type: "text", nullable: false),
                    DoctorPhoneExtension = table.Column<string>(type: "text", nullable: false),
                    DoctorState = table.Column<string>(type: "text", nullable: false),
                    DoctorZipCode = table.Column<string>(type: "text", nullable: false),
                    DoctorStreet1 = table.Column<string>(type: "text", nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: false),
                    HIPPALocation = table.Column<string>(type: "text", nullable: false),
                    IsError = table.Column<bool>(type: "boolean", nullable: false),
                    IsTestOnly = table.Column<bool>(type: "boolean", nullable: false),
                    LoadToEISMain = table.Column<bool>(type: "boolean", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PatientCity = table.Column<string>(type: "text", nullable: false),
                    PatientDOB = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PatientEmail = table.Column<string>(type: "text", nullable: false),
                    PatientFirstName = table.Column<string>(type: "text", nullable: false),
                    PatientLastName = table.Column<string>(type: "text", nullable: false),
                    PatientMiddleName = table.Column<string>(type: "text", nullable: false),
                    PatientPhone1 = table.Column<string>(type: "text", nullable: false),
                    PatientPhone2 = table.Column<string>(type: "text", nullable: false),
                    PatientGender = table.Column<string>(type: "text", nullable: false),
                    PatientSSN = table.Column<string>(type: "text", nullable: false),
                    PatientState = table.Column<string>(type: "text", nullable: false),
                    PatientStreet1 = table.Column<string>(type: "text", nullable: false),
                    PatientZipCode = table.Column<string>(type: "text", nullable: false),
                    PolicyAmcount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PolicyNumber = table.Column<string>(type: "text", nullable: false),
                    RequestorAddess = table.Column<string>(type: "text", nullable: false),
                    RequestorCity = table.Column<string>(type: "text", nullable: false),
                    RequestorEmail = table.Column<string>(type: "text", nullable: false),
                    RequestorFirstName = table.Column<string>(type: "text", nullable: false),
                    RequestorLastName = table.Column<string>(type: "text", nullable: false),
                    RequestorPhone = table.Column<string>(type: "text", nullable: false),
                    RequestorPhoneExt = table.Column<string>(type: "text", nullable: false),
                    RequestorState = table.Column<string>(type: "text", nullable: false),
                    RequestorZipCode = table.Column<string>(type: "text", nullable: false),
                    RequirementAcctNum = table.Column<string>(type: "text", nullable: false),
                    RequirementInfoUniqueID = table.Column<string>(type: "text", nullable: false),
                    SendToCompany = table.Column<bool>(type: "boolean", nullable: false),
                    TrackingID = table.Column<string>(type: "text", nullable: false),
                    TransRefGUID = table.Column<string>(type: "text", nullable: false),
                    WritingAgentAddress = table.Column<string>(type: "text", nullable: false),
                    WritingAgentCity = table.Column<string>(type: "text", nullable: false),
                    WritingAgentEmail = table.Column<string>(type: "text", nullable: false),
                    WritingAgentFirstName = table.Column<string>(type: "text", nullable: false),
                    WritingAgentLastName = table.Column<string>(type: "text", nullable: false),
                    WritingAgentPhone = table.Column<string>(type: "text", nullable: false),
                    WritingAgentPhoneExt = table.Column<string>(type: "text", nullable: false),
                    WritingAgentState = table.Column<string>(type: "text", nullable: false),
                    WritingAgentZipCode = table.Column<string>(type: "text", nullable: false),
                    CarrierCode = table.Column<string>(type: "text", nullable: false),
                    AgencyCarrierCode = table.Column<string>(type: "text", nullable: false),
                    CompanyProducerID = table.Column<string>(type: "text", nullable: false),
                    IsUrgent = table.Column<bool>(type: "boolean", nullable: false),
                    DestinationCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aps_incoming", x => x.APSIncomingEntityID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aps_incoming");
        }
    }
}
