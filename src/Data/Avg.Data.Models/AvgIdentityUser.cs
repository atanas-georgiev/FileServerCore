namespace Avg.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Avg.Data.Common;
    using Avg.Data.Common.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class AvgIdentityUser : IdentityUser, IHavePrimaryKey<string>, IAuditInfo, IDeletableEntity
    {
        public DateTime CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        public bool IsDeleted { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string PasswordAnswerHash { get; set; }

        [MaxLength(100)]
        public string PasswordQuestion { get; set; }
    }
}