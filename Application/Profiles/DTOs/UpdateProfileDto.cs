using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles.DTOs
{
    public class UpdateProfileDto
    {
        public required string DisplayName { get; set; }
        public string? Bio { get; set; }
    }
}
