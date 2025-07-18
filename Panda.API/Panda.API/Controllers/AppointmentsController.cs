using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Panda.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentsController(IAppointmentRepository repository)
        {
            _repository = repository;
        }
        // POST: api/Appointments
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
        {
           var existingAppointment = await _repository.GetAsync(appointment.Id);

            await _repository.AddAsync(appointment);
            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
        }

        // GET: api/Appointments/{Id}
        [HttpGet("{Id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(string Id)
        {
            var Appointment = await _repository.GetAsync(Id);
            if (Appointment == null)
                return NotFound();
            return Ok(Appointment);
        }


        // PUT: api/Appointments/{Id}
        [HttpPut("{Id}")]
        public async Task<IActionResult> PutAppointment(string Id, Appointment Appointment)
        {
            if (Id != Appointment.Id)
                return BadRequest("Please check the id of the appointment and the id you are requesting to be changed");

            var updated = await _repository.UpdateAsync(Appointment);
            if (!updated)
                return NotFound();

            return NoContent();
        }
    }
}
