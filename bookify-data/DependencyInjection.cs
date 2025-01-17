using bookify_data.Data;
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
			service.AddTransient(typeof(IRepository<>), typeof(Repository<>));
			return service;
		}
	}
}
