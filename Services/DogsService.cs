using CodebridgeTest.Models;
using CodebridgeTest.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodebridgeTest.Services
{
    public class DogsService : IDogsService
    {
        private readonly DogDBContext _context;

        public DogsService(DogDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dogs>> GetDogsAsync(string attribute, string order, int pageNumber, int pageSize)
        {
            var query = _context.Dogs.AsQueryable();

            // Sorting logic based on the attribute and order parameters
            query = attribute.ToLower() switch
            {
                "weight" => order == "desc" ? query.OrderByDescending(d => d.Weight) : query.OrderBy(d => d.Weight),
                "tail_length" => order == "desc" ? query.OrderByDescending(d => d.TailLength) : query.OrderBy(d => d.TailLength),
                _ => order == "desc" ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name)
            };

            // Pagination
            return await query.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();
        }

        public async Task CreateDogAsync(Dogs dog)
        {
            // Check if json is valid
            if (dog == null)
            {
                throw new InvalidOperationException("Invalid JSON format.");
            }
            if (dog.TailLength < 0 || dog.Weight < 0)
            {
                throw new InvalidOperationException("Tail length and weight should be positive numbers.");
            }

            // Check if a dog with the same name exists
            if (await _context.Dogs.AnyAsync(d => d.Name == dog.Name))
            {
                throw new InvalidOperationException("A dog with the same name already exists.");
            }

            await _context.Dogs.AddAsync(dog);
            await _context.SaveChangesAsync();
        }
    }

}