using AutoMapper;
using AutoMapper.QueryableExtensions;
using Grpc.Core;
using gRPCRESTAPI.Data;
using gRPCRESTAPI.Models;
using Microsoft.EntityFrameworkCore;
using static gRPCRESTAPI.ItemService;

namespace gRPCRESTAPI.Services
{
    public class ItemService : ItemServiceBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ItemService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public override async Task<CreateItemResponse> CreateItem(CreateItemRequest request, ServerCallContext context)
        {
            if (request.Title == string.Empty || request.Description == string.Empty)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Title and Description MUST be provided"));

            var item = _mapper.Map<Item>(request);
            
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();

            var response = new CreateItemResponse { Id = item.Id };
            return await Task.FromResult(response);
        }

        public override async Task<ReadItemResponse> ReadItem(ReadItemRequest request, ServerCallContext context)
        {
            var item = await (from x in _context.Items where x.Id == request.Id select x).FirstOrDefaultAsync() ??
                throw new RpcException(new Status(StatusCode.NotFound, $"Item with Id {request.Id} not found"));

            var response = _mapper.Map<ReadItemResponse>(item);
            return await Task.FromResult(response);
        }

        public override async Task<ReadItemsResponse> ReadItems(ReadItemsRequest request, ServerCallContext context)
        {
            var items = await (from x in _context.Items select x).ProjectTo<ReadItemResponse>(_mapper.ConfigurationProvider).ToListAsync();
            
            var response = new ReadItemsResponse();
            response.Items.AddRange(items);

            return await Task.FromResult(response);
        }

        public override async Task<UpdateItemResponse> UpdateItem(UpdateItemRequest request, ServerCallContext context)
        {
            if (request.Title == string.Empty || request.Description == string.Empty)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Title and Description MUST be provided"));
            
            var item = await (from x in _context.Items where x.Id == request.Id select x).FirstOrDefaultAsync() ??
                throw new RpcException(new Status(StatusCode.NotFound, $"Item with Id {request.Id} not found"));

            var updatedItem = _mapper.Map(request, item);
            
            _context.Items.Update(updatedItem);
            await _context.SaveChangesAsync();

            return await Task.FromResult(_mapper.Map<UpdateItemResponse>(request));
        }

        public override async Task<DeleteItemResponse> DeleteItem(DeleteItemRequest request, ServerCallContext context)
        {
            var item = await (from x in _context.Items where x.Id == request.Id select x).FirstOrDefaultAsync() ??
                throw new RpcException(new Status(StatusCode.NotFound, $"Item with Id {request.Id} not found"));

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return await Task.FromResult(_mapper.Map<DeleteItemResponse>(request));
        }
    }
}