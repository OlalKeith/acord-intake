import json
from datetime import datetime, timezone

import psycopg
from django.conf import settings
from django.http import JsonResponse
from django.views.decorators.csrf import csrf_exempt


@csrf_exempt
def json_intake(request):
    if request.method != 'POST':
        return JsonResponse({'error': 'Only POST is allowed.'}, status=405)

    try:
        payload = json.loads(request.body.decode('utf-8'))
    except (TypeError, ValueError, UnicodeDecodeError):
        return JsonResponse({'error': 'Invalid JSON payload.'}, status=400)

    tx_life = payload.get('TXLife') or payload.get('txLife')
    if not tx_life:
        return JsonResponse({'error': 'TXLife payload is required.'}, status=400)

    request_node = tx_life.get('TXLifeRequest') or tx_life.get('txLifeRequest')
    if not request_node:
        return JsonResponse({'error': 'TXLifeRequest is required.'}, status=400)

    olif_e = request_node.get('OLifE') or request_node.get('oLifE') or {}
    holding = (olif_e.get('Holding') or olif_e.get('holding')) or {}
    policy = (holding.get('Policy') or holding.get('policy')) or {}
    requirement = (policy.get('RequirementInfo') or policy.get('requirementInfo')) or {}
    insured = None
    for party in olif_e.get('Party', []) if isinstance(olif_e.get('Party'), list) else [olif_e.get('Party')]:
        if party and party.get('id') == 'Party_Insured':
            insured = party
            break

    if not insured:
        return JsonResponse({'error': 'Party_Insured not found.'}, status=400)

    person = insured.get('Person') or insured.get('person') or {}
    address = insured.get('Address') or insured.get('address') or {}
    email = insured.get('EMailAddress') or insured.get('emailAddress') or {}
    phone = insured.get('Phone') or insured.get('phone') or {}

    trans_ref_guid = request_node.get('TransRefGUID') or request_node.get('transRefGUID') or ''
    tracking_id = (policy.get('ApplicationInfo') or policy.get('applicationInfo') or {}).get('TrackingID') or ''
    applicant_first = (person.get('FirstName') or person.get('firstName')) or ''
    applicant_last = (person.get('LastName') or person.get('lastName')) or ''
    applicant_dob = person.get('BirthDate') or person.get('birthDate') or ''
    applicant_ssn = insured.get('GovtID') or insured.get('govtID') or ''
    applicant_gender = (person.get('Gender') or person.get('gender') or {}).get('Value') or ''
    applicant_address = address.get('Line1') or address.get('line1') or ''
    applicant_city = address.get('City') or address.get('city') or ''
    applicant_state = address.get('AddressState') or address.get('addressState') or ''
    applicant_zip = address.get('Zip') or address.get('zip') or ''
    patient_email = (email.get('AddrLine') or email.get('addrLine')) or ''
    patient_phone = phone.get('DialNumber') or phone.get('dialNumber') or ''
    policy_number = policy.get('PolNumber') or policy.get('polNumber') or ''
    face_amount = (policy.get('Life') or policy.get('life') or {}).get('FaceAmt') or ''
    requirement_details = requirement.get('RequirementDetails') or requirement.get('requirementDetails') or ''
    requirement_acct_num = requirement.get('RequirementAcctNum') or requirement.get('requirementAcctNum') or ''
    requirement_unique_id = requirement.get('id') or requirement.get('ID') or ''
    attachment_location = ((requirement.get('Attachment') or requirement.get('attachment') or {}).get('AttachmentLocation') or '')
    created = datetime.now(timezone.utc)

    mapping = {
        'ACORDVendorCode': '',
        'CompanyID': 1,
        'CopyInstructions': requirement_details,
        'Created': created,
        'DoctorCity': '',
        'DoctorCountry': '',
        'DoctorFacility': '',
        'DoctorFax': '',
        'DoctorFirstName': '',
        'DoctorLastName': '',
        'DoctorPhone': '',
        'DoctorPhoneExtension': '',
        'DoctorState': '',
        'DoctorZipCode': '',
        'DoctorStreet1': '',
        'ErrorMessage': '',
        'HIPPALocation': attachment_location,
        'IsError': False,
        'IsTestOnly': False,
        'LoadToEISMain': True,
        'OrderDate': datetime.now(timezone.utc),
        'PatientCity': applicant_city,
        'PatientDOB': _parse_date(applicant_dob),
        'PatientEmail': patient_email,
        'PatientFirstName': applicant_first,
        'PatientLastName': applicant_last,
        'PatientMiddleName': person.get('MiddleName') or person.get('middleName') or '',
        'PatientPhone1': patient_phone,
        'PatientPhone2': '',
        'PatientGender': applicant_gender,
        'PatientSSN': applicant_ssn,
        'PatientState': applicant_state,
        'PatientStreet1': applicant_address,
        'PatientZipCode': applicant_zip,
        'PolicyAmcount': float(face_amount) if face_amount not in ('', None) else 0.0,
        'PolicyNumber': policy_number,
        'RequestorAddess': '',
        'RequestorCity': '',
        'RequestorEmail': '',
        'RequestorFirstName': '',
        'RequestorLastName': '',
        'RequestorPhone': '',
        'RequestorPhoneExt': '',
        'RequestorState': '',
        'RequestorZipCode': '',
        'RequirementAcctNum': requirement_acct_num,
        'RequirementInfoUniqueID': requirement_unique_id,
        'SendToCompany': True,
        'TrackingID': tracking_id,
        'TransRefGUID': trans_ref_guid,
        'WritingAgentAddress': '',
        'WritingAgentCity': '',
        'WritingAgentEmail': '',
        'WritingAgentFirstName': '',
        'WritingAgentLastName': '',
        'WritingAgentPhone': '',
        'WritingAgentPhoneExt': '',
        'WritingAgentState': '',
        'WritingAgentZipCode': '',
        'CarrierCode': '',
        'AgencyCarrierCode': '',
        'CompanyProducerID': '',
        'IsUrgent': False,
        'DestinationCode': '',
    }

    columns = [
        'ACORDVendorCode', 'CompanyID', 'CopyInstructions', 'Created',
        'DoctorCity', 'DoctorCountry', 'DoctorFacility', 'DoctorFax',
        'DoctorFirstName', 'DoctorLastName', 'DoctorPhone', 'DoctorPhoneExtension',
        'DoctorState', 'DoctorZipCode', 'DoctorStreet1', 'ErrorMessage',
        'HIPPALocation', 'IsError', 'IsTestOnly', 'LoadToEISMain',
        'OrderDate', 'PatientCity', 'PatientDOB', 'PatientEmail',
        'PatientFirstName', 'PatientLastName', 'PatientMiddleName', 'PatientPhone1',
        'PatientPhone2', 'PatientGender', 'PatientSSN', 'PatientState',
        'PatientStreet1', 'PatientZipCode', 'PolicyAmcount', 'PolicyNumber',
        'RequestorAddess', 'RequestorCity', 'RequestorEmail', 'RequestorFirstName',
        'RequestorLastName', 'RequestorPhone', 'RequestorPhoneExt', 'RequestorState',
        'RequestorZipCode', 'RequirementAcctNum', 'RequirementInfoUniqueID',
        'SendToCompany', 'TrackingID', 'TransRefGUID', 'WritingAgentAddress',
        'WritingAgentCity', 'WritingAgentEmail', 'WritingAgentFirstName',
        'WritingAgentLastName', 'WritingAgentPhone', 'WritingAgentPhoneExt',
        'WritingAgentState', 'WritingAgentZipCode', 'CarrierCode', 'AgencyCarrierCode',
        'CompanyProducerID', 'IsUrgent', 'DestinationCode',
    ]

    try:
        with psycopg.connect(
            host=settings.DATABASES['default']['HOST'],
            dbname=settings.DATABASES['default']['NAME'],
            user=settings.DATABASES['default']['USER'],
            password=settings.DATABASES['default']['PASSWORD'],
            port=settings.DATABASES['default']['PORT'],
        ) as conn:
            with conn.cursor() as cur:
                cols_sql = ', '.join(f'"{column}"' for column in columns)
                values_sql = ', '.join(['%s'] * len(columns))
                insert_sql = f'INSERT INTO aps_incoming ({cols_sql}) VALUES ({values_sql})'
                cur.execute(insert_sql, [mapping[column] for column in columns])
        return JsonResponse({'status': 'ok', 'payload': mapping, 'saved_to_table': 'aps_incoming'}, status=200)
    except Exception as exc:
        return JsonResponse({'error': f'Database save failed: {exc}'}, status=500)


def _parse_date(value):
    if not value:
        return None
    try:
        return datetime.fromisoformat(value.replace('Z', '+00:00'))
    except ValueError:
        return None
