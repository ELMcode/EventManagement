using EventManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Models.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Events
                .Include(e => e.Registrations)
                    .ThenInclude(r => r.Participant)
                .ToListAsync();
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Registrations)
                    .ThenInclude(r => r.Participant)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Participant>> GetAllParticipantsAsync()
        {
            return await _context.Participants.ToListAsync();
        }

        public async Task AddAsync(Event @event)
        {
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Event @event)
        {
            _context.Entry(@event).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();
            }
        }
    }
}
