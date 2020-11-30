using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BandAPI.profiles
{
    public class AlbumProfile: Profile
    {
        public AlbumProfile()
        {
            CreateMap<Entities.Album, Models.AlbumDTO>().ReverseMap();
            CreateMap<Entities.Album, Models.AlbumCreateDTO>().ReverseMap();
        }
    }
}
