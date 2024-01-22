using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using YourRest.Infrastructure.Core.Contracts.Entities;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Producer.Infrastructure.ExpressionHelper;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public abstract class PgRepository<TEntity, U, TSource> : IRepository<TSource, U>
        where TEntity : BaseEntity<U>
        where U : notnull
        where TSource : BaseEntityDto<U>
    {
        protected readonly DbContext _dataContext;
        protected readonly IMapper _mapper;

        public PgRepository(DbContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        #region AddAsync
        public async Task<TSource> AddAsync(
            TSource entity,
            bool saveChanges = true,
            CancellationToken cancellationToken = default)
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            Debug.WriteLine("Dto сущность {0}: {1}", typeof(TSource), JsonSerializer.Serialize<TSource>(entity, options));
            var dbEntity = _mapper.Map<TEntity>(entity);

            var linkedEntity = DetachLinkedEntityAsync(dbEntity);

            Debug.WriteLine("Db сущность до вставки {0}: {1}", typeof(TEntity), JsonSerializer.Serialize<TEntity>(dbEntity, options));
            var result = await _dataContext
                .Set<TEntity>()
                .AddAsync(dbEntity, cancellationToken);

            await AttachLinkedEntityAsync(dbEntity, linkedEntity, cancellationToken);

            Debug.WriteLine("Тип result.Entity после вставки {0}: {1}", result.Entity.GetType(), JsonSerializer.Serialize<TEntity>((TEntity)result.Entity, options));
            Debug.WriteLine("Тип dbEntity после вставки {0}: {1}", dbEntity.GetType(), JsonSerializer.Serialize<TEntity>(dbEntity, options));
            if (saveChanges)
            {
                await _dataContext.SaveChangesAsync(cancellationToken);
                result.State = EntityState.Detached;
            }
            entity = _mapper.Map<TSource>(result.Entity);
            Debug.WriteLine("Dto сущность на выходе из метода {0}: {1}", typeof(TSource), JsonSerializer.Serialize<TSource>(entity, options));
            return entity;
        }

        protected virtual IReadOnlyDictionary<string, object> DetachLinkedEntityAsync(TEntity entity)
        {
            return (IReadOnlyDictionary<string, object>)(new Dictionary<string, object>());
        }

        protected virtual Task AttachLinkedEntityAsync(TEntity dbEntity, IReadOnlyDictionary<string, object> linkedEntity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected void SaveLinkedEntityProperty<TField>(TField field, string key, Dictionary<string, object> linkedEntity)
            where TField : BaseEntity<int>
        {
            if (field != null)
            {
                linkedEntity[key] = field;
                field = null;
            }
        }

        protected void SaveLinkedEntityCollection<TField>(ICollection<TField> collection, string key, Dictionary<string, object> linkedEntity)
            where TField : BaseEntity<int>
        {
            if (collection.Any())
            {
                linkedEntity[key] = collection;
                collection = null;
            }
        }

        protected async Task FillEntityCollection<TField>(Action<TField> addAction, string key, IReadOnlyDictionary<string, object> linkedEntity, CancellationToken cancellationToken)
            where TField : BaseEntity<int>
        {
            if (linkedEntity.ContainsKey(key) && linkedEntity[key] != null)
            {
                await _dataContext.SaveChangesAsync(cancellationToken);

                if (linkedEntity[key] is ICollection<TField> linkedCollection)
                {                    
                    var existedLinkedItemIds = linkedCollection
                        .Where(r => r.Id > 0)
                        .Select(r => r.Id);

                    var existedRoomFacilities = await this._dataContext.Set<TField>()
                        //.AsNoTracking()
                        .Where(r => existedLinkedItemIds.Contains(r.Id))
                        .ToListAsync();

                    foreach (var room in existedRoomFacilities)
                    {
                        addAction(room);
                    }
                    foreach (var room in linkedCollection.Where(room => room.Id <= 0))
                    {
                        addAction(room);
                    }
                }
            }
        }

        protected async Task FillEntityField<TField>(TField entityField, int entityFieldId, string key, IReadOnlyDictionary<string, object> linkedEntity, CancellationToken cancellationToken)
             where TField : BaseEntity<int>
        {
            if (linkedEntity.ContainsKey(key) && linkedEntity[key] != null)
            {
                await _dataContext.SaveChangesAsync(cancellationToken);

                if (linkedEntity[key] is TField item)
                {
                    if (entityFieldId <= 0 && item.Id <= 0)
                    {
                        entityField = item;
                    }
                }
            }
        }
        #endregion
        public async Task AddRangeAsync(
            TSource[] entites,
            bool saveChanges = true,
            CancellationToken cancellationToken = default)
        {
            await _dataContext
                .Set<TEntity>()
                .AddRangeAsync(_mapper.Map<IEnumerable<TEntity>>(entites), cancellationToken);

            if (saveChanges)
            {
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TSource>> FindAsync(
            Expression<Func<TSource, bool>> expression,
            CancellationToken cancellationToken = default)
        {            
            var expressionByEntity = expression.ToEntityExpression<TSource, TEntity>();
            var entities = await _dataContext
                .Set<TEntity>()
                .Where(expressionByEntity)
                .ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TSource>>(
                entities
                );
        }

        public async Task<bool> FindAnyAsync(
            Expression<Func<TSource, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            return await _dataContext
                .Set<TEntity>()
                .Where(expression.ToEntityExpression<TSource, TEntity>())
                .AnyAsync(cancellationToken);
        }

        public async Task<IEnumerable<TSource>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _mapper.Map<IEnumerable<TSource>>(
                await _dataContext
                .Set<TEntity>()
                .ToListAsync(cancellationToken)
                );
        }

        public async Task<TSource?> GetAsync(U id, CancellationToken cancellationToken = default)
        {
            return _mapper.Map<TSource>(
                await _dataContext
                .Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken)
                );
        }

        public async Task<IEnumerable<TSource>> GetWithIncludeAsync(
            Expression<Func<TSource, bool>> predicate,
            CancellationToken cancellationToken = default,
            params Expression<Func<TSource, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dataContext.Set<TEntity>().AsNoTracking();

            return _mapper.Map<IEnumerable<TSource>>(
                await Include(query, includeProperties.ToEntityExpression<TSource, TEntity>())
                .Where(predicate.ToEntityExpression<TSource, TEntity>())
                .ToListAsync(cancellationToken));
        }

        public async Task<IEnumerable<TSource>> GetWithIncludeAndTrackingAsync(
            Expression<Func<TSource, bool>> predicate,
            CancellationToken cancellationToken = default,
            params Expression<Func<TSource, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dataContext.Set<TEntity>().AsQueryable();

            return _mapper.Map<IEnumerable<TSource>>(
                await Include(query, includeProperties.ToEntityExpression<TSource, TEntity>())
                .Where(predicate.ToEntityExpression<TSource, TEntity>())
                .ToListAsync(cancellationToken));
        }

        public async Task<IEnumerable<TSource>> GetAllWithIncludeAsync(
            Expression<Func<TSource, object>> includeProperty,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _dataContext.Set<TEntity>().AsNoTracking();

            return _mapper.Map<IEnumerable<TSource>>(
                await Include(query, includeProperty.ToEntityExpression<TSource, TEntity>())
                .ToListAsync(cancellationToken)
                );
        }

        public async Task DeleteAsync(
            U id,
            bool saveChanges = true,
            CancellationToken cancellationToken = default)
        {
            var entity = await _dataContext.Set<TEntity>().FindAsync(id, cancellationToken);

            if (entity != null)
            {
                _dataContext.Set<TEntity>().Remove(entity);

                if (saveChanges)
                {
                    await _dataContext.SaveChangesAsync(cancellationToken);
                }
            }
        }

        public async Task<TSource> UpdateAsync(
            TSource entityDto,
            bool saveChanges = true,
            CancellationToken cancellationToken = default)
        {
            var oldEntity = await _dataContext.Set<TEntity>().FindAsync(entityDto.Id, cancellationToken);

            if (oldEntity != null)
            {
                if(_dataContext.Entry(oldEntity).State == EntityState.Detached)
                {
                    _dataContext.Attach(oldEntity);
                }
                _dataContext.Entry(oldEntity).CurrentValues.SetValues(_mapper.Map<TEntity>(entityDto));
                Debug.WriteLine("_dataContext.Entry(oldEntity).State: {0}", _dataContext.Entry(oldEntity).State);
                if (saveChanges)
                {
                    await _dataContext.SaveChangesAsync(cancellationToken);
                    _dataContext.Entry(oldEntity).State = EntityState.Detached;
                    Debug.WriteLine("_dataContext.Entry(oldEntity).State: {0}", _dataContext.Entry(oldEntity).State);
                }
            }

            return _mapper.Map<TSource>(oldEntity);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dataContext.SaveChangesAsync(cancellationToken);
        }
        private IQueryable<TEntity> Include(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}