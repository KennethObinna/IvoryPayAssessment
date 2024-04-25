using IvoryPayAssessment.Application.Interfacses.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Implementations.Products
{
    public class ProductService : ResponseBaseService, IProductService
    {
    
        private readonly IHttpContextAccessor _httpContext;
        private readonly string _language = string.Empty;
        private readonly IMessageProvider _messageProvider;
        private readonly RestuarantHelper _restuarantHelper;
        private readonly IAppDbContext _context;
        private readonly IDbContextTransaction _trans;

        public ProductService(IMessageProvider messageProvider, IHttpContextAccessor httpContext, IAppDbContext context, RestuarantHelper restuarantHelper) : base(messageProvider)
        {

            _messageProvider = messageProvider;
            _httpContext = httpContext;
            _language = _httpContext?.HttpContext?.Request?.Headers[ResponseCodes.LANGUAGE];
            _context = context;
            _trans = _context.Begin();
            _restuarantHelper = restuarantHelper;
        }



        public async Task<ServerResponse<bool>> AddProduct( ProductDTO request)
        {
            var response = new ServerResponse<bool>();
            ValidationResponse err = new ValidationResponse();
            if (!request.IsValid(out err, _messageProvider, _httpContext))
            {
                return SetError(response, err.Code, err.Message, _language);
            }
            
                var data = request.Adapt<Product>();
                data.DateCreated = DateTime.Now;
                data.IsActive = true;

            var exists = await _context.Products.AnyAsync(p => p.Name.ToLower() == data.Name.ToLower());
            if (exists)
            {
                return SetError(response, ResponseCodes.RECORD_EXISTS, _language);
            }



            await _context.Products.AddAsync(data);
            int save = await _context.SaveChangesAsync();
            if (save > 0  )
            {
                await _trans.CommitAsync();
                SetSuccess(response, true, ResponseCodes.SUCCESS, _language);
            }
            else
            {
                SetError(response, ResponseCodes.REQUEST_NOT_SUCCESSFUL, _language);
            }

            return response;

        }
        public async Task<ServerResponse<List<ProductModelView>>> GetAllProductByCategorey(long categoryId)
        {
            var response = new ServerResponse<List<ProductModelView>>();
            var data = await (from p in _context.Products
                              join c in _context.ProductCategories on p.ProductCategoryId equals c.Id
                              where c.Id == categoryId && p.IsDeleted == false
                              select new ProductModelView
                              {
                                  Category = c.Name,
                                  Description = p.Description,
                                  Name = p.Name,
                                  Price = p.Price,
                                  Quantity = p.Quantity, Id = p.Id
                              }).ToListAsync();
            SetSuccess(response, data, ResponseCodes.SUCCESS, _language);
            return response;

        }
        public async Task<ServerResponse<List<ProductModelView>>> GetAllProducts()
        {
            var response = new ServerResponse<List<ProductModelView>>();
            var data = await _context.Products.Where(p=>p.IsDeleted==false).ProjectToType<ProductModelView>().ToListAsync();
            SetSuccess(response, data, ResponseCodes.SUCCESS, _language);
            return response;
        }

        public async Task<ServerResponse<bool>> DeleteProduct(long id)
        {
            var response = new ServerResponse<bool>();


            var data = await _context.Products.FirstOrDefaultAsync(p => p.Id.Equals(id));
            if (data is null)
            {
                return SetError(response, ResponseCodes.INVALID_PARAMETER, _language);
            }
            data.IsActive = false;
            data.IsDeleted = true;
            _context.Products.Update(data);
            int save = await _context.SaveChangesAsync();
            if (save > 0)
            {
                await _trans.CommitAsync();
                SetSuccess(response, true, ResponseCodes.SUCCESS, _language);
            }
            else
            {
                SetError(response, ResponseCodes.REQUEST_NOT_SUCCESSFUL, _language);
            }

            return response;

        }
    }
}
