﻿using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required]
        public DateTime Birthdate { get; set; }

        [Required]
        public string Sex { get; set; } = null!;

        public string MobileNumber { get; set; }

        public string ProfileImageUrl { get; set; }

        [Required]
        public string AboutMe { get; set; } = null!;








    }
}
