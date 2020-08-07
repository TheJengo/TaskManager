using AutoMapper;
using Core.Entity.Concrete;
using Core.Extensions;
using Entity.Concrete;
using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Mappings.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserListDto>();
            //.ForPath(dest => dest.IsAdmin, source => source.MapFrom(x => x.UserOperationClaims.Count > 1));
            CreateMap<UserListDto, User>();
            CreateMap<User, UserDetailsDto>().ReverseMap();
            CreateMap<UserDetailsDto, UserListDto>().ReverseMap();
            CreateMap<UserTask, UserTaskDto>()
                .ForPath(dest => dest.ScheduleTypeName, source => source.MapFrom(x => x.TaskType.Name))
                .ForPath(dest => dest.TaskTypeId, source => source.MapFrom(x => x.TaskType.Id))
                .ReverseMap()
                .ForMember(dest => dest.TaskType, dest => dest.Ignore());
        }
    }
}
