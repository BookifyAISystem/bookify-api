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
			//Voucher
            CreateMap<Voucher, GetVoucherDTO>();
            CreateMap<AddVoucherDTO, Voucher>();
			CreateMap<UpdateVoucherDTO, Voucher>();
			//Order
			CreateMap<Order, GetOrderDTO>();
            CreateMap<GetOrderDTO,Order>();
            CreateMap<AddEmptyOrderDTO, Order>();
            CreateMap<UpdateOrderDTO, Order>();
            //Order Detail
            CreateMap<OrderDetail, GetOrderDetailDTO>();
            CreateMap<AddOrderDetailDTO, OrderDetail>();
            CreateMap<UpdateOrderDetailDTO, OrderDetail>();
            //Category
            CreateMap<Category, GetCategoryDTO>();
            CreateMap<AddCategoryDTO, Category>();
            CreateMap<UpdateCategoryDTO, Category>();


        }
	}
}
