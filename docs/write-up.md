# ACORD TXLife Intake

## 1. What this project does

The assignment is based on an existing C# ACORD TXLife intake flow.

I built two small APIs around the same basic flow:

- .NET API that accepts the supplied ACORD XML
- Django API that accepts the equivalent JSON

Both applications save the mapped data into the same PostgreSQL table:

`public.aps_incoming`

The main flow is:

XML / JSON
→ read the request
→ map the relevant fields
→ validate the data
→ create the incoming order
→ save it to PostgreSQL

## 2. Environment

I developed and tested the project on:

- Kali Linux
- .NET 8
- Python 3.14
- Django 5.2
- PostgreSQL
- Git

Local database:

`acord_intake`

PostgreSQL:

`localhost:5432`

## 3. Source code and sample XML

The original C# files provided for the assignment are under:

`reference/TXLifeAccordRequestServices/`

The sample request is:

`docs/sample-data/OrderRequest.xml`

The XML is an ACORD TXLife request. It contains the transaction details, policy, requirement, attachment, insured, physician, agent and the relationships between them.

The main parts I worked with are:

- `TXLifeRequest`
- `OLifE`
- `Holding`
- `Policy`
- `RequirementInfo`
- `Attachment`
- `Party`
- `Relation`

## 4. .NET XML intake

The .NET application is in:

`src/AcordIntake.Api`

The endpoint is:

`POST /api/intake/xml`

The application:

1. Receives the XML request.
2. Reads the `TXLifeRequest`.
3. Extracts the transaction information.
4. Reads the policy and requirement information.
5. Finds the relevant parties, including the insured and physician.
6. Maps the information into a `WorkOrder`.
7. Validates the required values.
8. Maps the `WorkOrder` into an `APSIncomingEntity`.
9. Saves the result to PostgreSQL.


The main values being mapped include the policy number, tracking ID, patient details, requirement information and attachment information.

## 5. Django JSON intake

The Django application is in:

`src/AcordIntake.Django`

The endpoint is:

`POST /api/intake/json`

The JSON uses the same basic structure and information as the XML request.

The Django API reads the relevant fields and saves them to the same `aps_incoming` table used by the .NET application.

## 6. Shared database table

Both applications write to:

`public.aps_incoming`

The .NET application creates the table using EF Core migrations.

Some of the fields used by the mapping are:

- `TransRefGUID`
- `TrackingID`
- `PolicyNumber`
- `PolicyAmcount`
- `PatientFirstName`
- `PatientLastName`
- `PatientDOB`
- `PatientSSN`
- `PatientEmail`
- `RequirementAcctNum`
- `HIPPALocation`
- `ErrorMessage`
- `IsTestOnly`
- `Created`
- `OrderDate`

The table is shared by both intake paths.

## 7. Main field mappings

Some of the important mappings are:

| XML field | Database field |
|---|---|
| `TXLifeRequest/TransRefGUID` | `TransRefGUID` |
| `Policy/PolNumber` | `PolicyNumber` |
| `Policy/Life/FaceAmt` | `PolicyAmcount` |
| `Policy/ApplicationInfo/TrackingID` | `TrackingID` |
| `Party[@id='Party_Insured']/Person/FirstName` | `PatientFirstName` |
| `Party[@id='Party_Insured']/Person/LastName` | `PatientLastName` |
| `Party[@id='Party_Insured']/Person/BirthDate` | `PatientDOB` |
| `Party[@id='Party_Insured']/GovtID` | `PatientSSN` |
| `Party[@id='Party_Insured']/EMailAddress/AddrLine` | `PatientEmail` |
| `Policy/RequirementInfo/RequirementAcctNum` | `RequirementAcctNum` |
| `Policy/RequirementInfo/Attachment/AttachmentLocation` | `HIPPALocation` |

## 8. Things I found while working through the reference code

The supplied C# files are not a complete application. They are a selection of services, controllers and models from the original system.

Because of this, some dependencies were missing from the supplied code. I added small placeholders where needed so that I could run the relevant flow without trying to recreate the entire original application.

I also found some areas where the sample data and the legacy code need to be handled carefully.

For example, the sample contains:

```xml
<TransExeDate>2024 -12-10</TransExeDate>
```

There is a space before the `-`, so this needs to be handled when parsing the date.

Another example is the transaction information. The legacy code reads the `tc` value from `TransType` as `TransCode`, so I kept track of that behavior when mapping the request.

I also found a reference to `TXLifeTXLifeRequestOLifEPartyAddress` in the supplied code, but the class was not included in the provided files. I created a minimal placeholder for the missing dependency.

## 9. Testing

I tested the .NET application with the supplied XML:

```bash
dotnet build src/AcordIntake.Api/AcordIntake.Api.csproj --no-restore
```

The project builds successfully with no warnings or errors.

I also checked the Django project:

```bash
cd src/AcordIntake.Django
/usr/bin/python3.14 manage.py check
```

The Django system check completed without issues.

### XML request

I sent the supplied XML directly to the API:

```bash
curl -sS -X POST http://127.0.0.1:5015/api/intake/xml \
  -H 'Content-Type: application/xml' \
  --data-binary @docs/sample-data/OrderRequest.xml
```

The request returned `200` and created a record in `aps_incoming`.

### JSON request

I also sent the equivalent JSON to:

```text
POST /api/intake/json
```

The response confirmed that the record was saved to:

```text
aps_incoming
```

### Database check

I queried the shared table after running both requests to confirm that records from both paths were present.

The important values such as policy number, tracking ID, requirement account number and patient information matched between the XML and JSON requests.

## 10. Running the project

### .NET

```bash
dotnet run --project src/AcordIntake.Api
```

Then send the XML to:

```text
POST http://127.0.0.1:5015/api/intake/xml
```

### Django

```bash
cd src/AcordIntake.Django
python manage.py runserver 127.0.0.1:8001
```

Then send JSON to:

```text
POST http://127.0.0.1:8001/api/intake/json
```