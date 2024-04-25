
namespace IvoryPayAssessment.Application.Implementations.ProductCategories
{
  
    public class ProductCategoryService : ResponseBaseService, IProductCategoryService
    {

        private readonly IHttpContextAccessor _httpContext;
        private readonly string _language = string.Empty;
        private readonly IMessageProvider _messageProvider;
        private readonly RestuarantHelper _restuarantHelper;
        private readonly IAppDbContext _context;
        private readonly IDbContextTransaction _trans;

        public ProductCategoryService(IMessageProvider messageProvider, IHttpContextAccessor httpContext, IAppDbContext context, RestuarantHelper restuarantHelper) : base(messageProvider)
        {

            _messageProvider = messageProvider;
            _httpContext = httpContext;
            _language = _httpContext?.HttpContext?.Request?.Headers[ResponseCodes.LANGUAGE];
            _context = context;
            _trans = _context.Begin();
            _restuarantHelper = restuarantHelper;
        }



        public async Task<ServerResponse<bool>> AddCategory( ProductCategoryDTO request)
        {
            var response = new ServerResponse<bool>();
            ValidationResponse err = new ValidationResponse();
            if (!request.IsValid(out err, _messageProvider, _httpContext))
            {
                return SetError(response, err.Code, err.Message, _language);
            }
          
                var data = request.Adapt<ProductCategory>();            
                data.DateCreated = DateTime.Now;
                data.IsActive = true;
              
       var exists=await _context.ProductCategories.AnyAsync(p=>p.Name.ToLower()==data.Name.ToLower());
            if(exists)
            {
                return SetError(response, ResponseCodes.RECORD_EXISTS, _language);
            }

            await _context.ProductCategories.AddAsync(data);
            int save = await _context.SaveChangesAsync();
            if (save > 0 )
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


        public async Task<ServerResponse<List< ProductCategoryViewModel>>> GetAllCategories()
        {
            var response = new ServerResponse<List<ProductCategoryViewModel>>();
            var data = await _context.ProductCategories.Where(p=>p.IsDeleted==false).ProjectToType<ProductCategoryViewModel>().ToListAsync();
            SetSuccess(response, data, ResponseCodes.SUCCESS, _language);
            return response;
        }

        public async Task<ServerResponse<bool>> DeleteCategory(long id)
        {
            var response = new ServerResponse<bool>();


            var category = await _context.Products.FirstOrDefaultAsync(p => p.ProductCategoryId.Equals(id));
            if (category != null)
            {
                return SetError(response, ResponseCodes.ALREADY_IN_USE, _language);
            }

            var data = await _context.ProductCategories.FirstOrDefaultAsync(p => p.Id == id);
            if (data is null)
            {
                return SetError(response, ResponseCodes.INVALID_PARAMETER, _language);
            }
            data.IsActive = false;
            data.IsDeleted = true;
            _context.ProductCategories.Update(data);
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

