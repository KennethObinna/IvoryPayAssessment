using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IvoryPayAssessment.Application.Helpers;

namespace IvoryPayAssessment.Application.Common.Helpers
{
    public class RestuarantHelper : ResponseBaseService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly string _language = string.Empty;

        public RestuarantHelper(IMessageProvider messageProvider, IHttpContextAccessor httpContext) : base(messageProvider)
        {
            _httpContext = httpContext;
            _language = _httpContext?.HttpContext?.Request?.Headers[ResponseCodes.LANGUAGE];
        }

        public async Task<ServerResponse<List<RestaurantModelView>>> GetRestaurantsNearby(GetRestaurantDTO request)
        {
            // Perform distance calculation and filtering logic here
            // For simplicity, let's assume we're using a simple distance formula


            var response = new ServerResponse<List<RestaurantModelView>>();
            List<RestaurantModelView> nearbyRestaurants = new List<RestaurantModelView>();
            foreach (var restaurant in request.Restuarants)
            {

                double restaurantDistance = Utilities.CalculateDistance(request.Latitude, request.Longitude, restaurant.Latitude, restaurant.Longitude);
                if (restaurantDistance <= request.Distance)
                {
                    nearbyRestaurants.Add(restaurant.Adapt<RestaurantModelView>());
                }
            }

            if (nearbyRestaurants != null && nearbyRestaurants.Count > 0)
            {
                SetSuccess(response, nearbyRestaurants, ResponseCodes.SUCCESS, _language);
            }
            else
            {
                SetError(response, ResponseCodes.NOT_FOUND, _language);
            }

            return response;
        }

        public bool IsValidLatitude(double latitude)
        {
            // Check if latitude is within the valid range (-90 to +90 degrees)
            return latitude >= -90 && latitude <= 90;
        }
        public bool IsValidLongitude(double longitude)
        {
            // Check if longitude is within the valid range (-180 to +180 degrees)
            return longitude >= -180 && longitude <= 180;
        }
   
    }
}
