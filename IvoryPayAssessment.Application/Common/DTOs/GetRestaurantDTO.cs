using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.DTOs
{
    public class GetRestaurantDTO
    {
        public List<RestaurantModelView> Restuarants { get; set; }
        public double Longitude {  get; set; }
        public double Latitude {  get; set; }
        public int  Distance {  get; set; }
    }
}
