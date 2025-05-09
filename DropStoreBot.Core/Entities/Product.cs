namespace DropStoreBot.Core.Entities;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true;

    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;
}
