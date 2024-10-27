namespace FarmFresh.Data.Models;

public class ProductPhoto
{
    public ProductPhoto()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }

    public string FilePath { get; set; } = string.Empty;

    public byte[] Photo { get; set; } = new byte[0];

    public Guid ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
