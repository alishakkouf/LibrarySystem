using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using TitanTechTask.Domain;

namespace TitanTechTask.Data.Models
{
    public class User : IdentityUser<int>, IAuditedEntity
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        public int UserId { get; set; }   

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string PasswordHash { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public long? ModifierUserId { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public bool? IsDeleted { get; set; } = false;

        public List<Borrowing> Borrowings { get; set; }
    }
}
