using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourRest.Infrastructure.Core.Contracts.Models;

namespace YourRest.Infrastructure.Core.Contracts.Repositories
{
    public interface IAgeRangeRepository : IRepository<AgeRangeDto, int>
    {
    }
}
