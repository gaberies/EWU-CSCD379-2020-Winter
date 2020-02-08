using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SecretSanta.Business.Dto
{
    public class UserInput
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }

        [Required]
        public int? SantaId { get; set; }

        public User? Santa { get; set; }

        public IList<Gift> Gifts { get; } = new List<Gift>();
    }
}