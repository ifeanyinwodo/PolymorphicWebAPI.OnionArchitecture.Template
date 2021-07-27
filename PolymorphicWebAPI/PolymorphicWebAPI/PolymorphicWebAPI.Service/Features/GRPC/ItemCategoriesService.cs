using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Persistence.Repositories;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests;
using MediatR;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Service.Features.GRPC
{
    public class ItemCategoriesService : ItemCategories.ItemCategoriesBase
    {
        private readonly IMediator _mediator;
        public readonly IStoreOrmRepository _storeORMRepository;
        
        public ItemCategoriesService(IMediator mediator, IStoreOrmRepository storeORMRepository, IMapper mapper)
        {
            _mediator = mediator;
            _storeORMRepository = storeORMRepository;
            
            
            
        }

        public override async Task<GetAllCategories> Get(Empty request, IServerStreamWriter<GetAllCategories> responseStream, ServerCallContext context)
        {
           
            var dtoObject = await _mediator.Send(new GetAllCategoryRequest());
           
            var allCategory = dtoObject.Select(grp => new  Category
            {
                Id = grp.Id,
                CategoryName = grp.CategoryName,
                Description = grp.Description,
                Quantity = grp.Quantity

            }).ToList();
            GetAllCategories getAllCategories = new GetAllCategories();
            getAllCategories.ItemCategories.AddRange(allCategory);
            await responseStream.WriteAsync(getAllCategories);

            return await Task.FromResult(getAllCategories);
        }
        public override async Task<GetAllCategories> GetCategory(GetCategoryRequest request, IServerStreamWriter<GetAllCategories> responseStream, ServerCallContext context)
        {

            var dtoObject = await _mediator.Send(new GetCategoryByIdRequest { Id = request.Id });

            var allCategory = dtoObject.Select(grp => new Category
            {
                Id = grp.Id,
                CategoryName = grp.CategoryName,
                Description = grp.Description,
                Quantity = grp.Quantity

            }).ToList();
            GetAllCategories getAllCategories = new GetAllCategories();
            getAllCategories.ItemCategories.AddRange(allCategory);
            await responseStream.WriteAsync(getAllCategories);

            return await Task.FromResult(getAllCategories);
        }
        public override async Task<Category> Post(Category request,  ServerCallContext context)
        {

            var dtoObject = await _mediator.Send(new CreateCategoryRequest { Id = request.Id, CategoryName= request.CategoryName, Description = request.Description, Quantity= request.Quantity });

            
            return await Task.FromResult(new Category
            {
                Id = dtoObject.Id,
                CategoryName = dtoObject.CategoryName,
                Description = dtoObject.Description,
                Quantity = dtoObject.Quantity

            });
        }
        public override async Task<Category> Update(Category request,  ServerCallContext context)
        {

            var dtoObject = await _mediator.Send(new UpdateCategoryRequest { Id = request.Id, CategoryName = request.CategoryName, Description = request.Description, Quantity = request.Quantity });


            return await Task.FromResult(new Category
            {
                Id = dtoObject.Id,
                CategoryName = dtoObject.CategoryName,
                Description = dtoObject.Description,
                Quantity = dtoObject.Quantity

            });
        }
        public override async Task<Empty> Remove(GetCategoryRequest request,  ServerCallContext context)
        {

          await _mediator.Send(new RemoveCategoryRequest { Id = request.Id });


            return await Task.FromResult(new Empty());
        }


    }
}

