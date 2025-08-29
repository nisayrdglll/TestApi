using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace DataModels.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "varchar(36)")]
        [Key]
        public override string Id { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string? Name { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string? FirstName { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string? LastName { get; set; }

        [Column(TypeName = "integer")]
        public int? DepartmentId { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string? PhoneNumber { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string? TcNumber { get; set; }

        public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "varchar(36)")]
        public string? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [Column(TypeName = "varchar(36)")]
        public string? UpdatedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        [Column(TypeName = "varchar(36)")]
        public string? DeletedBy { get; set; }

        [Column(TypeName = "smallint")]
        [DefaultValue(0)]
        public byte IsDeleted { get; set; }

        public DateTime? LastLogin { get; set; }

        public DateTime? LastActive { get; set; }

        [Column(TypeName = "integer")]
        public int? TitleId { get; set; }

        [Column(TypeName = "varchar(450)")]
        public string? ADUserName { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string? InterPhone { get; set; }

        [Column(TypeName = "varchar(450)")]
        public string? PictureUrl { get; set; }

        [Column(TypeName = "smallint")]
        public byte SmsNotification { get; set; }

        [Column(TypeName = "smallint")]
        public byte EmailNotification { get; set; }

        [Column(TypeName = "smallint")]
        public byte MobileNotification { get; set; }
        [Column(TypeName = "smallint")]
        public byte IsMessageAllowed { get; set; }


        public static implicit operator ClaimsPrincipal(ApplicationUser v)
        {
            throw new NotImplementedException();
        }
    }
}
