using AutoMapper;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Producer.Infrastructure.DbContexts;
using YourRest.Producer.Infrastructure.Entities;

namespace YourRest.Producer.Infrastructure.Repositories
{
    internal class UserAccommodationRepository : PgRepository<UserAccommodation, int, UserAccommodationDto>, IUserAccommodationRepository
    {
        public UserAccommodationRepository(SharedDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
        }

        #region AddAsync
        protected override IReadOnlyDictionary<string, object> DetachLinkedEntityAsync(UserAccommodation entity)
        {
            Dictionary<string, object> linkedEntity = new();
                        
            SaveLinkedEntityProperty(entity.Accommodation, "Accommodation", linkedEntity);

            SaveLinkedEntityProperty(entity.User, "User", linkedEntity);
            
            IReadOnlyDictionary<string, object> result = linkedEntity;
            return result;
        }

        protected override async Task AttachLinkedEntityAsync(UserAccommodation entity, IReadOnlyDictionary<string, object> linkedEntity, CancellationToken cancellationToken)
        {
            await FillEntityField(entity.Accommodation, entity.AccommodationId, "Accommodation", linkedEntity, cancellationToken);

            await FillEntityField(entity.User, entity.UserId, "User", linkedEntity, cancellationToken);
        }
        #endregion
    }
}
