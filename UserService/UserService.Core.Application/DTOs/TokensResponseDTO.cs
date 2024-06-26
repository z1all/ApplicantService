﻿using System.ComponentModel.DataAnnotations;

namespace UserService.Core.Application.DTOs
{
    public class TokensResponseDTO
    {
        [Required]
        public string Refresh { get; set; } = null!;
        [Required]
        public string Access { get; set; } = null!;
    }
}
