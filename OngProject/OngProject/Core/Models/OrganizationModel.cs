﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Models
{
    public class OrganizationModel: EntityBase
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Image { get; set; }

        [MaxLength(255)]
        public string Adress { get; set; }

        [MaxLength(20)]
        public int Phone { get; set; }

        [Required]
        [MaxLength(320)]
        public string Email { get; set; }

        [Required]
        [MaxLength(500)]
        public string WelcomeText { get; set; }

        [MaxLength(2000)]
        public string AboutUsText { get; set; }
    }
}
