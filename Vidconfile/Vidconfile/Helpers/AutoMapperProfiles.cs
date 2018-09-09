using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vidconfile.ApiModels;
using Vidconfile.Data.Models;

namespace Vidconfile.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //CreateMap<ModelFrom, ModelTo>();

            CreateMap<Video, GetAllVideoApiModel>();
            CreateMap<Video, GetVideoByIdApiModel>();
        }
    }
}
