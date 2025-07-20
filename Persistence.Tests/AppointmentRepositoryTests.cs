using Domain;
using Moq;
using MongoDB.Driver;
using Persistence;

namespace RepositoryTests
{
    public class AppointmentRepositoryTests
    {
        private Appointment GetSampleAppointment()
        {
            return new Appointment
            {
                Id = "A1",
                Status = "Confirmed",
                Time = new DateTime(2025, 7, 18, 10, 0, 0),
                Duration = "01:30",
                Clinician = "Dr Smith",
                Department = "Cardiology",
                PostCode = "AB12 3CD"
            };
        }

        [Fact]
        public async Task GetAsync_ReturnsAppointment_WhenFound()
        {
            var appointment = GetSampleAppointment();
            var mockAdapter = new Mock<IMongoCollectionAdapter<Appointment>>();
            mockAdapter.Setup(a => a.FindOneAsync(It.IsAny<FilterDefinition<Appointment>>()))
                .ReturnsAsync(appointment);

            var repo = new AppointmentRepository(mockAdapter.Object);

            var result = await repo.GetAsync("A1");

            Assert.NotNull(result);
            Assert.Equal("A1", result.Id);
        }

        [Fact]
        public async Task GetAsync_ReturnsNull_WhenNotFound()
        {
            var mockAdapter = new Mock<IMongoCollectionAdapter<Appointment>>();
            mockAdapter.Setup(a => a.FindOneAsync(It.IsAny<FilterDefinition<Appointment>>()))
                .ReturnsAsync((Appointment?)null);

            var repo = new AppointmentRepository(mockAdapter.Object);

            var result = await repo.GetAsync("A2");

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_CallsInsertOneAsync()
        {
            var appointment = GetSampleAppointment();
            var mockAdapter = new Mock<IMongoCollectionAdapter<Appointment>>();
            mockAdapter.Setup(a => a.InsertOneAsync(appointment))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var repo = new AppointmentRepository(mockAdapter.Object);

            await repo.AddAsync(appointment);

            mockAdapter.Verify(a => a.InsertOneAsync(appointment), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_CallsReplaceOneAsync_AndReturnsTrue()
        {
            var appointment = GetSampleAppointment();
            var mockAdapter = new Mock<IMongoCollectionAdapter<Appointment>>();
            mockAdapter.Setup(a => a.ReplaceOneAsync(It.IsAny<FilterDefinition<Appointment>>(), appointment))
                .ReturnsAsync(true);

            var repo = new AppointmentRepository(mockAdapter.Object);

            var result = await repo.UpdateAsync(appointment);

            Assert.True(result);
            mockAdapter.Verify(a => a.ReplaceOneAsync(It.IsAny<FilterDefinition<Appointment>>(), appointment), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_CallsReplaceOneAsync_AndReturnsFalse()
        {
            var appointment = GetSampleAppointment();
            var mockAdapter = new Mock<IMongoCollectionAdapter<Appointment>>();
            mockAdapter.Setup(a => a.ReplaceOneAsync(It.IsAny<FilterDefinition<Appointment>>(), appointment))
                .ReturnsAsync(false);

            var repo = new AppointmentRepository(mockAdapter.Object);

            var result = await repo.UpdateAsync(appointment);

            Assert.False(result);
            mockAdapter.Verify(a => a.ReplaceOneAsync(It.IsAny<FilterDefinition<Appointment>>(), appointment), Times.Once);
        }
    }
}