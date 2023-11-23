using AutoMapper;
using gRPCRESTAPI.Models;

namespace gRPCRESTAPI.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            // Source --> Desc
            CreateMap<CreateItemRequest, Item>()
                .ForMember(d=> d.Title, opts=> opts.MapFrom(src=> src.Title))
                .ForMember(d=> d.Description, opts=> opts.MapFrom(src=> src.Description));

            CreateMap<Item, ReadItemResponse>()
                .ForMember(d=> d.Id, opts => opts.MapFrom(src=> src.Id))
                .ForMember(d=> d.Title, opts => opts.MapFrom(src=> src.Title))
                .ForMember(d=> d.Description, opts => opts.MapFrom(src=> src.Description))
                .ForMember(d=> d.Status, opts => opts.MapFrom(src=> src.Status));

            CreateMap<UpdateItemRequest, Item>()
                .ForMember(d=> d.Id, opts => opts.MapFrom(src=> src.Id))
                .ForMember(d=> d.Title, opts => opts.MapFrom(src=> src.Title))
                .ForMember(d=> d.Description, opts => opts.MapFrom(src=> src.Description))
                .ForMember(d=> d.Status, opts => opts.MapFrom(src=> src.Status));

            CreateMap<UpdateItemRequest, UpdateItemResponse>();
            CreateMap<DeleteItemRequest, DeleteItemResponse>();
        }
    }
}