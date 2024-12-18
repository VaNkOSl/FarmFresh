namespace FarmFresh.ViewModels.Product;

public record UpdateProductDto : ProductForManipulationDto
{
    public List<ProductPhotosDto> CurrentPhotos {  get; set; } = new List<ProductPhotosDto>();
}
