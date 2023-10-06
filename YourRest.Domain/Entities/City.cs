using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourRest.Domain.Entities
{
    public class City : IntBaseEntity
    {
        public City() => Addresses = new List<Address>();
        
        public string Name { get; set; }
        // Navigation property
        public ICollection<Address> Addresses { get; set; }
    }
}

