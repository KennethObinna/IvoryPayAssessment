﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Domain.Entities
{
    public class ProductCategory:BaseObject
    {
        public long Id { get; set; }    
        public string Name { get; set; }  
        public string Description { get; set; }


    }
}
