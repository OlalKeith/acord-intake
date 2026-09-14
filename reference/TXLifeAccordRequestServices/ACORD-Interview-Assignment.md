# Interview assignment: ACORD TXLife intake

**Shared code:** everything in this folder. Read the project as a whole, not only one file.

**XML node reference:** [ACORD Model Viewer (PilotFish)](https://modelviewers.pilotfishtechnology.com/modelviewers/ACORD/index.html) — use this to identify TXLife / OLifE nodes, attributes, and how they nest.

---

## What to do

1. **Read the shared project code.** Work through all `.cs` files in this folder. They model an ACORD TXLife request (controller plus nested OLifE types: party, holding, policy, relations, attachments, and so on).

2. **Translate and document it.** In your own words, explain how the code works so it is clear you understood it: intake flow, XML parsing, object mapping, WorkOrder mapping, validation (new vs update/cancel), and `WorkOrderMapper`. Ignore commented-out blocks. Stub types that are not in these files. Tie XML elements you mention back to the [ACORD Model Viewer](https://modelviewers.pilotfishtechnology.com/modelviewers/ACORD/index.html).

3. **Run it in a .NET project that accepts XML.** Stand up a runnable .NET app (modern ASP.NET Core or a console app is fine) that accepts **XML files** (or XML request bodies). Use the **provided XML sample** as the request. Wire in the live behavior from the shared code. Do not change the original files in this folder; copy or adapt them into your own project.

4. **Build a separate Django project that accepts JSON.** Port the same live behavior into Django (DRF is fine). That API must accept **JSON**, not XML. JSON fields must correspond to the same ACORD XML nodes/fields as the provided XML sample (same data, different format).

5. **Persist both payloads to the same database table.** After parsing and mapping, both the .NET XML intake and the Django JSON intake must **save into the same table** (same schema, same destination). A row from XML and a row from JSON for the same sample should look equivalent.

6. **Show you can run both.** Exercise the .NET path with the provided XML sample, convert that sample to JSON for Django, and confirm both writes land in the same table. Also show WorkOrder, APSIncomingEntity, and any `ErrorMessage` validation errors.

---

## Background

This folder is a legacy ACORD TXLife intake slice:

| File | Role (high level) |
| --- | --- |
| `GenericACORDController.cs` | Intake controller: parse document, find TXLife / TXLifeRequest, map to WorkOrder, validate, map to APSIncomingEntity |
| `TXLifeRequest.cs` | TXLife request envelope (trans ref, type, dates, and related request fields) |
| `TXLifeRequestOLifEParty.cs` and related Party files | Party / person / organization / producer / phone / email |
| `TXLifeRequestOLifEHolding*.cs` | Holding, policy, life, application info, requirement info, attachments |
| `TXLifeRequestOLifERelation*.cs` | Relations (agency, doctor, home office, writing agent, etc.) |
| `TXLifeRequestAttachment.cs` | Request-level attachments |

The original C# parses **XML**. For this assignment:

- **Provided XML sample:** you will be given an XML file. Use that as the source payload. Do not invent a different ACORD document unless you also show the equivalent JSON of that same sample.
- **.NET project:** accept **XML** (the provided sample), using the same nodes as this codebase and as documented in the [ACORD Model Viewer](https://modelviewers.pilotfishtechnology.com/modelviewers/ACORD/index.html).
- **Django project:** accept **JSON** whose keys/structure represent the **same fields** as that XML sample (for example `TXLife` → `TXLifeRequest` → `OLifE` → Party / Holding / Relation).
- **Shared table:** both apps write mapped results to **one** database table. Design the table from the mapped WorkOrder / APSIncomingEntity fields. Point both projects at the same database.

The live flow to preserve in both projects:

1. Accept a payload (XML in .NET, JSON in Django).
2. Parse the document.
3. Find `TXLife` / `TXLifeRequest` objects.
4. Map each request into a **WorkOrder**.
5. Validate new vs update/cancel.
6. Map a WorkOrder to an **APSIncomingEntity**.
7. **Save** the mapped record into the **same** database table.

If the provided sample uses real-looking identifiers, treat them as test data. Do not call external APIs.

---

## Submit

A zip or git repo with:

- Your write-up of the original C# (shows you read and understood the shared folder)
- A **.NET** project that accepts **XML** (run against the provided sample)
- A **Django** project that accepts **JSON** of the same fields
- Schema / connection notes proving **both save to the same table**
- Brief notes on what you stubbed, how the XML nodes map to JSON keys, and how you ran both

Leave the files in this folder unchanged.

Good luck.
