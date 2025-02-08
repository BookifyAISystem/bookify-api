using AutoMapper;
using bookify_data.Entities;
using bookify_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static bookify_data.Helper.CacheKey;

namespace bookify_data.Helper
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			
			CreateMap<RegisterRequest, Account>();
			
		}
	}
}
