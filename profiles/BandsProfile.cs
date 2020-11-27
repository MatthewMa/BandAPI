using AutoMapper;
using BandAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BandAPI.profiles
{
    public class BandsProfile: Profile
    {
        public BandsProfile()
        {
            CreateMap<Entities.Band, Models.BandDTO>()
                .ForMember(
                    dest => dest.FoundedYearsAgo,
                    opt => opt.MapFrom(src => $"{src.Founded.ToString("yyyy")} ({ src.Founded.GetYearsAgo()}) years ago")
                );
        }
    }
}
