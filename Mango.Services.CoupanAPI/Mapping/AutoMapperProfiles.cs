using System;
using AutoMapper;
using Mango.Services.CoupanAPI.Models;
using Mango.Services.CoupanAPI.Models.DTO;

namespace Mango.Services.CouponAPI.Mapping
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<Coupon, CouponDto>().ReverseMap();
            CreateMap<CouponDto, Coupon>().ReverseMap();
        }
	}
}

