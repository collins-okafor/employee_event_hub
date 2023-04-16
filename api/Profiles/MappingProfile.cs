using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO.AuthDto;
using api.DTO.EmployeeDto;
using api.DTO.EventDto;
using api.Models;
using AutoMapper;

namespace api.Profiles
{
    public class MappingProfile : Profile 
    {
        public MappingProfile()
        {
            // Source ----> Destination
            CreateMap<Employee, GetEmployeeDto>();
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>();

            CreateMap<Event, GetEventDto>();
            CreateMap<CreateEventDto, Event>();
            CreateMap<UpdateEventDto, Event>();

            CreateMap<CheckCredentialsDto, User>();
            CreateMap<RegisterDto, User>();

        }
    }
}