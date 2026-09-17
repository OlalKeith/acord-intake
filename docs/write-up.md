# ACORD TXLife Intake

## 1. Environment

The assessment is being developed on:

- Kali Linux
- .NET 8
- PostgreSQL
- Python / Django
- Git

The database used for local development is:

- Database: `acord_intake`

## 2. Assessment Overview

The supplied code is part of a larger application that receives ACORD TXLife requests.

The assessment requires building a new application that can receive the supplied XML request, process the relevant business logic, map it into a WorkOrder, and save the resulting data into a database.

The supplied C# files are not a complete application. They are a subset of services, controllers, and data classes from the original system. Missing dependencies will therefore need to be identified and stubbed where necessary.

The main processing flow I am working toward is:

XML request
→ TXLifeRequest
→ validation
→ WorkOrder mapping
→ APSIncomingEntity
→ database

## 3. Input XML

`OrderRequest.xml` is the sample ACORD TXLife request provided with the assessment.

It is used as the input payload for testing the new application.

The XML contains:

- `TXLife`
- `TXLifeRequest`
- transaction information
- `OLifE` business data
- `Holding`
- `Policy`
- `RequirementInfo`
- `Attachment`
- `Party`
- `Relation`

The request represents a requirement order. In the supplied sample, the requirement code is `11` and the `HORequirementRefID` is `1`, which the assignment identifies as an APS order.

The XML also contains an inline base64 encoded PDF attachment.

## 4. Important XML Fields

Some of the fields that are relevant to processing are:

| XML field | Purpose |
|---|---|
| `TXLifeRequest/TransRefGUID` | Unique transaction reference |
| `TXLifeRequest/TransType/@tc` | Transaction type code |
| `TXLifeRequest/TransExeDate` | Transaction execution date |
| `TXLifeRequest/TransExeTime` | Transaction execution time |
| `TXLifeRequest/TransMode/@tc` | Transaction mode |
| `Holding/Policy/PolNumber` | Policy number |
| `Holding/Policy/RequirementInfo/ReqCode/@tc` | Requirement code |
| `Holding/Policy/RequirementInfo/HORequirementRefID` | Determines the requirement order type |
| `Holding/Policy/RequirementInfo/Attachment` | Requirement document |
| `Party` | People or organizations involved in the request |
| `Relation` | Relationships between the entities in the request |

## 5. Initial Understanding of the ACORD Structure

The XML uses the ACORD TXLife structure to represent the request and its related business entities.

`TXLifeRequest` contains the transaction information.

`OLifE` contains the business entities involved in the request.

A `Holding` contains the policy information and requirement being requested.

`Party` represents entities such as the insured, agent, physician, and agency.

`Relation` connects these entities to each other and to the holding.

For example, the sample contains relationships between the holding and the insured, the holding and the agent, and the insured and the physician.

## 6. Requirement Information

The sample contains a `RequirementInfo` element under the policy.

Important values include:

```xml
<ReqCode tc="11">Attending Phyisian Statement</ReqCode>
<HORequirementRefID>1</HORequirementRefID>