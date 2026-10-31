namespace Domain;

public class BaseEntity
{
    //SQL Server sorts uniqueidentifier by its last 6 bytes, so v7 inserts land in random places and fragment the index.
    public Guid Id  { get; private set; } = Guid.CreateVersion7();
    public DateTime CreatedOn { get; set; }
}

public class AuditableEntity: BaseEntity
{
    public DateTime LastModifiedOn { get; set; }
}