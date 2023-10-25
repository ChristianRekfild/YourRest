using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces.Age
{
    public interface IGetAgeRangeByIdUseCase
    {
        Task<AgeRangeWithIdDto> ExecuteAsync(int id, CancellationToken token);
    }
}
