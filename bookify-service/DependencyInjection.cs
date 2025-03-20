using bookify_service.Interfaces;
using bookify_service.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddServices(this IServiceCollection service)
		{
			service.AddTransient<IAuthenServices, AuthenServices>();
			service.AddTransient<IAccountService, AccountService>();
			service.AddTransient<IOrderService, OrderService>();
            service.AddTransient<IOrderDetailService, OrderDetailService>();
            service.AddTransient<IVoucherService, VoucherService>();
            service.AddTransient<ICategoryService, CategoryService>();
            service.AddTransient<IBookCategoryService, BookCategoryService>();
            service.AddTransient<IFeedbackService, FeedbackService>();
            service.AddTransient<IVnpayService, VnpayService>();

            return service;
		}
	}
}
