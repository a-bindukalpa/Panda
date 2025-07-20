using Domain;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Panda.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController(IAppointmentService service) : ControllerBase
    {
        // POST: api/Appointments
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
        {
           
            await service.AddAsync(appointment);
            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
        }

        // GET: api/Appointments/{Id}
        [HttpGet("{Id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(string Id)
        {
            var appointment = await service.GetAsync(Id);
            if (appointment == null)
                return NotFound();
            return Ok(appointment);
        }

        // PUT: api/Appointments/{Id}
        [HttpPut("{Id}")]
        public async Task<IActionResult> PutAppointment(string Id, Appointment appointment)
        {
            if (Id != appointment.Id)
                return BadRequest("Please check the id of the appointment and the id you are requesting to be changed");

            var updated = await service.UpdateAsync(appointment);
            if (!updated)
                return UnprocessableEntity();

            return NoContent();
        }
    }
}
