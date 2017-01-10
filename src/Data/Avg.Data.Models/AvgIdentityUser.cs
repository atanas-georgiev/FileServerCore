namespace Avg.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Avg.Data.Common;
    using Avg.Data.Common.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class AvgIdentityUser : IdentityUser, IHavePrimaryKey<string>, IAuditInfo, IDeletableEntity
    {
        public byte[] Avatar { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string FirstName { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string LastName { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}