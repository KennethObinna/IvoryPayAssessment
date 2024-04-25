using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Interfacses.Restuarants
{
    public interface IRestaurantService
    {
        Task<ServerResponse<bool>> CreateRestaurant(RestaurantDTO request);
        Task<ServerResponse<bool>> DeleteRestaurantsById(long id);
        Task<ServerResponse<List<RestaurantModelView>>> GetRestaurants(FetchRestaurantDTO request);
        Task<ServerResponse<RestaurantModelView>> GetRestaurant(FetchRestaurantDTO request);
        Task<ServerResponse<bool>> UpdateRestaurant(UpdateRestaurantDTO request);
        Task<ServerResponse<RestaurantModelView>> GetRestaurantById(long id);
    }
}
