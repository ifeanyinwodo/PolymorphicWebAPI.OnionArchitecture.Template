using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Persistence.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tactical.DDD;
using Dapper;
using PolymorphicWebAPI.Persistence.Factories;
using PolymorphicWebAPI.Domain.DTO;
using AutoMapper;
using Npgsql;
using Microsoft.Data.SqlClient;
using PolymorphicWebAPI.Domain.Types;
using Domain.DomainEvents;

namespace PolymorphicWebAPI.Persistence.Repositories.ORM
{
    public class DapperStoreOrm : IStoreOrm
    {
        private readonly string EventStoreTableName = TablesDocuments.EventStoreTableName;    
        private static string EventStoreListOfColumnsInsert = "Id, CreatedAt, Version, Name, AggregateId, Data";
        private static readonly string EventStoreListOfColumnsSelect = $"{EventStoreListOfColumnsInsert},Sequence";

        private readonly string NonEventStoreTableName = TablesDocuments.NonEventStoreTableName;
        private static string NonEventStoreListOfColumnsInsert = "Id, CategoryName, Description, Quantity";
        private static readonly string NonEventStoreListOfColumnsSelect = $"{NonEventStoreListOfColumnsInsert}";
        private readonly ConnectionFactory _connectionFactory;
        private readonly DatabaseConfig _config;
       
        private readonly IMapper _mapper;

        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.None,
            NullValueHandling = NullValueHandling.Ignore
        };

        public DapperStoreOrm(IMapper mapper, DatabaseConfig config)
        {

             _config = config;
            _mapper = mapper;
            _connectionFactory = new ConnectionFactory(_config);
            
           
        }


      
        private IDomainEvent TransformEvent(EventStoreDBSet eventSelected)
        {
            var o = JsonConvert.DeserializeObject<DomainEvent>(eventSelected.Data, _jsonSerializerSettings);
            var evt = o as IDomainEvent;

            return evt;
        }


      

        public async Task EventSourcingSaveAsync(ItemCategory itemCategory)
        {
            if (itemCategory.DomainEvents.Count == 0) return;

            var query =
                $@"INSERT INTO {EventStoreTableName} ({EventStoreListOfColumnsInsert})
                    VALUES (@Id,@CreatedAt,@Version,@Name,@AggregateId,@Data);";
            int originatingVersion = itemCategory.Version;
            var listOfEvents = itemCategory.DomainEvents.Select(ev => new
            {
                
                ev.CreatedAt,
                Data = JsonConvert.SerializeObject(ev, Formatting.Indented, _jsonSerializerSettings),
                Id = Guid.NewGuid(),
                ev.GetType().Name,
                AggregateId = itemCategory.Id.ToString(),
                Version = ++originatingVersion
            });

            using (var connection = _connectionFactory.Create(_config.DataBaseType).DBConnection())
            {
                await connection.ExecuteAsync(query, listOfEvents);
            }
        }

        public async Task<ItemCategoryDto> EventSourcingSaveWithResultAsync(ItemCategory itemCategory)
        {
            if (itemCategory.DomainEvents.Count == 0) return null;

            var query =
                $@"INSERT INTO {EventStoreTableName} ({EventStoreListOfColumnsInsert})
                    VALUES (@Id,@CreatedAt,@Version,@Name,@AggregateId,@Data);";
            int originatingVersion = itemCategory.Version;
            var listOfEvents = itemCategory.DomainEvents.Select(ev => new
            {
                Aggregate = 
                ev.CreatedAt,
                Data = JsonConvert.SerializeObject(ev, Formatting.Indented, _jsonSerializerSettings),
                Id = Guid.NewGuid(),
                ev.GetType().Name,
                AggregateId = itemCategory.Id.ToString(),
                Version = ++originatingVersion
            });

            using (var connection = _connectionFactory.Create(_config.DataBaseType).DBConnection())
            {
                await connection.ExecuteAsync(query, listOfEvents);
                var ItemCategoryDto = _mapper.Map<ItemCategoryDto>(new ItemCategory(itemCategory.DomainEvents));
                return ItemCategoryDto;
            }

        }

        public async Task<IQueryable<ItemCategoryDto>> EventSourcingLoadAsync(IEntityId aggregateRootId)
        {
            if (aggregateRootId == null) throw new AggregateRootNotProvidedException("AggregateRootId cannot be null");

            var query = new StringBuilder($@"SELECT {EventStoreListOfColumnsSelect} FROM {EventStoreTableName}");
            query.Append(" WHERE AggregateId = @AggregateId ");
            query.Append(" ORDER BY Version ASC;");
           
            using(var connection = _connectionFactory.Create(_config.DataBaseType).DBConnection())
            {
                var events = (await connection.QueryAsync<EventStoreDBSet>(query.ToString(),  new { AggregateId = aggregateRootId.ToString() } )).ToList();
               
                var itemevents = events.ConvertAll(x => new List<IDomainEvent>() { TransformEvent(x) }.AsReadOnly());

                var itemsresult = itemevents.ConvertAll(x => _mapper.Map<ItemCategoryDto>(new ItemCategory(x)));

                return itemsresult.AsQueryable();
            }
        }

        public async Task<IQueryable<ItemCategoryDto>> EventSourcingLoadAllAsync()
        {
            var query = new StringBuilder($@"SELECT {EventStoreListOfColumnsSelect} FROM {EventStoreTableName}");
            query.Append(" ORDER BY Version ASC;");


            using (var connection = _connectionFactory.Create(_config.DataBaseType).DBConnection())
            {
               
                var events = (await connection.QueryAsync<EventStoreDBSet>(query.ToString(), null)).ToList();
                var itemevents = events.ConvertAll(x => new List<IDomainEvent>() { TransformEvent(x) }.AsReadOnly());

                var itemsresult = itemevents.ConvertAll(x => _mapper.Map<ItemCategoryDto>(new ItemCategory(x)));
                return itemsresult.AsQueryable();


            }
        }

