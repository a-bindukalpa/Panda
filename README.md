# Panda
A backend application for patient and appointments
The datastore used is MongoDB. It  is used as the persistent stoore.
The application uses Docker to build and run the application and Mongo db images.

- [ ] Run `docker-compose up --build -d` 
- [ ] Navigate to http://localhost:5001/swagger/index.html
- [ ] Perform the various crud operations
- [ ] The CRUD operations can also be performed using Panda.API.http within Visual Studio
- [ ] To close app and remove containers run `docker-compose down`

Sample patient data for insertion
```json

  {
  "nhsNumber": "1685807151", 
  "name": "Jeffrey Young3", 
  "dateOfBirth": "1963-08-27", 
  "postcode":"WA55 8HE"
}
{
    "nhsNumber": "1685807151",
    "name": "Dr Julie Smith",
    "dateOfBirth": "1940-02-05",
    "postcode": "S95 8GE"
  }
  {
    "nhsNumber": "1962097471",
    "name": "Nicholas Rowley",
    "dateOfBirth": "1991-10-16",
    "postcode": "DT8 5BW"
  }
  {
    "nhsNumber": "0803542917",
    "name": "Mrs Marian Bailey",
    "dateOfBirth": "1987-03-25",
    "postcode": "S65 7QW"
  }

Sample patient data for updates
```json
{
  "nhsNumber": "1685807151",
  "name": "Jeffrey Young Jefferson",
  "dateOfBirth": "1963-08-27",
  "postcode": "WA55 8HE"
}
{
    "nhsNumber": "1685807151",
    "name": "Dr Julie Smith Hamilton",
    "dateOfBirth": "1940-02-05",
    "postcode": "S95 8GE"
  }
  {
    "nhsNumber": "1962097471",
    "name": "Ben Stokes",
    "dateOfBirth": "1991-10-16",
    "postcode": "DT8 5BW"
  }
  {
    "nhsNumber": "0803542917",
    "name": "Joe Root",
    "dateOfBirth": "1987-03-25",
    "postcode": "S65 7QW"
  }
```
The NHS number can be used to query the patient data.
Sample appointment data for insertion
```json
{
    "patient": "0240288238",
    "status": "Active",
    "time": "2025-07-16T13:00:00+01:00",
    "duration": "1h30m",
    "clinician": "Dr Jordan Lewis",
    "department": "orthopaedics",
    "postcode": "CW6E 2ET",
    "id": "c455638f-99aa-478a-82bc-fd76da4991f8"
  }
  {
    "patient": "0699052556",
    "status": "Attended",
    "time": "2021-03-15T13:30:00+00:00",
    "duration": "1h30m",
    "clinician": "Francis Stewart",
    "department": "gastroentology",
    "postcode": "LA10 3TZ",
    "id": "3fd51de5-a30a-458e-89e1-bb7f1b89cab2"
  }
  {
    "patient": "0544469364",
    "status": "Active",
    "time": "2023-10-24T16:00:00+01:00",
    "duration": "1h",
    "clinician": "Rosemary Simmons-West",
    "department": "orthopaedics",
    "postcode": "BT84 1NA",
    "id": "41601e49-aa8b-4ee7-8d85-8f76db34624c"
  }
  {
    "patient": "1635754941",
    "status": "Active",
    "time": "2026-03-11T12:00:00+00:00",
    "duration": "15m",
    "clinician": "Alexandra Watson",
    "department": "orthopaedics",
    "postcode": "NR6 7US",
    "id": "e88fe1af-53de-4d14-9da5-0794c08d7f51"
  }

  Appointment data for updates
  ```json
  {
    "patient": "0240288238",
    "status": "Attended",
    "time": "2025-07-16T13:00:00+01:00",
    "duration": "1h30m",
    "clinician": "Dr Jordan Lewis",
    "department": "orthopaedics",
    "postcode": "CW6E 2ET",
    "id": "c455638f-99aa-478a-82bc-fd76da4991f8"
  }
  {
    "patient": "0699052556",
    "status": "Cancelled",
    "time": "2021-03-15T13:30:00+00:00",
    "duration": "1h30m",
    "clinician": "Francis Stewart",
    "department": "gastroentology",
    "postcode": "LA10 3TZ",
    "id": "3fd51de5-a30a-458e-89e1-bb7f1b89cab2"
  }
  {
    "patient": "0544469364",
    "status": "Missed",
    "time": "2023-10-24T16:00:00+01:00",
    "duration": "1h",
    "clinician": "Rosemary Simmons-West",
    "department": "orthopaedics",
    "postcode": "BT84 1NA",
    "id": "41601e49-aa8b-4ee7-8d85-8f76db34624c"
  }
  {
    "patient": "1635754941",
    "status": "Cancelled",
    "time": "2026-03-11T12:00:00+00:00",
    "duration": "15m",
    "clinician": "Alexandra Watson",
    "department": "orthopaedics",
    "postcode": "NR6 7US",
    "id": "e88fe1af-53de-4d14-9da5-0794c08d7f51"
  }
  {
    "patient": "1431315257",
    "status": "missed",
    "time": "2022-10-28T10:30:00+01:00",
    "duration": "40m",
    "clinician": "Ms Geraldine Collins",
    "department": "orthopaedics",
    "postcode": "JE09 9WF",
    "id": "5d5c84b6-9e88-4164-b7ec-5b1d11ca49a2"
  }

  I have gone for a layered architecture with the following layers
  + API layer which handles the HTTP requests and responses
  + Service layer which contains the business logic
  + Repository layer which handles the data access and storage
  + Domain layer which contains the domain models

  I have used the repository pattern to abstract the data access and storage from the service layer. This makes it easily alter databases if needed.
  I have done most of the validation in the service layer. This makes it easy to test the business logic and separates concern of validation to another layer. 
  I have used the NHS number as the unique identifier for the patient. I could have gone with a DTO pattern where the DTO would take in the NHS number and it would be translated into the Id of the domain objects. But to keep things simple, I opeted for keeping the NHS number as the primary identifier. 
  I have not dealt with extensive error handling and logging. Given more time I would have liked to explore forming custom exception objects for different scenarios.
  At the moment there is no checking of the patient while entering appointments. I have assumed that the patient exists in the system. This is another validation I would have liked to add to the system.
