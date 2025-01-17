namespace EventManagement.Models.Repositories
{
    public interface IEventStatsRepository
    {
        Task<EventStats> GetEventStatsAsync(int eventId);
        Task UpsertEventStatsAsync(int eventId, int totalRegistrations, decimal totalRevenue);
    }
}
