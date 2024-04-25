using Azure.Core;
using IvoryPayAssessment.Application.Interfacses.Restuarants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Implementations.Restuarants
{

    public class RestaurantService : ResponseBaseService, IRestaurantService
    {
       
        private readonly IHttpContextAccessor _httpContext;
        private readonly string _language = string.Empty;
        private readonly IMessageProvider _messageProvider;
        private readonly RestuarantHelper _restuarantHelper;
        private readonly IAppDbContext _context;
        private readonly IDbContextTransaction _trans;

        public RestaurantService(IMessageProvider messageProvider, IHttpContextAccessor httpContext, IAppDbContext context, RestuarantHelper restuarantHelper) : base(messageProvider)
        {
            
            _messageProvider = messageProvider;
            _httpContext = httpContext;
            _language = _httpContext?.HttpContext?.Request?.Headers[ResponseCodes.LANGUAGE];
            _context = context;
            _trans = _context.Begin();
            _restuarantHelper = restuarantHelper;
        }



        public async Task<ServerResponse<bool>> CreateRestaurant(RestaurantDTO request)
        {
            var response = new ServerResponse<bool>();
            if (!request.IsValid(out ValidationResponse err, _messageProvider, _httpContext))
            {
                return SetError(response, err.Code, err.Message, _language);
            }
            var data = request.Adapt<Restaurant>();
            if (!_restuarantHelper.IsValidLatitude(data.Latitude))
            {
                return SetError(response, ResponseCodes.INVALID_LATITUDE, _language);
            }
            if (!_restuarantHelper.IsValidLatitude(data.Longitude))
            {
                return SetError(response, ResponseCodes.INVALID_LONGITUDE, _language);
            }
            if (data is null)
            {
                return SetError(response, ResponseCodes.INVALID_OBJECT, _language);
            }
            bool dataExists = await _context.Restaurants.AnyAsync(p => p.Address.ToLower() == request.Address.ToLower() && p.Latitude == request.Latitude && p.Longitude == request.Longitude);

            if (dataExists)
            {
                return SetError(response, ResponseCodes.RECORD_EXISTS, _language);
            }
            data.DateCreated = DateTime.Now;
            data.IsActive = true;
            
            await _context.Restaurants.AddAsync(data);
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

        public async Task<ServerResponse<List<RestaurantModelView>>> GetRestaurants(FetchRestaurantDTO request)
        {
            var response = new ServerResponse<List<RestaurantModelView>>();

            var data = await _context.Restaurants.ProjectToType<RestaurantModelView>().ToListAsync();
            if (data is null)
            {
                return SetError(response, ResponseCodes.NOT_FOUND, _language);

            }
            var feedData = new GetRestaurantDTO
            {
                Distance = request.Distance,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Restuarants = data

            };
            var result = await _restuarantHelper.GetRestaurantsNearby(feedData);
            return result;
        }
        public async Task<ServerResponse<bool>> DeleteRestaurantsById(long id)
        {
            var response = new ServerResponse<bool>();
            var data = await _context.Restaurants.FirstOrDefaultAsync(p => p.Id.Equals(id));

            if (data is null)
            {
                return SetError(response, ResponseCodes.INVALID_PARAMETER, _language); ;
            }
            data.IsDeleted = true;
            data.IsActive = false;
            _context.Restaurants.Update(data);
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
        public async Task<ServerResponse<RestaurantModelView>> GetRestaurant(FetchRestaurantDTO request)
        {
            var response = new ServerResponse<RestaurantModelView>();
            var data = await _context.Restaurants.ProjectToType<RestaurantModelView>().ToListAsync();
            if (data is null)
            {
                return SetError(response, ResponseCodes.NOT_FOUND, _language);

            }
            var feedData = new GetRestaurantDTO
            {
                Distance = request.Distance,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Restuarants = data

            };
            var result = (await _restuarantHelper.GetRestaurantsNearby(feedData)).Data?.PickRandom();
            return SetSuccess(response, result, ResponseCodes.SUCCESS, _language);
        }
        public async Task<ServerResponse<RestaurantModelView>> GetRestaurantById(long id)
        {
            var response = new ServerResponse<RestaurantModelView>();
            var data = (await _context.Restaurants.FirstOrDefaultAsync(p=>p.Id.Equals(id))).Adapt<RestaurantModelView>();
            if (data is null)
            {
                return SetError(response, ResponseCodes.NOT_FOUND, _language);

            }
             
            
            return SetSuccess(response, data, ResponseCodes.SUCCESS, _language);
        }
        public async Task<ServerResponse<bool>> UpdateRestaurant(UpdateRestaurantDTO request)
        {
            var response = new ServerResponse<bool>();
            if (!request.IsValid(out ValidationResponse err, _messageProvider, _httpContext))
            {
                return SetError(response, err.Code, err.Message, _language);
            }
            var data = await _context.Restaurants.FirstOrDefaultAsync(p => p.Id == request.Id);
            if (data is null)
            {
                return SetError(response, ResponseCodes.INVALID_PARAMETER, _language); ;
            }
            bool dataExists = await _context.Restaurants.AnyAsync(p => p.Address.ToLower() == request.Address.ToLower() && p.Latitude == request.Latitude && p.Longitude == request.Longitude && p.Id != request.Id);

            if (dataExists)
            {
                return SetError(response, ResponseCodes.RECORD_EXISTS, _language);
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                data.Name = request.Name;
            }
            if (!string.IsNullOrWhiteSpace(request.Address))
            {
                data.Address = request.Address;
            }

            if (request.Longitude > 0)
            {
                data.Longitude = request.Longitude;
            }

            if (request.Latitude > 0)
            {
                data.Latitude = request.Latitude;
            }
            _context.Restaurants.Update(data);
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
