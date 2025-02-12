using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
	public class ResultJs<T>
	{
		public ResultJs()
		{
			status = 404;
		}
		public int status { get; set; }
		public string message { get; set; }
		public T data { get; set; }
		public Object ExtraData { get; set; }
	}
}
