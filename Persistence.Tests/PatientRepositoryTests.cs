using Domain;
using MongoDB.Driver;
using Moq;
using Persistence;
using Persistence.Repositories;

namespace RepositoryTests;

public class PatientRepositoryTests
{
    private static Patient GetSamplePatient() => new()
    {
        Id = "1",
        NhsNumber = "1234567890",
        Name = "John Doe",
        DateOfBirth = new DateTime(1980, 1, 1),
        PostCode = "AB12 3CD"
    };

    [Fact]
    public async Task GetAsync_WhenAvailable_ReturnsPatient()
    {
        var sample = GetSamplePatient();
        var mockAdapter = new Mock<IMongoCollectionAdapter<Patient>>();
        mockAdapter.Setup(m =>
            m.FindOneAsync(It.Is<FilterDefinition<Patient>>(f => true)))
                   .ReturnsAsync(sample);

        var repo = new PatientRepository(mockAdapter.Object);
        var result = await repo.GetAsync(sample.NhsNumber);

        Assert.NotNull(result);
        Assert.Equal(sample.NhsNumber, result!.NhsNumber);
    }

    [Fact]
    public async Task AddAsync_CallsInsert()
    {
        var sample = GetSamplePatient();
        var mockAdapter = new Mock<IMongoCollectionAdapter<Patient>>();
        mockAdapter.Setup(m => m.InsertOneAsync(sample))
                   .Returns(Task.CompletedTask)
                   .Verifiable();

        var repo = new PatientRepository(mockAdapter.Object);
        await repo.AddAsync(sample);

        mockAdapter.Verify(m => m.InsertOneAsync(sample), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsTrueWhenModified()
    {
        var sample = GetSamplePatient();
        var mockAdapter = new Mock<IMongoCollectionAdapter<Patient>>();
        mockAdapter.Setup(m => m.ReplaceOneAsync(
                    It.IsAny<FilterDefinition<Patient>>(), sample))
                   .ReturnsAsync(true);

        var repo = new PatientRepository(mockAdapter.Object);
        var result = await repo.UpdateAsync(sample);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrueWhenDeleted()
    {
        var mockAdapter = new Mock<IMongoCollectionAdapter<Patient>>();
        mockAdapter.Setup(m => m.DeleteOneAsync(It.IsAny<FilterDefinition<Patient>>()))
                   .ReturnsAsync(true);

        var repo = new PatientRepository(mockAdapter.Object);
        var result = await repo.DeleteAsync("1234567890");

        Assert.True(result);
    }
}
