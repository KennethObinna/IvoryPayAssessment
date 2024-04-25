using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Interfacses.ProductCategories
{
    public interface IProductCategoryService
    {
        Task<ServerResponse<bool>> AddCategory( ProductCategoryDTO request);
        Task<ServerResponse<bool>> DeleteCategory(long id);
        Task<ServerResponse<List<ProductCategoryViewModel>>> GetAllCategories();
    }

}
