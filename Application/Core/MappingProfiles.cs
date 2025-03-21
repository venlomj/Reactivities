﻿
using Application.Activities.DTOs;
using Application.Profiles.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, Activity>();
            CreateMap<CreateActivityDto, Activity>();
            CreateMap<EditActivityDto, Activity>();

            CreateMap<Activity, ActivityDto>()
                .ForMember(dest => dest.HostDisplayName, opt => opt.MapFrom(src => 
                    src.Attendees.FirstOrDefault(x => x.IsHost)!.User.DisplayName))
                .ForMember(dest => dest.HostId, opt => opt.MapFrom(src => 
                    src.Attendees.FirstOrDefault(x => x.IsHost)!.User.Id));
            CreateMap<ActivityAttendee, UserProfile>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.User.DisplayName))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.User.Bio))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.User.ImageUrl))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id));
        }
    }
}
