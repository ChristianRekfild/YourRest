﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace YourRest.Domain.Entities
{
    public class City : IntBaseEntity
    {
        public string Name { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}

