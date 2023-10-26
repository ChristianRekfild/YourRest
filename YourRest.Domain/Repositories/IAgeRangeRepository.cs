using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourRest.Domain.Entities;

namespace YourRest.Domain.Repositories
{
    public interface IAgeRangeRepository : IRepository<AgeRange, int>
    {
    }
}
