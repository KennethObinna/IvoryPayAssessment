
namespace IvoryPayAssessment.Application.Interfacses.Products
{
    public interface IProductService
    {
        Task<ServerResponse<bool>> AddProduct( ProductDTO request);
        Task<ServerResponse<bool>> DeleteProduct(long id);
        Task<ServerResponse<List<ProductModelView>>> GetAllProductByCategorey(long categoryId);
        Task<ServerResponse<List<ProductModelView>>> GetAllProducts();
    }
}