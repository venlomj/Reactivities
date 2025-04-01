using Application.Core;

namespace Application.UseCases.Activities.Queries
{
    public class ActivityParams : PaginationParams<DateTime?>
    {
        public string? Filter { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
    }
}
