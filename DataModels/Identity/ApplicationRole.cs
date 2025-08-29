using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Identity
{
    public class ApplicationRole : IdentityRole
    {
		[Column(TypeName = "varchar(36)")]
		[Key]
		public override string Id { get; set; }

	}

}
