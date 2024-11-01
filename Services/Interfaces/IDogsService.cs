using CodebridgeTest.Models;

namespace CodebridgeTest.Services.Interfaces
{
    public interface IDogsService
    {
        Task CreateDogAsync(Dogs dog);
        Task<IEnumerable<Dogs>> GetDogsAsync(string attribute, string order, int pageNumber, int pageSize);
    }
}
