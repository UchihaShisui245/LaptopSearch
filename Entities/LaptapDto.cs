using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApps.Models;

namespace WebApps.Entities
{
    public class LaptapDto
    {
        
        public string Name { get; set; }

        public IEnumerable<SelectListItem> LaptopDropDown { get; set; }

        public IEnumerable<Laptop> LaptopTable { get; set; }
    }
}
