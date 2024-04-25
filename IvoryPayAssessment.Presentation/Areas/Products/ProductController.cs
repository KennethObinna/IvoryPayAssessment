using IvoryPayAssessment.Application.Interfacses.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IvoryPayAssessment.Presentation.Areas.Products
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService??throw new ArgumentNullException(nameof(productService));
        }
        /// <summary>
        /// An API to add product to th emenu
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ServerResponse<bool>), StatusCodes.Status200OK)] 
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]  ProductDTO request)
        {
            var result = await _productService.AddProduct(request);

            if (result.IsSuccessful)
            {
                return Created(nameof(Create), result);
            }
            return BadRequest(result.Error);
        }
        /// <summary>
        /// An API to get all products of a category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ServerResponse<List<ProductModelView>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpGet("get-product-by-category-id/{categoryId}")]
        public async Task<IActionResult>  GetAllProductByCategorey([FromRoute] long categoryId)
        {
            var result = await _productService.GetAllProductByCategorey(categoryId);

            if (result.IsSuccessful)
            {
                return Created(nameof(Create), result);
            }
            return BadRequest(result.Error);
        }
        /// <summary>
        /// AN API to get all the active products
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(ServerResponse<List<ProductModelView>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpGet("get-all-product")]
        public async Task<IActionResult> Create()
        {
            var result = await _productService.GetAllProducts();

            if (result.IsSuccessful)
            {
                return Created(nameof(Create), result);
            }
            return BadRequest(result.Error);
        }
        /// <summary>
        /// An API to delete a particular Product (Archive the record)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [ProducesResponseType(typeof(ServerResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            var result = await _productService.DeleteProduct(id);

            if (result.IsSuccessful)
            {
                return Created(nameof(Create), result);
            }
            return BadRequest(result.Error);
        }


    }
}
