using IvoryPayAssessment.Application.Interfacses.ProductCategories;
using IvoryPayAssessment.Application.Interfacses.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IvoryPayAssessment.Presentation.Areas.Categories
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly IProductCategoryService _productCategoryService;

        public CategoriesController(IProductService productService, IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService ?? throw new ArgumentNullException(nameof(productCategoryService));
        }
        /// <summary>
        /// An api to add the product category
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [ProducesResponseType(typeof(ServerResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ProductCategoryDTO request)
        {
            var result = await _productCategoryService.AddCategory(request);

            if (result.IsSuccessful)
            {
                return Created(nameof(Create), result);
            }
            return BadRequest(result.Error);
        }
        //An API to get all the product categories
        [ProducesResponseType(typeof(ServerResponse<List<ProductCategoryViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpGet("get-category")]
        public async Task<IActionResult>  GetAllCategories()
        {
            var result = await _productCategoryService.GetAllCategories();

            if (result.IsSuccessful)
            {
                return Created(nameof(Create), result);
            }
            return BadRequest(result.Error);
        }
       
        /// <summary>
        /// An API to remove a particular category form the systrem
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ServerResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            var result = await _productCategoryService.DeleteCategory(id);

            if (result.IsSuccessful)
            {
                return Created(nameof(Create), result);
            }
            return BadRequest(result.Error);
        }


    }

}
