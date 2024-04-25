using IvoryPayAssessment.NearByRestaurants.Models;
using IvoryPayAssessment.NearByRestaurants.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IvoryPayAssessment.NearByRestaurants.Services.Implementations
{
   
        public class RestaurantService : IRestaurantService
        {
            private readonly List<Restaurant> _restaurants; // This could be replaced with a database or external API
        /*{
"name": "Cafe Delight",
"address": "123 Main St, New York, NY",
"latitude": 40.7112,
"longitude": -74.0055
},
        
         
         "name": "Pasta Paradise",
"address": "456 Elm St, New York, NY",
"latitude": 40.7145,
"longitude": -74.0082




        {
"name": "Cafe Delight",
"address": "123 Main St, New York, NY",
"latitude": 40.7112,
"longitude": -74.0055
},
         
         */
        public RestaurantService()
            {
                // Initialize with some sample data
                _restaurants = new List<Restaurant>
        {
            new Restaurant { Id = 1, Name = "Restaurant 1", Latitude = 40.7128, Longitude = -74.0060,Address="" },
            new Restaurant { Id = 2, Name = "Restaurant 2", Latitude = 40.7306, Longitude = -73.9352 },
            
        };
            }

            public List<Restaurant> GetRestaurantsNearby(double latitude, double longitude, double distance)
            {
                // Perform distance calculation and filtering logic here
                // For simplicity, let's assume we're using a simple distance formula

                List<Restaurant> nearbyRestaurants = new List<Restaurant>();
                foreach (var restaurant in _restaurants)
                {
                    double restaurantDistance = CalculateDistance(latitude, longitude, restaurant.Latitude, restaurant.Longitude);
                    if (restaurantDistance <= distance)
                    {
                        nearbyRestaurants.Add(restaurant);
                    }
                }

                return nearbyRestaurants;
            }

            private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
            {
                // This is a simple distance formula, you might want to use a more accurate formula in production
                // For simplicity, let's assume the earth is a perfect sphere
                double dLat = (lat2 - lat1) * Math.PI / 180;
                double dLon = (lon2 - lon1) * Math.PI / 180;
                double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                           Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                           Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                double distance = 6371 * c; // Radius of the earth in km
                return distance;
            }
        }

      
      
}
