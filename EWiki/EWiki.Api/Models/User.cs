using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EWiki.Api.Models
{
    public class User : EntityBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CurrentLattitude { get; set; }
        public string CurrentLongtitude { get; set; }
        public string AZ { get; set; }
        public IList<string> Roles { get; set; }
        public IList<Claim> Claims { get; set; }
        public string Priority { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }
}
