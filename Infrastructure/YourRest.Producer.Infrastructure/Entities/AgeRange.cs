using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourRest.Producer.Infrastructure.Entities
{
    public class AgeRange: IntBaseEntity
    {
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
    }
}