        public async Task NonEventSourcingSaveAsync(ItemCategoryDto ItemCategoryDto)
        {
            if(ItemCategoryDto == null) return;

            var query =
                $@"INSERT INTO {NonEventStoreTableName} ({NonEventStoreListOfColumnsInsert})
                    VALUES (@Id,@CategoryName,@Description,@Quantity);";


            using (var connection = _connectionFactory.Create(_config.DataBaseType).DBConnection())
            {

                await connection.ExecuteAsync(query, new { Id = Guid.Parse(ItemCategoryDto.Id), CategoryName = ItemCategoryDto.CategoryName, Description = ItemCategoryDto.Description, Quantity = ItemCategoryDto.Quantity });
            }
        }

        public async Task<ItemCategoryDto> NonEventSourcingSaveWithResultAsync(ItemCategoryDto ItemCategoryDto)
        {
            if (ItemCategoryDto == null) return null;

            var query =
                $@"INSERT INTO {NonEventStoreTableName} ({NonEventStoreListOfColumnsInsert})
                    VALUES (@Id,@CategoryName,@Description,@Quantity);";


            using (var connection = _connectionFactory.Create(_config.DataBaseType).DBConnection())
            {

                await connection.ExecuteAsync(query, new { Id = Guid.Parse(ItemCategoryDto.Id), CategoryName = ItemCategoryDto.CategoryName, Description = ItemCategoryDto.Description, Quantity = ItemCategoryDto.Quantity });
                return new ItemCategoryDto { Id = Guid.Parse(ItemCategoryDto.Id).ToString(), CategoryName = ItemCategoryDto.CategoryName, Description = ItemCategoryDto.Description, Quantity = ItemCategoryDto.Quantity };
            }
        }

        public async Task<IQueryable<ItemCategoryDto>> NonEventSourcingLoadAsync(IEntityId aggregateRootId)
        {
            if (aggregateRootId == null) throw new IdException("Id cannot be null");

            var query = new StringBuilder($@"SELECT {NonEventStoreListOfColumnsSelect} FROM {NonEventStoreTableName}");
            query.Append(" WHERE Id = @Id ;");



            using (var connection = _connectionFactory.Create(_config.DataBaseType).DBConnection())
            {
                var items = (await connection.QueryAsync<ItemCategoryStoreDBSet>(query.ToString(),  new { Id = aggregateRootId.ToString() } )).ToList();
               

                var selectedItem = _mapper.Map<List<ItemCategoryStoreDBSet>, List<ItemCategoryDto>>(items);

                return selectedItem.AsQueryable();
            }
        }

        public async Task<IQueryable<ItemCategoryDto>> NonEventSourcingLoadAllAsync()
        {
            var query = new StringBuilder($@"SELECT {NonEventStoreListOfColumnsSelect} FROM {NonEventStoreTableName}");
            query.Append(" ORDER BY CategoryName ASC;");


            using (var connection = _connectionFactory.Create(_config.DataBaseType).DBConnection())
            {

                var items = (await connection.QueryAsync<ItemCategoryStoreDBSet>(query.ToString(), null)).ToList();

                var itemsq = _mapper.Map<List<ItemCategoryStoreDBSet>, List<ItemCategoryDto>>(items);

                return itemsq.AsQueryable();
            }
        }

        public async Task<ItemCategory> EventSourcingLoadEventAsync(IEntityId aggregateRootId)
        {
            if (aggregateRootId == null) throw new AggregateRootNotProvidedException("AggregateRootId cannot be null");

            var query = new StringBuilder($@"SELECT {EventStoreListOfColumnsSelect} FROM {EventStoreTableName}");
            query.Append(" WHERE AggregateId = @AggregateId ");
            query.Append(" ORDER BY Version ASC;");


            using (var connection = _connectionFactory.Create(_config.DataBaseType).DBConnection())
            {
                var events = (await connection.QueryAsync<EventStoreDBSet>(query.ToString(),  new { AggregateId = aggregateRootId.ToString() } )).ToList();
                var domainEvents = events.Select(TransformEvent).Where(x => x != null).ToList().AsReadOnly();
                var itemCategory = new ItemCategory(domainEvents);
                return itemCategory;
            }
        }

        public async Task<ItemCategoryDto> NonEventSourcingSaveUpdate(string id, string categoryName, string description, int quantity)
        {
            var ItemCategoryDto = new ItemCategoryDto{Id = id, CategoryName = categoryName, Description = description, Quantity = quantity};
            
            var query =
                $@" UPDATE  {NonEventStoreTableName} 
                    SET CategoryName=@CategoryName, Description=@Description, Quantity=@Quantity where Id=@Id;";


            using (var connection = _connectionFactory.Create(_config.DataBaseType).DBConnection())
            {

                await connection.ExecuteAsync(query, new { Id = Guid.Parse(ItemCategoryDto.Id), CategoryName = ItemCategoryDto.CategoryName, Description = ItemCategoryDto.Description, Quantity = ItemCategoryDto.Quantity });
                return ItemCategoryDto;
            }
        }

        public async Task NonEventSourcingRemove(string id)
        {
            var query =
               $@" DELETE FROM  {NonEventStoreTableName} 
                     where Id=@Id;";


            using (var connection = _connectionFactory.Create(_config.DataBaseType).DBConnection())
            {

                await connection.ExecuteAsync(query, new { Id = Guid.Parse(id)});
               
            }
        }

      
    }
}
