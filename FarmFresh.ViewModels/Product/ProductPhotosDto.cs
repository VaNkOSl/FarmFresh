namespace FarmFresh.ViewModels.Product;

public record ProductPhotosDto(
    Guid Id,
    string FilePath,
    byte[]? Photo,
    Guid ProductId);
