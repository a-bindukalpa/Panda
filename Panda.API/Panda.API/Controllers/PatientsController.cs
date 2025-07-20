using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Panda.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController(IPatientRepository repository) : ControllerBase
    {
        // POST: api/Patients
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            if (!Patient.IsValidNHSNumber(patient.NhsNumber))
                return BadRequest("Invalid NHS number checksum.");
            var existingPatient = await repository.GetAsync(patient.NhsNumber);

            if (existingPatient != null)
                throw new InvalidOperationException("A patient with this NHS number already exists.");
            
            await repository.AddAsync(patient);
            return CreatedAtAction(nameof(GetPatient), new { nhsNumber = patient.NhsNumber }, patient);
        }

        // GET: api/Patients/{nhsNumber}
        [HttpGet("{nhsNumber}")]
        public async Task<ActionResult<Patient>> GetPatient(string nhsNumber)
        {
            var patient = await repository.GetAsync(nhsNumber);
            if (patient == null)
                return NotFound();
            return Ok(patient);
        }
        
        // PUT: api/Patients/{nhsNumber}
        [HttpPut("{nhsNumber}")]
        public async Task<IActionResult> PutPatient(string nhsNumber, Patient patient)
        {
            if (nhsNumber != patient.NhsNumber)
                return BadRequest("NHS number mismatch.");

            var updated = await repository.UpdateAsync(patient);
            
            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Patients/{nhsNumber}
        [HttpDelete("{nhsNumber}")]
        public async Task<IActionResult> DeletePatient(string nhsNumber)
        {
            var deleted = await repository.DeleteAsync(nhsNumber);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
