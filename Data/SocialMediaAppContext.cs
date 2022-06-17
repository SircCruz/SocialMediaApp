using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Models;

namespace SocialMediaApp.Data
{
    public class SocialMediaAppContext : DbContext
    {
        public SocialMediaAppContext (DbContextOptions<SocialMediaAppContext> options)
            : base(options)
        {
        }

        public DbSet<SocialMediaApp.Models.AccountsModel>? AccountsModel { get; set; }

        public DbSet<SocialMediaApp.Models.PostsModel>? PostsModel { get; set; }
    }
}
