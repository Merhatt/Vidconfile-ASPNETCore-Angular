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
            CreateMap<Video, GetAllVideoApiModel>();
            CreateMap<Video, GetVideoByIdApiModel>();
            CreateMap<VidconfileUser, GetUserByVideoIdApiModel>();
            CreateMap<Comment, AddCommentApiModel>();
            CreateMap<Comment, GetAllCommentsApiModel>()
                .ForMember(x => x.AuthorProfilePhotoUrl, o => o.MapFrom(s => s.Author.ProfilePhotoUrl))
                .ForMember(x => x.AuthorUsername, o => o.MapFrom(s => s.Author.Username));
            CreateMap<VidconfileUser, GetUserApiModel>()
                .ForMember(x => x.Videos, o => o.MapFrom(s => s.UploadedVideos));
            CreateMap<VidconfileUser, GetAllUsersApiModel>();
        }
    }
}
