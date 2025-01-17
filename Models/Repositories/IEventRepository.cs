using EventManagement.Models;

namespace EventManagement.Models.Repositories
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(int id);
        Task<IEnumerable<Participant>> GetAllParticipantsAsync();
        Task AddAsync(Event @event);
        Task UpdateAsync(Event @event);
        Task DeleteAsync(int id);
    }
}
