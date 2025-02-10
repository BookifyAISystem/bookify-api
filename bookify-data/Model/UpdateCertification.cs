using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
	public class AddCertification
	{
		public List<IFormFile> Certification { get; set; }
	}
	public class UpdateCertification
	{
		public string CertificationName { get; set; }
		public IFormFile Certification { get; set; }
	}
}
