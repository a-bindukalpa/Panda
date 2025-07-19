using Domain;
using Service;
using Moq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace ServiceTests
{
    public class AppointmentServiceTests
    {
        private Appointment GetValidAppointment(string id = "A1", string status = "Active")
        {
            return new Appointment
            {
                Id = id,
                Status = status,
                Time = DateTime.UtcNow.AddHours(1),
                Duration = "1h",
                Clinician = "Dr Smith",
                Department = "Cardiology",
                PostCode = "AB12 3CD"
            };
        }

        [Fact]
        public async Task AddAsync_ValidAppointment_CallsRepository()
        {
            var repoMock = new Mock<IAppointmentRepository>();
            repoMock.Setup(r => r.AddAsync(It.IsAny<Appointment>())).Returns(Task.CompletedTask);

            var service = new AppointmentService(repoMock.Object);
            var appointment = GetValidAppointment();

            await service.AddAsync(appointment);

            repoMock.Verify(r => r.AddAsync(appointment), Times.Once);
        }

        [Fact]
        public async Task AddAsync_InvalidStatus_ThrowsValidationException()
        {
            var repoMock = new Mock<IAppointmentRepository>();
            var service = new AppointmentService(repoMock.Object);
            var appointment = GetValidAppointment(status: "InvalidStatus");

            await Assert.ThrowsAsync<ValidationException>(() => service.AddAsync(appointment));
        }

        [Fact]
        public async Task UpdateAsync_ValidAppointment_ReturnsTrue()
        {
            var repoMock = new Mock<IAppointmentRepository>();
            var appointment = GetValidAppointment();
            repoMock.Setup(r => r.GetAsync(appointment.Id)).ReturnsAsync(appointment);
            repoMock.Setup(r => r.UpdateAsync(appointment)).ReturnsAsync(true);

            var service = new AppointmentService(repoMock.Object);

            var result = await service.UpdateAsync(appointment);

            Assert.True(result);
            repoMock.Verify(r => r.UpdateAsync(appointment), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NonExistentAppointment_ReturnsFalse()
        {
            var repoMock = new Mock<IAppointmentRepository>();
            var appointment = GetValidAppointment();
            repoMock.Setup(r => r.GetAsync(appointment.Id)).ReturnsAsync((Appointment)null);

            var service = new AppointmentService(repoMock.Object);

            var result = await service.UpdateAsync(appointment);

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAsync_ReinstateCancelledAppointment_ReturnsFalse()
        {
            var repoMock = new Mock<IAppointmentRepository>();
            var cancelledAppointment = GetValidAppointment(status: "Cancelled");
            var newAppointment = GetValidAppointment(status: "Pending");
            repoMock.Setup(r => r.GetAsync(newAppointment.Id)).ReturnsAsync(cancelledAppointment);

            var service = new AppointmentService(repoMock.Object);

            var result = await service.UpdateAsync(newAppointment);

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAsync_InvalidStatus_ReturnsFalse()
        {
            var repoMock = new Mock<IAppointmentRepository>();
            var appointment = GetValidAppointment(status: "InvalidStatus");
            repoMock.Setup(r => r.GetAsync(appointment.Id)).ReturnsAsync(appointment);

            var service = new AppointmentService(repoMock.Object);

            var result = await service.UpdateAsync(appointment);

            Assert.False(result);
        }
    }
}