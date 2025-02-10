using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace bookify_data.Model
{
	public class RefreshToken
	{
		[Key]
		[JsonIgnore]
		public long Id { get; set; }//userID
		public decimal userId { get; set; }//userID
		public string Token { get; set; }//refresh_Token
		public DateTime Expires { get; set; }

		[JsonIgnore]
		[NotMapped]
		public bool IsExpired => DateTime.UtcNow >= Expires;
		public DateTime Created { get; set; }
		public string CreatedByIp { get; set; }
		public DateTime? Revoked { get; set; }
		public string RevokedByIp { get; set; }
		public string ReplacedByToken { get; set; }

		[JsonIgnore]
		[NotMapped]
		public bool IsActive => Revoked == null && !IsExpired;
	}
}
