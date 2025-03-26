using bookify_data.Data;
using bookify_data.Helper;
using bookify_data.Interfaces;
using bookify_data.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddRepository(this IServiceCollection service)
		{
            service.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
			service.AddTransient(typeof(IRepository<>), typeof(Repository<>));
			service.AddTransient<IAuthenRepository, AuthenRepository>();
			service.AddTransient<IAccountRepository, AccountRepository>();
            service.AddTransient<IOrderRepository, OrderRepository>();
            service.AddTransient<IVoucherRepository, VoucherRepository>();
            service.AddTransient<IOrderDetailRepository, OrderDetailRepository>();
            service.AddTransient<ICategoryRepository, CategoryRepository>();
            service.AddTransient<IBookCategoryRepository, BookCategoryRepository>();
            service.AddTransient<IBookRepository, BookRepository>();
            service.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            service.AddScoped<IFeedbackRepository, FeedbackRepository>();
            service.AddScoped<IPaymentRepository, PaymentRepository>();

            return service;
		}
	}
}
