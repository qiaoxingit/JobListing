namespace AuthService.Repositories.Entities;

public class Job
{
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public DateTime PostedDate { get; set; }

    public DateTime ExpireDate { get; set; }

    public Guid PostedByUser { get; set; }
}
