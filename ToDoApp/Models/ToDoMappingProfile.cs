using AutoMapper;
using ToDoApp.ViewModels;

namespace ToDoApp.Models
{
    public class ToDoMappingProfile : Profile
    {
        public ToDoMappingProfile()
        {
            CreateMap<ToDoModel, ToDoViewModel>()
              .ForMember(o => o.TaskId, ex => ex.MapFrom(o => o.Id))
              .ReverseMap();
        }
    }
}
