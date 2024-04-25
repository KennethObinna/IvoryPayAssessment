using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Domain.Entities
{
    public class Product:BaseObject
    {
        public long Id { get; set; }
        public string Name { get; set; }     
        public string Description {  get; set; }
        public long? ProductCategoryId {  get; set; }
        public decimal Price { get; set; }
        public int Quantity {  get; set; }

    }
}
