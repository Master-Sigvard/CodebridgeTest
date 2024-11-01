using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodebridgeTest.Models;
using CodebridgeTest.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class UnitTest1
{
    private readonly DogDBContext _context;
    private readonly DogsService _dogsService;

    public UnitTest1()
    {
        var options = new DbContextOptionsBuilder<DogDBContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new DogDBContext(options);
        _dogsService = new DogsService(_context);

        // Добавляем тестовые данные
        SeedDatabase();
    }

    private void SeedDatabase()
    {
        var dogs = new List<Dogs>
        {
            new Dogs { Name = "Neo", Color = "red&amber", TailLength = 22, Weight = 32 },
            new Dogs { Name = "Jessy", Color = "black&white", TailLength = 7, Weight = 14 },
        };

        _context.Dogs.AddRange(dogs);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetDogsAsync_SortsByWeightDescending()
    {
        // Act
        var result = await _dogsService.GetDogsAsync("weight", "desc", 1, 10);

        // Assert
        Assert.Equal("Neo", result.First().Name); // Heaviest dog should be first
    }

    [Fact]
    public async Task GetDogsAsync_ReturnsCorrectPage()
    {
        // Act
        var result = await _dogsService.GetDogsAsync("name", "asc", 2, 1);

        // Assert
        Assert.Single(result); // Only one item per page
        Assert.Equal("Jessy", result.First().Name); // Second page item
    }

    [Fact]
    public async Task GetDogsAsync_DefaultSortsByNameAscending()
    {
        // Act
        var result = await _dogsService.GetDogsAsync("name", "asc", 1, 10);

        // Assert
        Assert.Equal("Jessy", result.First().Name); // Sorted by name, ascending
    }

    [Fact]
    public async Task CreateDogAsync_NullDog_ThrowsInvalidOperationException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _dogsService.CreateDogAsync(null));
    }

    [Fact]
    public async Task CreateDogAsync_NegativeTailLengthOrWeight_ThrowsInvalidOperationException()
    {
        // Arrange
        var dogWithNegativeTailLength = new Dogs { Name = "Max", Color = "Black", TailLength = -1, Weight = 10 };
        var dogWithNegativeWeight = new Dogs { Name = "Charlie", Color = "Black", TailLength = 5, Weight = -10 };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _dogsService.CreateDogAsync(dogWithNegativeTailLength));
        await Assert.ThrowsAsync<InvalidOperationException>(() => _dogsService.CreateDogAsync(dogWithNegativeWeight));
    }

    [Fact]
    public async Task CreateDogAsync_DuplicateDogName_ThrowsInvalidOperationException()
    {
        // Arrange
        var duplicateDog = new Dogs { Name = "Neo", Color = "Black", TailLength = 5, Weight = 10 };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _dogsService.CreateDogAsync(duplicateDog));
    }

    [Fact]
    public async Task CreateDogAsync_ValidDog_AddsDogToDatabase()
    {
        // Arrange
        var newDog = new Dogs { Name = "Rocky", Color = "Black", TailLength = 7, Weight = 15 };

        // Act
        await _dogsService.CreateDogAsync(newDog);

        // Assert
        var addedDog = await _context.Dogs.FirstOrDefaultAsync(d => d.Name == "Rocky");
        Assert.NotNull(addedDog);
        Assert.Equal(newDog.TailLength, addedDog.TailLength);
        Assert.Equal(newDog.Weight, addedDog.Weight);
    }


}
